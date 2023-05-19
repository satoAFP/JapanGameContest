using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;

    //GetComponentを用いてAnimatorコンポーネントを取り出す.
    public Animator animator;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            //animator.Play("GoalEffect");
            //animator.SetBool("Goal", true);//ゴールアニメーション再生
            //ゴールするとそこにはもう入らない
            if (goalChara)
            {
               
                //animator.Play("GoalEffect");
                //managerAccessor.Instance.dataMagager.goalPlayerNum++;
            }
        }
    }


}
