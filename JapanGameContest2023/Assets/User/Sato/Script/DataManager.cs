using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField, Header("複製時の移動量")] private Vector3 duplicationMoveAmount;

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

    //主人公が動けるか編集モードに入るか切り替え用フラグ
    [System.NonSerialized] public bool playMode = true;

    //主人公が動けるか編集モードに入るか切り替え用フラグ
    [System.NonSerialized] public int objNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        managerAccessor.Instance.dataMagager = this;
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
        playMode = !playMode;
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
                //既に選択された状態にしておく
                dataManager.selectObjsData.Add(clone);
            }

            //ボタンが押されたらUIが消える
            Destroy(dataManager.rightClickUIClone);
        }
    }

    //複製ボタン用関数
    public void DuplicationButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;


        //以前選択されていたオブジェクトデータ削除
        dataManager.selectObjsData.Clear();

        //その場所に表示
        for (int i = 0; i < dataManager.copyObjsData.Count; i++)
        {
            GameObject clone = Instantiate(dataManager.copyObjsData[i]);
            clone.transform.localPosition += managerAccessor.Instance.dataMagager.duplicationMoveAmount;
            //既に選択された状態にしておく
            dataManager.selectObjsData.Add(clone);
        }

        //ボタンが押されたらUIが消える
        Destroy(dataManager.rightClickUIClone);

    }
}
