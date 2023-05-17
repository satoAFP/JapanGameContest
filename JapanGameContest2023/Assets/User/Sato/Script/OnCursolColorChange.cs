using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCursolColorChange : MonoBehaviour
{
    [SerializeField, Header("�A���t�@�l")] private float alpha;

    // Update is called once per frame
    void Update()
    {
        //�K�v�ȏ��̎擾
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;
        Color color = gameObject.GetComponent<Image>().color;
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //�I�u�W�F�N�g���ɃJ�[�\���������Ă��鎞�A�؂�ւ���
        if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
        {
            color.a = alpha;
            gameObject.GetComponent<Image>().color = color;

            //���[�h�`�F���W�̂݃J�[�\�����֎~�ɂȂ�ꍇ����
            if (dataManager.isMoving || dataManager.onBlock) 
            {
                if (gameObject.transform.parent.gameObject.name == "ChangeButton")
                {
                    managerAccessor.Instance.dataMagager.isNoClick = true;
                }
            }
            else
            {
                dataManager.isNoClick = false;
            }
        }
        else
        {
            color.a = 0;
            gameObject.GetComponent<Image>().color = color;

            if (gameObject.transform.parent.gameObject.name == "ChangeButton")
            {
                managerAccessor.Instance.dataMagager.isNoClick = false;
            }
        }
    }
}
