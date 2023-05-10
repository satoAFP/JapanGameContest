using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    public int playercount = 0;//�v���C���[�̐��𐔂���

    [SerializeField, Header("��������ړ��w�W�I�u�W�F�N�g")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    private bool nocreate = false;//�I�u�W�F�N�g�����������Ȃ��t���O


    // Start is called before the first frame update
    void Start()
    {
        playercount = GameObject.FindGameObjectsWithTag("Player").Length;  
    }

    // Update is called once per frame
    void Update()
    {
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {

            //Decoy�t�@�C���ɂӂꂽ�Ƃ����Ƃ�t�@�C��������
            if (managerAccessor.Instance.dataMagager.onDecoyFile)
            {
                Destroy(CreateObj);
            }

            if (!managerAccessor.Instance.dataMagager.noTapArea)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // �N���b�N���ꂽ�ʒu���擾
                    managerAccessor.Instance.dataMagager.clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    managerAccessor.Instance.dataMagager.clickPosition.z = 0; // z���W��0�ɐݒ�i2D�Q�[���Ȃ̂Łj


                    if (CreateObj == null) //���߂Ĉړ��w�W�I�u�W�F�N�g�����Ƃ�
                    {

                        CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//�ړ��w�W�I�u�W�F�N�g�쐬
                        nocreate = true;
                    }
                    else
                    {
                        //���łɈړ��w�W�I�u�W�F�N�g�쐬������Ă���ꍇ
                        Destroy(CreateObj);//�O���������̂��폜
                        CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//�V���Ɉړ��w�W�I�u�W�F�N�g�쐬
                    }




                }
            }

                

            if (playercount == 0)//�ړ����Ă���v���C���[��0�ɂȂ��CreateObj�폜
            {
                Destroy(CreateObj);
                nocreate = false;
                playercount = GameObject.FindGameObjectsWithTag("Player").Length;//�v���C���[�̐����ăJ�E���g
            }
           

        }

        
    }
}
