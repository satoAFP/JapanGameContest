using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> selectObjsData = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();

    [System.NonSerialized] public GameObject rightClickUIClone = null;

    [System.NonSerialized] public bool objsCopy = false;
    [System.NonSerialized] public bool copyReset = true;

    [System.NonSerialized] public bool playMode = true;

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

    public void ModeChange()
    {
        playMode = !playMode;
    }

    public void CopyButton()
    {
        objsCopy = true;
    }

    public void PasteButton()
    {
        Debug.Log(managerAccessor.Instance.dataMagager.copyObjsData[0].name);
        Vector3 moveAmount = MouseWorldChange() - managerAccessor.Instance.dataMagager.copyObjsData[0].transform.localPosition;

        for (int i = 0; i < managerAccessor.Instance.dataMagager.copyObjsData.Count; i++) 
        {
            GameObject clone = Instantiate(managerAccessor.Instance.dataMagager.copyObjsData[i]);
            clone.transform.localPosition += moveAmount;
        }
    }
}
