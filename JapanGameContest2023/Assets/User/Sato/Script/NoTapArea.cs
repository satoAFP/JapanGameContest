using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTapArea : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //�K�v�ȏ��̎擾
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;

        //�I�u�W�F�N�g���ɃJ�[�\���������Ă��鎞�A�؂�ւ���
        if (pos.x - (size.x/2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y/2) < mouse.y && pos.y + (size.y / 2) > mouse.y) 
        {
            managerAccessor.Instance.dataMagager.noTapArea = true;
        }
        else
        {
            managerAccessor.Instance.dataMagager.noTapArea = false;
        }


    }
}
