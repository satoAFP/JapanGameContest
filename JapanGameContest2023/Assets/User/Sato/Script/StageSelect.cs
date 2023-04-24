using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("ステージ配置時の親オブジェクト")] private GameObject stageParent;

    [SerializeField, Header("ダブルクリックする間隔時間")] private int clickFrameRate;

    private List<GameObject> stages = new List<GameObject>();   //ステージ記憶用
    private int frameCount = 0;                                 //ダブルクリックの間隔をカウント
    private bool oneClick = false;                            　//一回目クリックされた判定
    private bool doubleClick = false;                           //二回目クリックされた判定

    //最初の一回だけ入る
    private bool first1 = true;

    // Start is called before the first frame update
    void Start()
    {
        //ステージの生成
        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
            stages[i].transform.parent = stageParent.transform;
            stages[i].transform.GetChild(0).GetComponent<Text>().text = "STAGE" + (i + 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ダブルクリック
        if (Input.GetMouseButton(0))
        {
            //長押しは反応しない
            if (first1)
            {
                //一回クリックされたら
                if(oneClick)
                {
                    doubleClick = true;
                }
                oneClick = true;
                first1 = false;
            }
        }
        else
        {
            first1 = true;
        }

        //ダブルクリックの判定消えるまでの時間計測処理
        if(oneClick)
        {
            if (clickFrameRate == frameCount) 
            {
                oneClick = false;
                frameCount = 0;
            }

            frameCount++;
        }

        //ダブルクリックに成功したとき
        if (doubleClick)
        {
            for (int i = 0; i < stages.Count; i++)
            {

                RectTransform stageNum = stages[i].GetComponent<RectTransform>();
                //マウスが座標内にいるとき
                if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                    stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                    stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                    stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
                {
                    //ステージ移動
                    managerAccessor.Instance.sceneMoveManager.SceneMoveName("Stage" + (i + 1));
                }
            }
        }
    }
}
