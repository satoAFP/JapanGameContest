using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // �ړ����x
    public float moveSpeed = 5f;

    // �N���b�N���ꂽ�ʒu
    private Vector3 clickPosition;

    // �ړ������ǂ����̃t���O
    private bool isMoving = false;

    [SerializeField] private Vector2 origin;//ray�̌��_

    private Vector2 direction;//ray�̕����x�N�g��

    [SerializeField] private LayerMask[] layermask;//���C���[�}�X�N

    int layerMask = ~(1 << 8);

    [SerializeField,Header("�e�X�g�pRay�̒�������")]
    private float ray_length = 5.0f;

    void Update()
    {
        origin = transform.position;

        direction = transform.right;//X�������w��

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, ray_length, layermask[0]);

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
                isMoving = false;
            }
        }

    }

  
}
