using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            //ゴールするとそこにはもう入らない
            if(goalChara)
            {
                //managerAccessor.Instance.dataMagager.goalPlayerNum++;
            }
        }
    }


}
