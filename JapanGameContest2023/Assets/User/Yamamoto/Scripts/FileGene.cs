using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    [SerializeField, Header("��������ړ��w�W�I�u�W�F�N�g")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �N���b�N���ꂽ�ʒu���擾
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // z���W��0�ɐݒ�i2D�Q�[���Ȃ̂Łj
    }
}
