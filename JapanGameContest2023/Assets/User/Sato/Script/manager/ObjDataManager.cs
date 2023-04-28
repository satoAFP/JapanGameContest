using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataManager : MonoBehaviour
{
    [Header("キャンバス")] public GameObject canvas;

    [Header("右クリック時表示UI")] public GameObject rightClickUI;

    [Header("編集中背景を変えるパネル")] public GameObject editPanel;

    [Header("範囲選択表示用オブジェクト")] public GameObject selectionObj;

    [Header("囲う用のドット")] public GameObject dotObj;

    [Header("ステージ選択用オブジェクト")] public GameObject stageSelectObj;

    [Header("コピーできるオブジェクトの親")] public GameObject blockParent;

    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
