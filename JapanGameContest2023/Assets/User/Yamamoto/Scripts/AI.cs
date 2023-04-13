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

    void Update()
    {
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
