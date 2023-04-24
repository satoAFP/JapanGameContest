using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private bool goalChara = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            Destroy(collision.gameObject);

            //ゴールするとそこにはもう入らない
            if(goalChara)
            {
                goalChara = false;
                transform.parent.GetComponent<GoalSystem>().goalCount++;
            }
        }
    }


}
