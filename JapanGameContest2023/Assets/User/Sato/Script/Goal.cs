using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;

    //GetComponent��p����Animator�R���|�[�l���g�����o��.
    public Animator animator;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���ɓ���������
        if (collision.gameObject.tag == "Player") 
        {
            //animator.Play("GoalEffect");
            //animator.SetBool("Goal", true);//�S�[���A�j���[�V�����Đ�
            //�S�[������Ƃ����ɂ͂�������Ȃ�
            if (goalChara)
            {
               
                //animator.Play("GoalEffect");
                //managerAccessor.Instance.dataMagager.goalPlayerNum++;
            }
        }
    }


}
