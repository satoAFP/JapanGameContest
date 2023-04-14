using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//�v���C���[���x

    [SerializeField,Header("�W�����v��")]private float jumpForce = 350f;//�v���C���[�W�����v��

    private int jumpCount = 0;//�W�����v�𕡐����͂����Ȃ�

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    public Vector2 firstpos;//�����ʒu�i���j

    //�ړ�����p�̕ϐ�(�}�E�X�p�j
    bool isMoving = false;

    //�u���b�N�ɂԂ��������̃v���C���[�̈ړ�
    bool hitMoving = false;

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstpos = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(managerAccessor.Instance.dataMagager.playMode)//���샂�[�h�̎�
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

            //if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
            //{
            //    this.rb.AddForce(transform.up * jumpForce);
            //    jumpCount++;
            //}

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

                // �ړ����J�n
                isMoving = true;
            }

            // �ړ����̏ꍇ�͈ړ�����
            if (isMoving)
            {
                Debug.Log("a");
                // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // �ړ����I�������t���O������
                if (transform.position.x == clickPosition.x)
                {
                    Debug.Log("b");
                    isMoving = false;//�ړ������I��
                }
            }
            else if (hitMoving)
            {
                Debug.Log("akys");
                isMoving = false;//�ړ������I��
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x+0.01f, transform.position.y), speed * Time.deltaTime);
            }

          
        }
        else//�G�f�B�b�g���[�h�̎�
        {
            isMoving = false;//�ړ������I��
            //Rigidbody�𐧌�����
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
            this.rb.AddForce(transform.up * jumpForce);
            // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y), speed * Time.deltaTime);
            isMoving = false;//�ړ������������I��
            hitMoving = true;//�u���b�N�ɂԂ������Ƃ��̋������s��
        }
    }

}
