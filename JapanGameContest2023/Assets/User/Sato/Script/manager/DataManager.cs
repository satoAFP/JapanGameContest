using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //選択されたオブジェクト
    [System.NonSerialized] public List<GameObject> selectObjsData = new List<GameObject>();
    //コピー用選択されたオブジェクト
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();

    //コピー用右クリックした時呼ばれるUI
    [System.NonSerialized] public GameObject rightClickUIClone = null;

    //コピーを選択したか判断用
    [System.NonSerialized] public bool objsCopy = false;
    //コピーデータの削除を最初の一回しか行わないためのフラグ
    [System.NonSerialized] public bool copyReset = true;

    //時間表示用テキスト
    [System.NonSerialized] public string timeText = null;

    //オブジェクトが最大数になった時変わるフラグ
    [System.NonSerialized] public bool objMaxFrag = false;

    //オブジェクトを選択した時、縁に乗ると変わる
    [System.NonSerialized] public bool onEdge = false;

    //乗ってはいけないブロックにオブジェクトがあるときモードチェンジできない
    [System.NonSerialized] public bool onBlock = false;

    //どの縁に乗っているか(8は何も入っていないデータ)
    [System.NonSerialized] public int whereEdge = 8;

    //NoTapAreaにカーソルがあるとき切り替わる
    [System.NonSerialized] public bool noTapArea = false;

    //主人公が動けるか編集モードに入るか切り替え用フラグ
    [System.NonSerialized] public bool playMode = true;

    //主人公が動けるか編集モードに入るか切り替え用フラグ
    [System.NonSerialized] public int objNum = 0;

    //主人公が消失した時のフラグ
    [System.NonSerialized] public bool playerlost = false;

    //マウスがクリックした座標を取得
    [System.NonSerialized] public Vector3 clickPosition;

    //プレイヤーの数を数える（各プレイヤーの移動をしているかを確認するために使う）
    [System.NonSerialized] public int playercount = 0;

    //主人公の移動フラグ
    [System.NonSerialized] public bool isMoving = false;

    //主人公がDecoyファイルに触れたとき
    [System.NonSerialized] public bool onDecoyFile = false;

    //ゴールに入った主人公の数
    [System.NonSerialized] public int goalPlayerNum = 0;


    //シーン切り替え開始
    [System.NonSerialized] public bool sceneMoveStart = false;


    //落下ししたとき
    [System.NonSerialized] public bool fallDeth = false;
    //ウイルスに感染したとき
    [System.NonSerialized] public bool infectionDeth = false;
    //CPU使用量を超えたとき
    [System.NonSerialized] public bool overDeth = false;



    [Header("全ステージ数")] public int stageNum;


    private GameObject clonePanel = null;


    //時間表示用
    private int frame = 0;
    private int second = 0;
    private int minute = 0;

    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        managerAccessor.Instance.dataMagager = this;
    }

    private void FixedUpdate()
    {
        managerAccessor.Instance.dataMagager.TimeCount();
    }


    //時間表示用
    public void TimeCount()
    {
        frame++;

        if (frame >= 50)
        {
            second++;
            frame = 0;
        }

        if (second >= 60)
        {
            minute++;
            second = 0;
        }

        managerAccessor.Instance.dataMagager.timeText = minute.ToString("d2") + " : " + second.ToString("d2");
    }


    //マウス座標をワールド座標変換関数
    public Vector3 MouseWorldChange()
    {
        //選択開始時の初期位置記憶
        Vector3 mousePos = Input.mousePosition;
        // Z軸修正
        mousePos.z = 10f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return screenToWorldPointPosition;
    }

    //プレイモード切替関数
    public void ModeChange()
    {
        if (!managerAccessor.Instance.dataMagager.isMoving &&
            !managerAccessor.Instance.dataMagager.onBlock) 
        {
            //プレイモード切替関数
            playMode = !playMode;

            //オブジェクトの数が限界を超えていた時切り替えると負け
            if (managerAccessor.Instance.dataMagager.objMaxFrag)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
                managerAccessor.Instance.dataMagager.overDeth = true;
            }

            //パネルの複製および削除
            if (playMode)
            {
                Destroy(clonePanel);

                //モードチェンジの画像変更
                managerAccessor.Instance.objDataManager.modeChangeObj.GetComponent<Image>().sprite = managerAccessor.Instance.objDataManager.playModeImg;
            }
            else
            {
                clonePanel = Instantiate(managerAccessor.Instance.objDataManager.editPanel);
                clonePanel.transform.position = new Vector3(0, 0, 0);

                //モードチェンジの画像変更
                managerAccessor.Instance.objDataManager.modeChangeObj.GetComponent<Image>().sprite = managerAccessor.Instance.objDataManager.editModeImg;
            }
        }
    }

    //コピーボタン用関数
    public void CopyButton()
    {
        if (managerAccessor.Instance.dataMagager.copyObjsData.Count != 0)
        {
            //コピーが押されている判定
            managerAccessor.Instance.dataMagager.objsCopy = true;

            //ボタンが押されたらUIが消える
            Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);

        }
    }

    //ペーストボタン用関数
    public void PasteButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //コピーが押されているとき
        if (managerAccessor.Instance.dataMagager.objsCopy)
        {

            //マウスと元あったオブジェクトとの移動量計算
            Vector3 moveAmount = MouseWorldChange() - dataManager.copyObjsData[0].transform.localPosition;

            //以前選択されていたオブジェクトデータ削除
            dataManager.selectObjsData.Clear();

            //その場所に表示
            for (int i = 0; i < dataManager.copyObjsData.Count; i++)
            {
                GameObject clone = Instantiate(dataManager.copyObjsData[i]);
                clone.transform.localPosition += moveAmount;
                clone.transform.parent = managerAccessor.Instance.objDataManager.blockParent.transform;
            }


            //ボタンが押されたらUIが消える
            Destroy(dataManager.rightClickUIClone);
        }
    }

    //削除ボタン用関数
    public void DeleteButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //生成されているオブジェクト削除
        for (int i = 0; i < dataManager.copyObjsData.Count; i++)
        {
            Destroy(dataManager.selectObjsData[i]);
        }

        //以前選択されていたオブジェクトデータ削除
        dataManager.selectObjsData.Clear();

        //ボタンが押されたらUIが消える
        Destroy(dataManager.rightClickUIClone);

    }

    
}
