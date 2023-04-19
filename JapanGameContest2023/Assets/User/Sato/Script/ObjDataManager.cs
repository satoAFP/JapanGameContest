using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataManager : MonoBehaviour
{
    [Header("キャンバス")] public GameObject canvas;

    [Header("右クリック時表示UI")] public GameObject rightClickUI;

    [Header("範囲選択表示用オブジェクト")] public GameObject selectionObj;

    [Header("囲う用のドット")] public GameObject dotObj;

    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
