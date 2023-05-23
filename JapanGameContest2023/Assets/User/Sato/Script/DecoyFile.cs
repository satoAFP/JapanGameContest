using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyFile : MonoBehaviour
{
    [SerializeField, Header("感染したときの画像")] private Sprite infectionFile;

    [SerializeField, Header("アニメーションの待機時間")] private int stopFrame;

    [SerializeField] private AudioClip infectionse;//感染効果音

    private AudioSource audioSource;


    private int FrameCount = 0;
    private Animator anim;

    [SerializeField, Header("感染爆発エフェクトアニメーション")]
    private Animator effect_ani;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//スクリプト取得
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ファイルに当たると
        if(managerAccessor.Instance.dataMagager.onDecoyFile)
        {
            FrameCount++;
            //設定しているフレームになるとゲームオーバー
            if (stopFrame <= FrameCount) 
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //キャラ削除
           // Destroy(collision.gameObject.GetComponent<Player>().CreateObj);
            Destroy(collision.gameObject);
            //感染したファイルになる
            //gameObject.GetComponent<SpriteRenderer>().sprite = infectionFile;


            //ここでアニメーションを流すフラグをON
            effect_ani.SetBool("PlayerHit", true);
            audioSource.PlayOneShot(infectionse);//感染SE鳴らす

            StartCoroutine("ChangeDecoyFile");//コルーチン開始

            managerAccessor.Instance.dataMagager.onDecoyFile = true;
            managerAccessor.Instance.dataMagager.infectionDeth = true;
        }
    }

    IEnumerator ChangeDecoyFile()
    {
        yield return new WaitForSeconds(0.3f);//時間差でアニメーションを行う

        anim.SetBool("HitEnemy", true);
    }

}
