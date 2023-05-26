using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("ファイルのクリア時の画像")] private Sprite clearFile;

    [SerializeField, Header("ファイルのクリアされていない時の画像")] private Sprite noClearFile;

    [SerializeField, Header("ステージ配置時の親オブジェクト")] private GameObject stageParent;

    [SerializeField, Header("ダブルクリックする間隔時間")] private int clickFrameRate;

    [SerializeField, Header("テキスト表示用オブジェクト")] private GameObject textObj;

    [SerializeField, Header("テキスト表示用オブジェクト(NoTapArea)")] private GameObject textNoTapArea;

    private List<GameObject> stages = new List<GameObject>();   //ステージ記憶用
    private int frameCount = 0;                                 //ダブルクリックの間隔をカウント
    private bool oneClick = false;                            　//一回目クリックされた判定
    private bool doubleClick = false;                           //二回目クリックされた判定
    private int stageNumber = 999;                              //ステージの番号
    private int firstStageNumber = 999;                         //最初クリックしたときのステージ番号
    private int secondStageNumber = 999;                        //二回目クリックしたときのステージ番号
    private GameObject textClone = null;                        //README表示用
    private bool isText = false;                                //テキストを選択しているか

    //最初の一回だけ入る
    private bool first1 = true;
    private bool clonefirst = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //最初の生成
        if(clonefirst)
        {
            //ステージ数取得
            int stage = managerAccessor.Instance.dataMagager.stageNum;

            //ステージのクリア状況データ取得用
            int stageClear = 0;

            //テキストファイルの複製時の親、名前、画像の変更
            textClone = Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj);
            textClone.transform.parent = stageParent.transform;
            textClone.transform.GetChild(0).GetComponent<Text>().text = "README.txt";

            //ステージの生成
            for (int i = 0; i < stage; i++)
            {
                //クリア状況データを取得
                stageClear = PlayerPrefs.GetInt("Stage" + i, 0);

                //複製時の親、名前、画像の変更
                stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
                stages[i].transform.parent = stageParent.transform;
                stages[i].transform.GetChild(0).GetComponent<Text>().text = "Stage" + (i + 1) + ".exe";
                if (stageClear == 1) 
                {
                    //クリアされていた時
                    stages[i].GetComponent<Image>().sprite = clearFile;
                }
                else
                {
                    //クリアされていなかった時
                    stages[i].GetComponent<Image>().sprite = noClearFile;
                }
            }
            clonefirst = false;
        }


        //HomeWindow内にカーソルが無いとき
        if (!managerAccessor.Instance.dataMagager.noTapArea)
        {
            //カーソルを合わせたときパネルが出る
            for (int i = 0; i < stages.Count; i++)
            {
                RectTransform stageNum = stages[i].GetComponent<RectTransform>();
                stageNum.transform.GetChild(2).gameObject.SetActive(false);

                //マウスが座標内にいるとき
                if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                    stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                    stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                    stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
                {
                    stageNum.transform.GetChild(2).gameObject.SetActive(true);
                }
            }

            //カーソルを合わせたときパネルが出る
            textClone.transform.GetChild(2).gameObject.SetActive(false);

            //マウスがテキストファイルの座標内にいるとき
            if (textClone.GetComponent<RectTransform>().position.x - textClone.GetComponent<RectTransform>().sizeDelta.x + 60 < Input.mousePosition.x &&
                textClone.GetComponent<RectTransform>().position.x + textClone.GetComponent<RectTransform>().sizeDelta.x - 60 > Input.mousePosition.x &&
                textClone.GetComponent<RectTransform>().position.y - textClone.GetComponent<RectTransform>().sizeDelta.y + 40 < Input.mousePosition.y &&
                textClone.GetComponent<RectTransform>().position.y + textClone.GetComponent<RectTransform>().sizeDelta.y - 60 > Input.mousePosition.y)
            {
                textClone.transform.GetChild(2).gameObject.SetActive(true);
            }


            if (!managerAccessor.Instance.dataMagager.sceneMoveStart)
            {
                //ダブルクリック
                if (Input.GetMouseButton(0))
                {
                    //長押しは反応しない
                    if (first1)
                    {
                        for (int i = 0; i < stages.Count; i++)
                        {
                            //一旦選択状態解除
                            stages[i].GetComponent<RectTransform>().transform.GetChild(1).gameObject.SetActive(false);
                        }
                        textClone.transform.GetChild(1).gameObject.SetActive(false);


                        for (int i = 0; i < stages.Count; i++)
                        {
                            RectTransform stageNum = stages[i].GetComponent<RectTransform>();

                            //マウスが座標内にいるとき
                            if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                                stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                                stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                                stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
                            {
                                //クリックしたステージ記憶
                                stageNumber = i + 1;
                                secondStageNumber = i + 1;
                                //選択したアイコンを選択状態にする
                                stageNum.transform.GetChild(1).gameObject.SetActive(true);
                                isText = false;
                                break;
                            }
                            else
                            {
                                stageNumber = 999;
                                secondStageNumber = 999;
                            }
                        }

                        //一回クリックされたら&&一回目と二回目のステージ番号が同じとき&&クリックした番号が存在しているとき
                        if (oneClick && firstStageNumber == secondStageNumber && secondStageNumber != 999 || isText)
                        {
                            doubleClick = true;
                        }


                        //マウスが座標内にいるとき
                        if (textClone.GetComponent<RectTransform>().position.x - textClone.GetComponent<RectTransform>().sizeDelta.x + 60 < Input.mousePosition.x &&
                            textClone.GetComponent<RectTransform>().position.x + textClone.GetComponent<RectTransform>().sizeDelta.x - 60 > Input.mousePosition.x &&
                            textClone.GetComponent<RectTransform>().position.y - textClone.GetComponent<RectTransform>().sizeDelta.y + 40 < Input.mousePosition.y &&
                            textClone.GetComponent<RectTransform>().position.y + textClone.GetComponent<RectTransform>().sizeDelta.y - 60 > Input.mousePosition.y)
                        {
                            //選択したアイコンを選択状態にする
                            textClone.transform.GetChild(1).gameObject.SetActive(true);
                            isText = true;
                        }

                        oneClick = true;
                        first1 = false;
                        firstStageNumber = secondStageNumber;
                    }
                }
                else
                {
                    first1 = true;
                }
            }
        }

        //ダブルクリックの判定消えるまでの時間計測処理
        if (oneClick)
        {
            if (clickFrameRate == frameCount) 
            {
                oneClick = false;
                isText = false;
                frameCount = 0;
            }
            frameCount++;
        }

        //ダブルクリックに成功したとき
        if (doubleClick)
        {
            //テキストを選択していないとき
            if (!isText)
            {
                if (stageNumber != 999)
                {
                    //ステージ移動
                    managerAccessor.Instance.sceneMoveManager.SceneMoveName("Stage" + stageNumber);
                }
            }
            else
            {
                //テキスト表示
                StartCoroutine("LoadAni");
                isText = false;
                doubleClick = false;
            }
        }
    }



    public void EndText()
    {
        textObj.SetActive(false);
        textNoTapArea.SetActive(false);
        isText = false;
    }

    //テキストの出現アニメーション
    private IEnumerator LoadAni()
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        textObj.SetActive(true);
        textNoTapArea.SetActive(true);
        textClone.transform.GetChild(1).gameObject.SetActive(false);
        managerAccessor.Instance.dataMagager.sceneMoveStart = false;
    }

}
