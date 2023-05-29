using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayerController : MonoBehaviour
{
    [SerializeField]private GameObject textMeshObj;

    [SerializeField] private int sortingnum = 0;//�����ŕ`�揇�����߂���

    // Start is called before the first frame update
    void Start()
    {
        //�e�L�X�g���b�V���̕`�揇�ύX
        textMeshObj.gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingnum;
    }

    // Update is called once per frame
    void Update()
    {

    }
}