using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> selectObjs = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
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

    public void CopyButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        dataManager.copyObjsData.Clear();
        for (int i = 0; i < dataManager.selectObjs.Count; i++)
        {
            Debug.Log("aaa");
            dataManager.copyObjsData.Add(dataManager.selectObjs[i]);
        }
    }
}
