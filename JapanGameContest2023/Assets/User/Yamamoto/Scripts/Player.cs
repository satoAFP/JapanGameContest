using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//�v���C���[���x

    [SerializeField,Header("�W�����v��")]private float jumpForce = 350f;//�v���C���[�W�����v��

    private int jumpCount = 0;//�W�����v�𕡐����͂����Ȃ�

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    private Collider2D coll;//�v���C���[�̃R���C�_�[

    public Vector2 firstpos;//�����ʒu�i���j

    private Vector2 playerPosition;//���݂̃v���C���[�̈ʒu

    //�ړ�����p�̕ϐ�(�}�E�X�p�j
    bool isMoving = false;

    //�u���b�N�ɂԂ��������̃v���C���[�̈ړ�
    bool hitMoving = false;

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    [SerializeField] private Vector2 origin;//ray�̌��_

    private Vector2 direction;//ray�̕����x�N�g��

    private bool isGrounded; // ���n���Ă��邩�ǂ���


    [SerializeField] private LayerMask layermask;//���C���[�}�X�N

    [SerializeField] private LayerMask groundlayermask;//�n�ʔ���p�̃��C���[�}�X�N
   
    [SerializeField, Header("�e�X�g�pRay�̒�������")]
    private float ray_length;

    [SerializeField, Header("���n����p��Ray�̒���")] private float g_ray_lenght;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//���W�b�g�{�f�B�̎擾

        coll = GetComponent<Collider2D>();

        firstpos = this.transform.position;//�v���C���[�̏����ʒu���擾

        playerPosition = firstpos;//�ŏ��̓v���C���[�̏����ʒu������

        //�擾���郌�C���[���l���i���E����p�j
        layermask = LayerMask.GetMask("Block");//�����ɒǉ����������C���[���������layermask�����C���[��������悤�ɂȂ�
        //�擾���郌�C���[���l���i��������p�j
        groundlayermask = LayerMask.GetMask("Ground","Block");//�����ɒǉ����������C���[���������groundlayermask�����C���[��������悤�ɂȂ�

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // �v���C���[�̒��S����̃I�t�Z�b�g���v�Z����
        Vector2 offset = new Vector2(0.5f * coll.bounds.size.x, 0f);

        origin = (Vector2)transform.position + offset;

        direction = transform.right;//X�������w��

        //�v���C���[�̌����Ă��������Ray���΂�
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, ray_length, layermask);

        // �v���C���[�̑�����Ray���΂�
        RaycastHit2D g_hit = Physics2D.Raycast(origin, Vector2.down, g_ray_lenght, groundlayermask);

        // Ray�̉���
        Debug.DrawLine(origin, origin + direction * ray_length, Color.red);//���E����p��Ray
        Debug.DrawLine(transform.position, transform.position + Vector3.down * g_ray_lenght, Color.blue);//���n����p��Ray


        //���E����p��Ray�������������̏���
        if (hit.collider != null)
        {
            Debug.DrawLine(origin, hit.point, Color.green);//�f�o�b�O�p��Ray���������鏈��

            // ���������I�u�W�F�N�g�����g�łȂ���΁A��������̏���������
            if (hit.collider.gameObject != gameObject)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);

                //if(jumpCount < 1)
                //{
                //    this.rb.AddForce(transform.up * jumpForce);
                //    jumpCount++;
                //}

            }

        }

        // Ray���n�ʂɓ��������ꍇ�AisGrounded��true�ɂ���
        if (g_hit.collider != null)
        {
            Debug.Log("���߂񂠂�");
            Debug.DrawLine(origin, g_hit.point, Color.green);//�f�o�b�O�p��Ray���������鏈��
            isGrounded = true;
        }
        else
        {
            Debug.Log("���߂�Ȃ�");
            isGrounded = false;
        }

        

        if (managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
        {
           // Debug.Log(firstpos);

            //FreezeRotation�̂݃I���ɂ���iFreeze�͏㏑���ł���j
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //if(position.y<=-10)//���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            //{
            //    Debug.Log("��蒼��");

            //    position = firstpos;//�����ʒu�ɖ߂�
            //}

            //�ړ������i�L�[�j
            //speed = 0.05f;

            //if (Input.GetKey(KeyCode.A))
            //{
            //    position.x -= speed;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    position.x += speed;
            //}

            if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
            {
                isMoving = false;//�ړ������������I��
                this.rb.AddForce(transform.up * jumpForce);
                jumpCount++;
            }

            //transform.position = position;


            speed = 5.0f;

            //���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            if (transform.position.y <= -10)
            {
                Debug.Log("��蒼��");
                isMoving = false;//�ړ������������I��
                transform.position = firstpos;
            }

            // �ړ����łȂ���΃N���b�N���󂯕t����
            if (!isMoving && Input.GetMouseButtonDown(0))
            {
                Debug.Log("�ړ�");
                // �N���b�N���ꂽ�ʒu���擾
                clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0; // z���W��0�ɐݒ�i2D�Q�[���Ȃ̂Łj

                //�N���b�N�����ꏊ�̍��E��������
                if (playerPosition.x < clickPosition.x)//�E
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    Debug.Log("�E");
                }
                else//��
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    Debug.Log("��");
                }

                // �ړ����J�n
                isMoving = true;
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
                    isMoving = false;//�ړ������I��
                }
            }
            else if (hitMoving)
            {
                Debug.Log("akys");
                isMoving = false;//�ړ������I��
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x+0.01f, transform.position.y), speed * Time.deltaTime);
                playerPosition = transform.position;//playerPosition���X�V
            }

          
        }
        else//�G�f�B�b�g���[�h�̎�
        {
            isMoving = false;//�ړ������I��
            //Rigidbody�𐧌�����
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //�v���C���[�̃W�����v����
    private void PlayerJump()
    {
        this.rb.AddForce(transform.up * jumpForce);
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
            isMoving = false;//�ړ������������I��
            hitMoving = true;//�u���b�N�ɂԂ������Ƃ��̋������s��
        }
    }

}
