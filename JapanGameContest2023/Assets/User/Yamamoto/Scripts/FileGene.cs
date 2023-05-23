using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    public int playercount = 0;//�v���C���[�̐��𐔂���i�t�@�C���폜���ߗp�j

    public bool posupdate = false;//�v���C���[�ɃN���b�N�������W�Ɉړ������閽�߂𑗂�t���O

    [SerializeField, Header("��������ړ��w�W�I�u�W�F�N�g")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    [SerializeField] private AudioClip appse;//�v���C���[�o��SE�i�����ŏ������R�́A�v���C���[�o��SE���d�Ȃ��Ė炳�Ȃ��悤�ɂ��邽�߁j

    private AudioSource audioSource;

    private bool oneshot = false;//��񂾂�����炷

    // Start is called before the first frame update
    void Start()
    {
        playercount = GameObject.FindGameObjectsWithTag("Player").Length;
        audioSource = GetComponent<AudioSource>();//�X�N���v�g�擾
    }

    // Update is called once per frame
    void Update()
    {
       
        if(managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage1")
        {
            //�X�e�[�W1�ł͂Ȃ��Ƃ�
        }
        else if(!oneshot)
        {
            audioSource.PlayOneShot(appse);//�v���C���[�o��SE�炷
            oneshot = true;
        }

        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            //�Q�[���I�[�o�[�ɂȂ��Ă��Ȃ���Ώ������s
            if (!managerAccessor.Instance.dataMagager.fallDeth ||
           !managerAccessor.Instance.dataMagager.infectionDeth ||
           !managerAccessor.Instance.dataMagager.overDeth)
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


                        posupdate = true;//Player���ɃN���b�N�������W�Ɉړ����閽�߂��o��

                        if (CreateObj == null) //���߂Ĉړ��w�W�I�u�W�F�N�g�����Ƃ�
                        {

                            CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//�ړ��w�W�I�u�W�F�N�g�쐬
                        }
                        else
                        {
                            //���łɈړ��w�W�I�u�W�F�N�g�쐬������Ă���ꍇ
                            Destroy(CreateObj);//�O���������̂��폜
                            CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//�V���Ɉړ��w�W�I�u�W�F�N�g�쐬
                        }




                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        posupdate = false;
                    }
                }



                if (playercount == 0)//�ړ����Ă���v���C���[��0�ɂȂ��CreateObj�폜
                {
                    Destroy(CreateObj);
                    managerAccessor.Instance.dataMagager.isMoving = false;//�v���C���[�S�̂̈ړ��������I��
                    playercount = GameObject.FindGameObjectsWithTag("Player").Length;//�v���C���[�̐����ăJ�E���g
                }
            }

        }

        �@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@
    }
}
