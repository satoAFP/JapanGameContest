using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //---------player�֌W�i�ړ��E�W�����v�j�֌W�̕ϐ��錾-----------------

    [SerializeField, Header("�v���C���[���x")] private float speed;//�v���C���[���x

    //�����͂��̂���DataManager�ɕ��荞��
    [System.NonSerialized] public bool setblock;//�����̔��肪�u���b�N�ɓ������Ă�����

    [SerializeField, Header("�v���C���[�㏸�^�C�}�[")] private float uptime;//�v���C���[����x�ɏ㏸�ł��鎞��

    private float fuptime;//uptime�̊J�n���̐��l������

    private float playerSize = 1f; // �v���C���[�̕�

    [SerializeField, Header("�W�����v��")] private float jumpForce = 350f;//�v���C���[�W�����v��

    private bool JumpFlag = false;//���݃W�����v���Ă��邩�̃t���O

    //���݃v���C���[���ړ����Ă��邩�𔻕ʂ���
    bool isMoving = false;

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    private Vector2 firstpos;//�����ʒu�i���j

    private Vector2 playerPosition;//���݂̃v���C���[�̈ʒu

    //-----------Click�֌W�̊֐�--------------------

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    [SerializeField, Header("��������ړ��w�W�I�u�W�F�N�g")]
    private GameObject prefab;

    private GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    private Vector2 mempos;//�O�t���[�����̍��W
    private bool fream_move=false;//�t���[��


    //-----------ray�֌W�̕ϐ��̐錾---------------

    private Vector2 origin_x;//ray�̌��_(X�����j

    private Vector2 direction;//ray�̕����x�N�g��

    private Vector2 offset;//�I�t�Z�b�g�iRay�̊J�n�ʒu)

   // private bool isGrounded; // ���n���Ă��邩�ǂ���

    private bool ray_first = true;//���x��Ray�̏������������Ƃ���񂾂��ʂ�

    private LayerMask layermask;//���C���[�}�X�N

    //[SerializeField] private LayerMask groundlayermask;//�n�ʔ���p�̃��C���[�}�X�N
   
    [SerializeField, Header("�e�X�g�pRay�̒�������")]
    private float ray_length;

    [SerializeField, Header("���n����p��Ray�̒���")] private float g_ray_lenght;


    //-----------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//���W�b�g�{�f�B�̎擾

        firstpos = this.transform.position;//�v���C���[�̏����ʒu���擾

        playerPosition = firstpos;//�ŏ��̓v���C���[�̏����ʒu������

        fuptime = uptime;//�v���C���[�㏸���Ԃ�ۑ�

        // �v���C���[�̒��S����̃I�t�Z�b�g���v�Z����
        offset = new Vector2(0.5f * playerSize, 0f);//�͂��߂͉E����

        //�擾���郌�C���[���l���i���E����p�j
        layermask = LayerMask.GetMask("CreateBlock","Block", "Ground");//�����ɒǉ����������C���[���������layermask�����C���[��������悤�ɂȂ�
        

    }

    private void Update()
    {
        //�N���b�N������Update�ł��܂��傤
      
        //Ray�̌��_���v���C���[�̌��݂̈ʒu
        origin_x = (Vector2)transform.position + offset;//(X�����j
    
        direction = transform.right;//X�������w��

        //�v���C���[�̌����Ă��������Ray���΂�
        RaycastHit2D hit = Physics2D.Raycast(origin_x, direction, ray_length,layermask);

        // Ray�̉���
        Debug.DrawLine(origin_x, origin_x + direction * ray_length, Color.red);//���E����p��Ray
       
        //���E����p��Ray�������������̏���
        if (hit.collider != null)
        {
            Debug.DrawLine(origin_x, hit.point, Color.green);//�f�o�b�O�p��Ray���������鏈��

            // ���������I�u�W�F�N�g�����g�łȂ���΁A��������̏���������
            if (hit.collider.gameObject != gameObject)
            {
                int layer = hit.collider.gameObject.layer;//Ray�����������I�u�W�F�N�g�̃��C���[������
                Debug.Log("���������I�u�W�F�N�g�̃��C���[��" + LayerMask.LayerToName(layer) + "�ł��B");

                //Ray�����������̂��ړ��w�W�I�u�W�F�N�g�̏ꍇ�A�W�����v���������Ȃ�
                if (LayerMask.LayerToName(layer) == "CreateBlock")
                {
                    //Debug.Log("tobanai");
                }
                //�W�����v�������s��
                else 
                {
                    //����̃��C���[�ɂ̂݃W�����v�������s��
                    if(LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
                    {
                        //�ړ������v���C���[�㏸���Ԃ�0�ł͂Ȃ��Ƃ��㏸����
                        if(isMoving)
                        {
                            if(uptime >= 0)
                            {
                                uptime -= Time.deltaTime;//�v���C���[�㏸���Ԍ���
                               
                                this.rb.AddForce(transform.up * jumpForce);
                            }
                            else
                            {
                                MoveFinish();//�v���C���[�㏸���Ԃ�0�ɂȂ�ƃv���C���[�̈ړ����~�߂�
                            }

                        }



                        //�W�����v�t���O��false�̎�&���݃v���C���[���ړ����Ă���Ƃ��A�W�����v�������s
                        //if (!JumpFlag && isMoving)
                        //{
                           
                        //    //������W�����v�������s��Ȃ��悤�ɏ��߂ɓ�������Ray�݂̂𔽉�������
                        //    if (ray_first)
                        //    {
                        //        Debug.Log("J");
                        //        speed = 1.7f;
                        //        this.rb.AddForce(transform.up * jumpForce);
                        //        JumpFlag = true;
                        //        ray_first = false;
                        //    }

                        //}
                    }
                }
                //else
                //{
                //    Debug.Log("soreigai");
                //    //speed = fspeed;
                //    ray_first = true;
                //}
            }

        }
        else
        {
            Debug.Log("�Ȃɂ��������ĂȂ�");
            //speed = fspeed;
            ray_first = true;
        }
       
        // �ړ����łȂ���΃N���b�N���󂯕t����
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("�ړ�");
            // �N���b�N���ꂽ�ʒu���擾
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // z���W��0�ɐݒ�i2D�Q�[���Ȃ̂Łj

            CreateObj = Instantiate(prefab, clickPosition, Quaternion.identity);//�ړ��w�W�I�u�W�F�N�g�쐬
            //ObjCount = CreateObj;//���������I�u�W�F�N�g�����[

            //�N���b�N�����ꏊ�̍��E��������
            if (playerPosition.x < clickPosition.x)//�E
            {
                offset = new Vector2(0.5f * playerSize, 0f);//�E����
                transform.eulerAngles = new Vector3(0, 0, 0);
                //Debug.Log("�E");
            }
            else//��
            {
                offset = new Vector2(-0.5f * playerSize, 0f);//������
                transform.eulerAngles = new Vector3(0, 180, 0);
               // Debug.Log("��");
            }

            // �ړ����J�n
            isMoving = true;
        }

        if(setblock)
        {
            Debug.Log("J");
            uptime = fuptime;
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            //FreezeRotation�̂݃I���ɂ���iFreeze�͏㏑���ł���j
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            if (transform.position.y <= -10)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;//�v���C���[�s�k�t���O��ON
                Destroy(this.gameObject);
            }

           
            // �ړ����̏ꍇ�͈ړ�����
            if (isMoving)
            {
                //Debug.Log("a");
                // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // �ړ����I�������t���O������
                //�O�t���[���̍��W�ƍ��̍��W���ׂāA�ړ��ʂ��ɒ[�ɏ��Ȃ��ꍇ�i�ǂɂԂ����Ă����ԁj�������I��
                if (transform.position.x == clickPosition.x||Mathf.Abs(transform.position.x-mempos.x) < 0.03f)
                {
                    Debug.Log("b");
                    playerPosition = transform.position;//playerPosition���X�V
                   // MoveFinish();//�ړ������I��
                }

                if (transform.position.x == clickPosition.x)
                {
                    Debug.Log("cccc");
                    MoveFinish();//�ړ������I��
                }
            }

            mempos = transform.position;//�O�t���[����ۑ�

        }
        else//�G�f�B�b�g���[�h�̎�
        {
             MoveFinish();//�ړ������I��
            //Rigidbody�𐧌�����
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //�ړ��I�����̏���
    private void MoveFinish()
    {
        if (isMoving)
        {
            Destroy(CreateObj);//�ړ��w�W�I�u�W�F�N�g�폜

            isMoving = false;//�ړ������I��
        }
    }

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {

            JumpFlag = false;
        }
    }

}
