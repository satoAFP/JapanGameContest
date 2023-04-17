using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // �ړ����x
    public float moveSpeed = 5f;

    private Vector2 playerPosition;//���݂̃v���C���[�̈ʒu

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    // �ړ������ǂ����̃t���O
    private bool isMoving = false;

    [SerializeField, Header("�W�����v��")] private float jumpForce = 350f;//�v���C���[�W�����v��

    private Rigidbody2D rb;//�v���C���[���W�b�h�{�f�B

    [SerializeField] private Vector2 origin;//ray�̌��_

    private Vector2 direction;//ray�̕����x�N�g��

    [SerializeField] private LayerMask layermask;//���C���[�}�X�N

    [SerializeField,Header("�e�X�g�pRay�̒�������")]
    private float ray_length = 5.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//���W�b�g�{�f�B�擾

        playerPosition = transform.position;//�ŏ��̓v���C���[�̏����ʒu������
    }

    void Update()
    {
        origin = transform.position;

        direction = transform.right;//X�������w��

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, ray_length, layermask);

        // Ray�̉���
        Debug.DrawLine(origin, origin + direction * ray_length, Color.red);

        if (hit.collider != null)
        {
            Debug.DrawLine(origin, hit.point, Color.green);

            // ���������I�u�W�F�N�g�����g�łȂ���΁A��������̏���������
            if (hit.collider.gameObject != gameObject)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }

        }


        // Debug.DrawRay(ray.origin, ray.direction * ray_length, Color.yellow);

        // �ړ����łȂ���΃N���b�N���󂯕t����
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Debug.Log("�ړ�");
            // �N���b�N���ꂽ�ʒu���擾
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // z���W��0�ɐݒ�i2D�Q�[���Ȃ̂Łj

            //�N���b�N�����ꏊ�̍��E��������
            if(playerPosition .x  < clickPosition.x)//�E
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
            // �L�����N�^�[��X���W���N���b�N���ꂽ�ʒu�Ɍ����Ĉړ�
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), moveSpeed * Time.deltaTime);

            // �ړ����I�������t���O������
            if (transform.position.x == clickPosition.x)
            {
                playerPosition = transform.position;//playerPosition���X�V
                Debug.Log("p_pos" + playerPosition);
                isMoving = false;
            }
        }

    }

  
}
