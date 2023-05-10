using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    //�I������Ă���Ƃ��̃I�u�W�F�N�g�i���o�[�擾�p
    [System.NonSerialized] public int objNum;


    //����Ă͂����Ȃ��I�u�W�F�N�g�̏�ɂ���Ƃ��̏���-----------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            //�ҏW���[�h�̎��̂�
            if (!managerAccessor.Instance.dataMagager.playMode)
            {
                managerAccessor.Instance.dataMagager.onBlock = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            //�ҏW���[�h�̎��̂�
            if (!managerAccessor.Instance.dataMagager.playMode)
            {
                managerAccessor.Instance.dataMagager.onBlock = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = false;
        }
    }
}
