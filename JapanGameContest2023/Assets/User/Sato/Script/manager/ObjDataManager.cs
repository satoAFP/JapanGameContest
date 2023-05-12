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

    [Header("ゴールを格納している親")] public GameObject goalParent;

    [Header("範囲選択するオブジェクト")] public RangeSelection rangeSelection;

    [Header("モードチェンジで画像を表示しているオブジェクト")] public GameObject modeChangeObj;

    [Header("モードチェンジで使用する画像(playmode)")] public Sprite playModeImg;

    [Header("モードチェンジで使用する画像(editmode)")] public Sprite editModeImg;

    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
