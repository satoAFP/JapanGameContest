using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIcon : MonoBehaviour
{
    //�T�C�Y�ύX�������Ă��鉏�̈ʒu�̖��O
    public enum ChangeSizePosName
    {
        DOWN,
        RIGHT,
        UP,
        LEFT,
        RIGHT_DOWN,
        RIGHT_UP,
        LEFT_UP,
        LEFT_DOWN,
        NONE,
    }


    [SerializeField, Header("�J�[�\���̉摜")] private Sprite cursor;

    [SerializeField, Header("���̉摜")] private Sprite arrow;

    [SerializeField, Header("�}�E�X�̈ʒu���ꂽ�������Z�p")] private Vector3 cursorMove;

    // Update is called once per frame
    void FixedUpdate()
    {
        //�}�E�X�̈ʒu�ɍ��킹��
        gameObject.GetComponent<RectTransform>().position = Input.mousePosition + cursorMove;

        //�J�[�\�������ꂼ��̉��ɏ���Ă���Ƃ��摜����ɕς���
        if (managerAccessor.Instance.dataMagager.onEdge) 
        {
            gameObject.GetComponent<Image>().sprite = arrow;

            if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.DOWN ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.UP) 
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT_DOWN ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT_UP)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 45);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT_UP ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT_DOWN)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -45);
            }
        }
        //�ʏ펞
        else
        {
            gameObject.GetComponent<Image>().sprite = cursor;
            gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
        }

    }
}
