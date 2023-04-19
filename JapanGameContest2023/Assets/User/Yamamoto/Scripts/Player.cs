using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //---------player�֌W�i�ړ��E�W�����v�j�֌W�̕ϐ��錾-----------------

    private float speed;//�v���C���[���x

    private float playerSize = 1f; // �v���C���[�̕�

    [SerializeField, Header("�W�����v��")] private float jumpForce = 350f;//�v���C���[�W�����v��

    private int jumpCount = 0;//�W�����v�𕡐����͂����Ȃ�

    //�ړ�����p�̕ϐ�(�}�E�X�p�j
    bool isMoving = false;

    //�u���b�N�ɂԂ��������̃v���C���[�̈ړ�
    bool hitMoving = false;

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    public Vector2 firstpos;//�����ʒu�i���j

    private Vector2 playerPosition;//���݂̃v���C���[�̈ʒu


    //-----------Click�֌W�̊֐�--------------------

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    [SerializeField, Header("��������ړ��w�W�I�u�W�F�N�g")]
    private GameObject prefab;

    [SerializeField]
    private GameObject CreateObj;//�ړ��w�W�I�u�W�F�N�g������i�폜���߂Ɏg���j

    //-----------ray�֌W�̕ϐ��̐錾---------------

    [SerializeField] private Vector2 origin_x;//ray�̌��_(X�����j

    [SerializeField] private Vector2 origin_y;//ray�̌��_(Y�����j

    private Vector2 direction;//ray�̕����x�N�g��

    private Vector2 offset;//�I�t�Z�b�g�iRay�̊J�n�ʒu)

    private bool isGrounded; // ���n���Ă��邩�ǂ���

    [SerializeField] private LayerMask layermask;//���C���[�}�X�N

    [SerializeField] private LayerMask groundlayermask;//�n�ʔ���p�̃��C���[�}�X�N
   
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

        // �v���C���[�̒��S����̃I�t�Z�b�g���v�Z����
        offset = new Vector2(0.5f * playerSize, 0f);//�͂��߂͉E����

        //�擾���郌�C���[���l���i���E����p�j
        layermask = LayerMask.GetMask("CreateBlock","Block");//�����ɒǉ����������C���[���������layermask�����C���[��������悤�ɂȂ�
        //�擾���郌�C���[���l���i��������p�j
        groundlayermask = LayerMask.GetMask("Ground","Block");//�����ɒǉ����������C���[���������groundlayermask�����C���[��������悤�ɂȂ�

    }

    private void Update()
    {
        //�N���b�N������Update�ł��܂��傤

        //Ray�̌��_���v���C���[�̌��݂̈ʒu
        origin_x = (Vector2)transform.position + offset;//(X�����j
        origin_y = (Vector2)transform.position;//(Y�����j

        direction = transform.right;//X�������w��

        //�v���C���[�̌����Ă��������Ray���΂�
        RaycastHit2D hit = Physics2D.Raycast(origin_x, direction, ray_length,layermask);

        // �v���C���[�̑�����Ray���΂�
        RaycastHit2D g_hit = Physics2D.Raycast(origin_y, Vector2.down, g_ray_lenght, groundlayermask);

        // Ray�̉���
        Debug.DrawLine(origin_x, origin_x + direction * ray_length, Color.red);//���E����p��Ray
        Debug.DrawLine(transform.position, transform.position + Vector3.down * g_ray_lenght, Color.blue);//���n����p��Ray


        //���E����p��Ray�������������̏���
        if (hit.collider != null)
        {
            Debug.DrawLine(origin_x, hit.point, Color.green);//�f�o�b�O�p��Ray���������鏈��

            // ���������I�u�W�F�N�g�����g�łȂ���΁A��������̏���������
            if (hit.collider.gameObject != gameObject)
            {
                // Debug.Log("Hit object: " + hit.collider.gameObject.name);

                int layer = hit.collider.gameObject.layer;//Ray�����������I�u�W�F�N�g�̃��C���[������
                Debug.Log("���������I�u�W�F�N�g�̃��C���[��" + LayerMask.LayerToName(layer) + "�ł��B");

                //Ray�����������̂��ړ��w�W�I�u�W�F�N�g�̏ꍇ�A�W�����v���������Ȃ�
                if (LayerMask.LayerToName(layer) == "CreateBlock")
                {
                    Debug.Log("tobanai");
                }
                //�W�����v�������s��
                else if (LayerMask.LayerToName(layer) == "Block")
                {
                    if (jumpCount < 1)
                    {
                        this.rb.AddForce(transform.up * jumpForce);
                        jumpCount++;
                    }
                }
            }

        }

        // Ray���n�ʂɓ��������ꍇ�AisGrounded��true�ɂ���
        if (g_hit.collider != null)
        {
            Debug.Log("���߂񂠂�");
            Debug.DrawLine(origin_y, g_hit.point, Color.yellow);//�f�o�b�O�p��Ray���������鏈��
            isGrounded = true;
        }
        else
        {
            Debug.Log("���߂�Ȃ�");
            isGrounded = false;
        }



        // �ړ����łȂ���΃N���b�N���󂯕t����
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Debug.Log("�ړ�");
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
                Debug.Log("�E");
            }
            else//��
            {
                offset = new Vector2(-0.5f * playerSize, 0f);//������
                transform.eulerAngles = new Vector3(0, 180, 0);
                Debug.Log("��");
            }

            // �ړ����J�n
            isMoving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {

            //�f�o�b�O�p�̃L�[�ړ�����(�I�����������j---------------------------------

            //if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
            //{
            //    isMoving = false;//�ړ������������I��
            //    this.rb.AddForce(transform.up * jumpForce);
            //    jumpCount++;
            //}

            //--------------------------------------------------

            //FreezeRotation�̂݃I���ɂ���iFreeze�͏㏑���ł���j
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            speed = 5.0f;

            //���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            if (transform.position.y <= -10)
            {
                Debug.Log("��蒼��");
                MoveFinish();//�ړ������������I��
                transform.position = firstpos;
            }

           
            // �ړ����̏ꍇ�͈ړ�����
            if (isMoving)
            {
                //Debug.Log("a");
                // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // �ړ����I�������t���O������
                if (transform.position.x == clickPosition.x)
                {
                    //Debug.Log("b");
                    playerPosition = transform.position;//playerPosition���X�V
                    MoveFinish();//�ړ������I��
                }
            }
            else if (hitMoving)
            {
                Debug.Log("akys");
                MoveFinish();//�ړ������I��
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x+0.01f, transform.position.y), speed * Time.deltaTime);
                playerPosition = transform.position;//playerPosition���X�V
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
        if (isMoving)
        {
            Destroy(CreateObj);//�ړ��w�W�I�u�W�F�N�g�폜

            isMoving = false;//�ړ������I��
        }
    }

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("�Ԃ����Ă�hc");
            hitMoving = false;//
            jumpCount = 0;
        }

        //�u���b�N�ɂԂ������Ƃ�
        if (other.gameObject.CompareTag("MoveBlock"))
        {
            Debug.Log("�Ԃ����Ă�");
            // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y), speed * Time.deltaTime);
            MoveFinish();//�ړ������������I��
            hitMoving = true;//�u���b�N�ɂԂ������Ƃ��̋������s��
        }
    }

}
