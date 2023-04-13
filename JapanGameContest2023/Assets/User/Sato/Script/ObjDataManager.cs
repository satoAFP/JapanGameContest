using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataManager : MonoBehaviour
{
    [Header("�L�����o�X")] public GameObject canvas;

    [Header("�E�N���b�N���\��UI")] public GameObject rightClickUI;

    [Header("�͈͑I��\���p�I�u�W�F�N�g")] public GameObject selectionObj;

    [Header("�͂��p�̃h�b�g")] public GameObject dotObj;

    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
