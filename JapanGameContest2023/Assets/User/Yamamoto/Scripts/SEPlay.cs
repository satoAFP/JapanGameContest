using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アニメーターに直接SEをつけるスクリプトです(プレイヤー用）

public class SEPlay : MonoBehaviour
{

    [Header("ここに効果音を入れる")]
    [SerializeField] private AudioClip walkse;//歩行SE
    [SerializeField] private AudioClip fallse;//落下SE
   
    private AudioSource audioSource;

    public bool startse = false;//SEを鳴らすフラグ

    public bool onese = true;//呼び出されたとき一度だけ音を鳴らす

    public bool playfallse = false;//落下SEを鳴らすフラグ(trueはアニメーターでする)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();//スクリプト取得
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //現在プレイヤーが死んでいなければSEを鳴らす
        if(!managerAccessor.Instance.dataMagager.fallDeth||
           !managerAccessor.Instance.dataMagager.infectionDeth ||
           !managerAccessor.Instance.dataMagager.overDeth)
        {
            if (startse)//アニメーターの方でstartseをtrueにする
            {
                if (onese)
                {
                    if (!managerAccessor.Instance.dataMagager.playerlost && !managerAccessor.Instance.dataMagager.isShutDown)
                    {
                        audioSource.PlayOneShot(walkse);
                        onese = false;
                        Debug.Log("sss");
                    }
                }
            }
            else
            {
                onese = true;
            }


            if(playfallse)
            {
                StartFallSE();
                playfallse = false;//ここでフラグ初期化
            }

        }
        
    }



    public void StartFallSE()
    {
        Debug.Log("staru");
        audioSource.PlayOneShot(fallse);
    }
}
