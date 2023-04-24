using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private bool goalChara = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ålŒö‚É“–‚½‚Á‚½
        if (collision.gameObject.tag == "Player") 
        {
            Destroy(collision.gameObject);

            //ƒS[ƒ‹‚·‚é‚Æ‚»‚±‚É‚Í‚à‚¤“ü‚ç‚È‚¢
            if(goalChara)
            {
                goalChara = false;
                transform.parent.GetComponent<GoalSystem>().goalCount++;
            }
        }
    }


}
