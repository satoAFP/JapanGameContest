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

    [SerializeField, Header("�W�����v��")] private float jumpForce = 350f;//�v���C���[�W�����v��

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    private bool moving = false;//�v���C���[�e���̈ړ��t���O

    public bool Objhit = false;//�ǂ�u���b�N��o�邱�Ƃ�������t���O

    public bool TimeStart = false;//uptime�J�n�̃t���O

    //-----------Click�֌W�̊֐�--------------------

    private FileGene script;//FileGene�X�N���v�g

   
    //-----------�A�j���[�V�����֌W�̕ϐ��̐錾---------------

    //GetComponent��p����Animator�R���|�[�l���g�����o��.
    [SerializeField] private Animator animator;

    public bool StartAction = false;//�A�j���[�V�����I����Ƀv���C���[�̏����J�n

    //-----------------------------------------------


    [SerializeField] private Vector2 origin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//���W�b�g�{�f�B�̎擾


        script = GameObject.Find("Clickjudge").GetComponent<FileGene>();//FileGene�X�N���v�g�擾

        fuptime = uptime;//�v���C���[�㏸���Ԃ�ۑ�

    }

    private void Update()
    {
        //�N���b�N������Update�ł��܂��傤
        bool stage1 = animator.GetBool("Stage1");//�v���C���[�A�j���[�^�[����bool�^��Stage1�������Ă���

        //�X�e�[�W1�̎��̂ݓo��A�j���[�V��������ʊO�������Ă���A�j���[�V�����ɂ���
        if (managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage1" && stage1)
        {
            Debug.Log("�X�e�[�W1�ł���");
            animator.Play("Stage1PlayerStart");
            animator.SetBool("Stage1", false);//��x�����A�j���[�V�����Đ������邽��false��
        }

       
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            animator.SetFloat("AniSpeed", 0.6f); //�A�j���[�V�������Đ�������

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stage1PlayerStart"))
            {
                StartAction = false;//�o��A�j���[�V�����Đ����̂��߈ړ��������N���b�N���������Ȃ�
                Debug.Log("Animation finished");
            }
            else
            {
                Debug.Log("BB");
                StartAction = true;
            }


            if(Objhit)//�v���C���[�̌����Ă�������ɂ��锻��ɕ�or�u���b�N���������Ă���Ƃ�
            {
                if(moving)//�Ȃ����v���C���[�������Ă���Ƃ��ɏ㏸������
                {
                    TimeStart = true;
                }
            }
            else
            {
                TimeStart = false;
            }

            if (!managerAccessor.Instance.dataMagager.noTapArea�@&& !managerAccessor.Instance.dataMagager.objMaxFrag)
            {
                if(StartAction)
                {

                    // �ړ����łȂ���΃N���b�N���󂯕t����
                    if (script.posupdate && setblock)
                    {
                        //���ړ����ɍēx���͂��Ă����E������󂯕t���Ȃ��s��I�I

                        //�N���b�N�����ꏊ�̍��E��������
                        if (origin.x <= managerAccessor.Instance.dataMagager.clickPosition.x)//�E
                        {
                            //offset = new Vector2(0.5f * playerSize, 0f);//�E����
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("�E");
                        }
                        else//��
                        {
                            // offset = new Vector2(-0.5f * playerSize, 0f);//������
                            transform.eulerAngles = new Vector3(0, 180, 0);
                            Debug.Log("��");
                        }

                      
                        // �ړ����J�n
                        managerAccessor.Instance.dataMagager.isMoving = true;//�v���C���[�S�̂̈ړ�����
                        moving = true;//���̃v���C���[���g�̈ړ��t���O��ON

                    }
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
                animator.SetBool("Wallhit", false);//�ǂ��痎����A�j���[�V�����I��
                uptime = fuptime;
            }

        }
    
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
            //���̃v���C���[���Q�[���I�[�o�[�ɂȂ�Ǝ��M�̈ړ��������~�߂�
            if (managerAccessor.Instance.dataMagager.playerlost || managerAccessor.Instance.dataMagager.isShutDown)
            {
                Debug.Log("��������");
                MoveFinish();//�ړ������I��
            }

            //FreezeRotation�̂݃I���ɂ���iFreeze�͏㏑���ł���j
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            rb.WakeUp();//�����Ă��Ȃ��ƃ��W�b�g�{�f�B���~�܂��Ă��܂��̂ł����ōċN��

            

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
                if(!managerAccessor.Instance.dataMagager.playerlost || !managerAccessor.Instance.dataMagager.isShutDown)
                {
                    // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(managerAccessor.Instance.dataMagager.clickPosition.x, transform.position.y), speed * Time.deltaTime);

                    origin = transform.position;

                    // �ړ����I�������t���O������
                    if (transform.position.x == managerAccessor.Instance.dataMagager.clickPosition.x)
                    {
                        //Debug.Log("cccc");
                        MoveFinish();//�ړ������I��
                    }
                }
               
               

            }
          
            //�v���C���[���I�u�W�F�N�g�ɓ������Ă�����㏸���鏈�����J�n
            if(TimeStart)
            {
                if (!managerAccessor.Instance.dataMagager.playerlost || !managerAccessor.Instance.dataMagager.isShutDown)
                {
                    //�ݒ肳�ꂽ�v���C���[�㏸���ԕ������v���C���[���㏸����
                    if (uptime >= 0)
                    {
                        // Debug.Log("������");
                        uptime -= Time.deltaTime;//�v���C���[�㏸���Ԍ���

                        this.rb.AddForce(transform.up * jumpForce);
                    }
                    else
                    {
                        animator.SetBool("Wallhit", true);//�ǂ��痎����A�j���[�V�����J�n
                        MoveFinish();//�v���C���[�㏸���Ԃ�0�ɂȂ�ƃv���C���[�̈ړ����~�߂�
                    }
                }

                    
            }
           

          
            

        }
        else//�G�f�B�b�g���[�h�̎�
        {
             MoveFinish();//�ړ������I��
            animator.SetFloat("AniSpeed", 0.0f); // �ꎞ��~

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

            TimeStart = false;//�ړ��I����ɍēx�㏸���Ȃ��悤�ɂ���

            //ray_hit = false;//�ړ��I����ɍēx��΂Ȃ��悤��Ray�̃t���O��؂�

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
                other.gameObject.GetComponent<Goal>().GoalEffect_animator.SetBool("Goal", true);//�S�[���A�j���[�V�����Đ�
                other.gameObject.GetComponent<Goal>().change = true;//�S�~���̃C���X�g��ς���
                other.gameObject.GetComponent<Goal>().goalChara = false;
               

                script.playercount--;//�v���C���[�̐�-1
                Destroy(gameObject);//���g���폜
            }
        }
    }

}
