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

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    private bool moving = false;//�v���C���[�e���̈ړ��t���O

    //private Vector2 firstpos;//�����ʒu�i���j

    //private Vector2 playerPosition;//���݂̃v���C���[�̈ʒu

    //[SerializeField]private Vector2 mempos;//�P�O�̃t���[���ł̈ړ����̃v���C���[�̈ʒu

    //GetComponent��p����Animator�R���|�[�l���g�����o��.
    [SerializeField] private Animator animator;

    //-----------Click�֌W�̊֐�--------------------

    private FileGene script;//FileGene�X�N���v�g

    [System.NonSerialized]
    public GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    //private Vector2 mempos;//�O�t���[�����̍��W
   
    //-----------ray�֌W�̕ϐ��̐錾---------------

    private Vector2 origin_x;//ray�̌��_(X�����j

    private Vector2 direction;//ray�̕����x�N�g��

    private Vector2 offset;//�I�t�Z�b�g�iRay�̊J�n�ʒu)

    // private bool isGrounded; // ���n���Ă��邩�ǂ���

    [SerializeField]private bool ray_hit = false;//Ray���������Ă�����

    private LayerMask layermask;//���C���[�}�X�N

    [SerializeField, Header("Ray�̒��������ł����")]
    private float ray_length;


    //-----------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//���W�b�g�{�f�B�̎擾

        //firstpos = this.transform.position;//�v���C���[�̏����ʒu���擾

        //playerPosition = firstpos;//�ŏ��̓v���C���[�̏����ʒu������

        //mempos = new Vector2(0, 0);//������

        script = GameObject.Find("Clickjudge").GetComponent<FileGene>();//FileGene�X�N���v�g�擾

        fuptime = uptime;//�v���C���[�㏸���Ԃ�ۑ�

        // �v���C���[�̒��S����̃I�t�Z�b�g���v�Z����
        offset = new Vector2(0.5f * playerSize, 0f);//�͂��߂͉E����

        //�擾���郌�C���[���l���i���E����p�j
        layermask = LayerMask.GetMask("CreateBlock","Block", "Ground");//�����ɒǉ����������C���[���������layermask�����C���[��������悤�ɂȂ�

        

    }

    private void Update()
    {
        //�N���b�N������Update�ł��܂��傤

        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            //Ray�̌��_���v���C���[�̌��݂̈ʒu
            origin_x = (Vector2)transform.position + offset;//(X�����j

            direction = transform.right;//X�������w��

            //�v���C���[�̌����Ă��������Ray���΂�
            RaycastHit2D hit = Physics2D.Raycast(origin_x, direction, ray_length, layermask);

            // Ray�̉���
            Debug.DrawLine(origin_x, origin_x + direction * ray_length, Color.red);//���E����p��Ray


            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerStart"))
            {
                animator.SetBool("StartAnim", true);//�ړ����̃A�j���[�V�����ɐ؂�ւ�
                Debug.Log("Animation finished");
            }

            //���E����p��Ray�������������̏���
            if (hit.collider != null)
            {
                Debug.DrawLine(origin_x, hit.point, Color.green);//�f�o�b�O�p��Ray���������鏈��

                // ���������I�u�W�F�N�g�����g�łȂ���΁A��������̏���������
                if (hit.collider.gameObject != gameObject)
                {
                    int layer = hit.collider.gameObject.layer;//Ray�����������I�u�W�F�N�g�̃��C���[������
                    Debug.Log("���������I�u�W�F�N�g�̃��C���[��" + LayerMask.LayerToName(layer) + "�ł��B");

                    //����̃��C���[�ɂ̂݃W�����v�������s��
                    if (LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
                    {
                        //�ړ����̂Ƃ��̂�Ray�������������Ƃɂ���
                        if (moving)
                        {
                            ray_hit = true;//Ray���������Ă���
                        }
                    }

                }

            }
            else
            {
                ray_hit = false;//Ray��������Ȃ�
            }


            if (!managerAccessor.Instance.dataMagager.noTapArea)
            {
                // �ړ����łȂ���΃N���b�N���󂯕t����
                if (Input.GetMouseButtonDown(0) && setblock)
                {

                    // CreateObj = Instantiate(prefab, clickPosition, Quaternion.identity);//�ړ��w�W�I�u�W�F�N�g�쐬

                    //�N���b�N�����ꏊ�̍��E��������
                    if (transform.position.x < managerAccessor.Instance.dataMagager.clickPosition.x)//�E
                    {
                        offset = new Vector2(0.5f * playerSize, 0f);//�E����
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        Debug.Log("�E");
                    }
                    else//��
                    {
                        offset = new Vector2(-0.5f * playerSize, 0f);//������
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        Debug.Log("��");
                    }

                    // �ړ����J�n
                    managerAccessor.Instance.dataMagager.isMoving = true;//�v���C���[�S�̂̈ړ�����
                    moving = true;//���̃v���C���[���g�̈ړ��t���O��ON

                }
            }
               

            if(moving)
            {
                animator.SetBool("Moving", true);//�ړ����̃A�j���[�V�����ɐ؂�ւ�
            }
            else
            {
                animator.SetBool("Moving", false);//��~���̃A�j���[�V�����ɐ؂�ւ�
            }

            if (setblock)
            {
                uptime = fuptime;
            }

        }
    
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            //FreezeRotation�̂݃I���ɂ���iFreeze�͏㏑���ł���j
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //���������@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            if (transform.position.y <= -10)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;//�v���C���[�s�k�t���O��ON
                managerAccessor.Instance.dataMagager.fallDeth = true;//�������̔���擾
                Destroy(this.gameObject);
            }

            

            // �ړ����̏ꍇ�͈ړ�����
            if (moving)
            {
                //if (mempos.x != transform.position.x || mempos.y != transform.position.y)//�O�t���[���Ɣ�r���v���C���[���S�������Ȃ�������A�ړ��I��
                //{
                //    //playerPosition = mempos;
                //    Debug.Log("2");
                //}
                //else
                //{
                //    //MoveFinish();
                //    Debug.Log("3");
                //}


                //mempos = transform.position;//�O�t���[����ۑ�

                // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(managerAccessor.Instance.dataMagager.clickPosition.x, transform.position.y), speed * Time.deltaTime);

                
                // �ړ����I�������t���O������
                if (transform.position.x == managerAccessor.Instance.dataMagager.clickPosition.x)
                {
                    //Debug.Log("cccc");
                    MoveFinish();//�ړ������I��
                }

            }
          
            //Ray���������Ă�����㏸���鏈�����J�n
            if(ray_hit)
            {
               
                if (uptime >= 0)
                {
                   // Debug.Log("������");
                    uptime -= Time.deltaTime;//�v���C���[�㏸���Ԍ���

                    this.rb.AddForce(transform.up * jumpForce);
                }
                else
                {
                    MoveFinish();//�v���C���[�㏸���Ԃ�0�ɂȂ�ƃv���C���[�̈ړ����~�߂�
                }
            }
           

          
            

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
        if (moving)
        {
            script.playercount--;//�v���C���[�̐�-1

            ray_hit = false;//�ړ��I����ɍēx��΂Ȃ��悤��Ray�̃t���O��؂�

            //playerPosition = transform.position;//�v���C���[���������ꏊ���擾����

            moving = false;//�ړI�n�ɂ��ǂ蒅�����v���C���[�̈ړ������I��
        }
    }

    //�����蔻��
    private void OnTriggerEnter2D(Collider2D other)
    {
        //�S�[��������
        if (other.gameObject.CompareTag("Goal"))
        {
            //�܂��N���S�[�����Ă��Ȃ��Ƃ�
            if (other.gameObject.GetComponent<Goal>().goalChara)
            {
                //�S�[�����Ă���L�����̃J�E���g�v���X
                managerAccessor.Instance.dataMagager.goalPlayerNum++;
                other.gameObject.GetComponent<Goal>().goalChara = false;
                script.playercount--;//�v���C���[�̐�-1
                Destroy(gameObject);//���g���폜
            }
        }
    }
 
}
