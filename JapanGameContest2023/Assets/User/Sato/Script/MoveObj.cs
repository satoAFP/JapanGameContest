using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    [SerializeField, Header("�d�Ȃ��Ăق����Ȃ��I�u�W�F�N�g�̃^�O��")] private string[] tagNames;

    //�I������Ă���Ƃ��̃I�u�W�F�N�g�i���o�[�擾�p
    [System.NonSerialized] public int objNum;


    //����Ă͂����Ȃ��I�u�W�F�N�g�̏�ɂ���Ƃ��̏���-----------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++) 
        {
            if(collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = false;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //�ҏW���[�h�̎��̂�
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = false;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                }
            }
        }
    }
}
