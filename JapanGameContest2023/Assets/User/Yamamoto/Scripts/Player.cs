using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//�v���C���[���x

    private float jumpForce = 350f;//�v���C���[�W�����v��

    private int jumpCount = 0;//�������͂����Ȃ�

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    public Vector2 firstpos;//�����ʒu�i���j

    //�ړ�����p�̕ϐ�(�}�E�X�p�j
    bool isMoving = false;

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(managerAccessor.Instance.dataMagager.playMode)
        {
            Vector2 position = transform.position;

            if(position.y<=-10)//���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            {
                Debug.Log("��蒼��");

                position = firstpos;//�����ʒu�ɖ߂�
            }

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
                // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // �ړ����I�������t���O������
                if (transform.position.x == clickPosition.x)
                {
                    isMoving = false;
                }
            }

          
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            jumpCount = 0;
        }
    }

}
