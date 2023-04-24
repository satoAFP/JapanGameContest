using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private bool goalChara = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���ɓ���������
        if (collision.gameObject.tag == "Player") 
        {
            Destroy(collision.gameObject);

            //�S�[������Ƃ����ɂ͂�������Ȃ�
            if(goalChara)
            {
                goalChara = false;
                transform.parent.GetComponent<GoalSystem>().goalCount++;
            }
        }
    }


}
