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

    [SerializeField, Header("���[�h����I�u�W�F�N�g")] private GameObject loadImg;

    [SerializeField, Header("�}�E�X�̈ʒu���ꂽ�������Z�p")] private Vector3 cursorMove;


    private Vector3 loadRotate = new Vector2(0, 0);

    // Update is called once per frame
    void FixedUpdate()
    {
        //�}�E�X�̈ʒu�ɍ��킹��
        gameObject.GetComponent<RectTransform>().position = Input.mousePosition + cursorMove;

        //�J�[�\�������ꂼ��̉��ɏ���Ă���Ƃ��摜����ɕς���
        if (managerAccessor.Instance.dataMagager.onEdge && !managerAccessor.Instance.dataMagager.playMode)  
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

        //�V�[���ړ����n�܂�ƃ��[�h���̉摜�ɑ���
        if (managerAccessor.Instance.dataMagager.sceneMoveStart)
        {
            //�J�[�\����\��
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            //���[�h�摜�\��
            loadImg.SetActive(true);
            //��]����
            loadRotate.z -= managerAccessor.Instance.dataMagager.loadRotate;
            loadImg.GetComponent<RectTransform>().eulerAngles = loadRotate;
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            loadImg.SetActive(false);
            loadImg.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }

    }
}
