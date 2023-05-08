using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ålŒö‚É“–‚½‚Á‚½
        if (collision.gameObject.tag == "Player") 
        {
            //ƒS[ƒ‹‚·‚é‚Æ‚»‚±‚É‚Í‚à‚¤“ü‚ç‚È‚¢
            if(goalChara)
            {
                managerAccessor.Instance.dataMagager.goalPlayerNum++;
            }
        }
    }


}
