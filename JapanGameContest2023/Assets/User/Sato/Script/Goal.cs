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

    //ゴミ箱のイラスト変更フラグ
    public bool change = false;

    private void Update()
    {
        if(change)
        {
            StartCoroutine("ChangeDastBox");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            //ゴールするとそこにはもう入らない
            if (goalChara)
            {
               
            }
        }
    }

    IEnumerator ChangeDastBox()
    {
        yield return new WaitForSeconds(0.15f);

        DastBox_animator.SetBool("dastboxchange", true);//ゴミ箱のイラスト変化
    }

}
