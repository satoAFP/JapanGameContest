using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyFile : MonoBehaviour
{
    [SerializeField, Header("感染したときの画像")] private Sprite infectionFile;

    [SerializeField, Header("アニメーションの待機時間")] private int stopFrame;


    private int FrameCount = 0;
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
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
            Destroy(collision.gameObject.GetComponent<Player>().CreateObj);
            Destroy(collision.gameObject);
            //感染したファイルになる
            //gameObject.GetComponent<SpriteRenderer>().sprite = infectionFile;

            anim.SetBool("HitEnemy", true);

            managerAccessor.Instance.dataMagager.onDecoyFile = true;
            managerAccessor.Instance.dataMagager.infectionDeth = true;
        }
    }

}
