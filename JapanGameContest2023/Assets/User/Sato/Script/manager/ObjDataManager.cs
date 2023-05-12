using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataManager : MonoBehaviour
{
    [Header("�L�����o�X")] public GameObject canvas;

    [Header("�E�N���b�N���\��UI")] public GameObject rightClickUI;

    [Header("�ҏW���w�i��ς���p�l��")] public GameObject editPanel;

    [Header("�͈͑I��\���p�I�u�W�F�N�g")] public GameObject selectionObj;

    [Header("�͂��p�̃h�b�g")] public GameObject dotObj;

    [Header("�X�e�[�W�I��p�I�u�W�F�N�g")] public GameObject stageSelectObj;

    [Header("�R�s�[�ł���I�u�W�F�N�g�̐e")] public GameObject blockParent;

    [Header("�S�[�����i�[���Ă���e")] public GameObject goalParent;

    [Header("�͈͑I������I�u�W�F�N�g")] public RangeSelection rangeSelection;

    [Header("���[�h�`�F���W�ŉ摜��\�����Ă���I�u�W�F�N�g")] public GameObject modeChangeObj;

    [Header("���[�h�`�F���W�Ŏg�p����摜(playmode)")] public Sprite playModeImg;

    [Header("���[�h�`�F���W�Ŏg�p����摜(editmode)")] public Sprite editModeImg;

    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
