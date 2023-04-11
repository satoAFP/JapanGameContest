using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//�v���C���[���x

    private float jumpForce = 350f;//�v���C���[�W�����v��

    private int jumpCount = 0;//�������͂����Ȃ�

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    [SerializeField] private bool movechange;//�ړ�������ύX�itrue:�}�E�X false:�L�[�j

    //�ړ�����p�̕ϐ�(�}�E�X�p�j
    bool isMoving;

    public Vector2 firstpos;//�����ʒu�i���j

    Vector3 mousePos, worldPos;//�i�}�E�X�̈ʒu�ƃN���b�N�����ʒu�j

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstpos = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(managerAccessor.Instance.dataMagager.playMode)
        {
            Vector2 position = transform.position;

            if(position.y<=-10)//���������i���j�@�Ƃ肠�������͗������珉���ʒu�ɖ߂�
            {
                Debug.Log("��蒼��");

                position = firstpos;//�����ʒu�ɖ߂�
            }

            if (!movechange)
            {
                speed = 0.05f;

                if (Input.GetKey(KeyCode.A))
                {
                    position.x -= speed;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    position.x += speed;
                }

                if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
                {
                    this.rb.AddForce(transform.up * jumpForce);
                    jumpCount++;
                }
            }
            else
            {
                speed = 5.0f;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("����");
                    //�}�E�X�̍��W�擾
                    mousePos = Input.mousePosition;
                    //�X�N���[�����W�����[���h���W�ɕϊ�
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);


                    position = new Vector2(worldPos.x, firstpos.y);
                }

                //�ړ����Ȃ珈�����󂯕t���Ȃ�
                //if (isMoving)
                //{
                //    return;
                //}

                //�ړ����Ă��Ȃ��ꍇ�̏���
                //���N���b�N���ꂽ��
                //if (Input.GetMouseButtonDown(0))
                //{
                //    Debug.Log("����");

                //    //�}�E�X�̍��W�擾
                //    mousePos = Input.mousePosition;
                //    //�X�N���[�����W�����[���h���W�ɕϊ�
                //    worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
                //    //�R���[�`���X�^�[�g
                //    StartCoroutine(_move());
                //}
            }


            transform.position = position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            jumpCount = 0;
        }
    }

    //�ړ��p�R���[�`��
    //IEnumerator _move()
    //{
    //    //�ړ��t���O��true
    //    isMoving = true;

    //    //���[���h���W�Ǝ��g�̍��W���r�����[�v
    //    while ((worldPos - transform.position).sqrMagnitude > Mathf.Epsilon)
    //    {
    //        //�w�肵�����W�Ɍ������Ĉړ�
    //        transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);
    //        //1�t���[���҂�
    //        yield return null;
    //    }
    //    //�ړ��t���O��false
    //    isMoving = false;
    //}
}
