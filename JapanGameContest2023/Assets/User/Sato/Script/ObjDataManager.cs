using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataManager : MonoBehaviour
{
    [Header("�L�����o�X")] public GameObject canvas;

    [Header("�E�N���b�N���\��UI")] public GameObject rightClickUI;


    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.objDataManager = this;
    }

}
