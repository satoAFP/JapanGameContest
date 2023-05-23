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

    [SerializeField] private AudioClip DastSE;//ゴミ箱に入った時のSE

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();//スクリプト取得
    }

    private void Update()
    {
        if(change)
        {
            StartCoroutine("ChangeDastBox");//ゴミ箱画像変更コルーチン
            change = false;
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
        audioSource.PlayOneShot(DastSE);//ゴミ箱SE鳴らす

        yield return new WaitForSeconds(0.15f);

        DastBox_animator.SetBool("dastboxchange", true);//ゴミ箱のイラスト変化
    }

}
