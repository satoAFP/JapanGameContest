using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;

    //ゴール時のエフェクトアニメーター取得
    public Animator GoalEffect_animator;

    //ゴミ箱にプレイヤーが入っているアニメーション
    public Animator DastBox_animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            //DastBox_animator.SetBool("dastboxchange", true);//ゴミ箱のイラスト変化

            //ゴールするとそこにはもう入らない
            if (goalChara)
            {
               
                //animator.Play("GoalEffect");
                //managerAccessor.Instance.dataMagager.goalPlayerNum++;
            }
        }
    }


}
