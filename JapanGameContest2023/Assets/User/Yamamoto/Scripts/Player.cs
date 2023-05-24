using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //---------player関係（移動・ジャンプ）関係の変数宣言-----------------

    [SerializeField, Header("プレイヤー速度")] private float speed;//プレイヤー速度

    //こいつはそのうちDataManagerに放り込む
    [System.NonSerialized] public bool setblock;//足元の判定がブロックに当たっていた時

    [SerializeField, Header("プレイヤー上昇タイマー")] private float uptime;//プレイヤーが一度に上昇できる時間

    private float fuptime;//uptimeの開始時の数値を入れる

    [SerializeField, Header("ジャンプ力")] private float jumpForce = 350f;//プレイヤージャンプ力

    private Rigidbody2D rb;//プレイヤーリジッドボディ

    private bool moving = false;//プレイヤー各自の移動フラグ

    public bool Objhit = false;//壁やブロックを登ることを許可するフラグ

    public bool TimeStart = false;//uptime開始のフラグ

    //-----------Click関係の関数--------------------

    private FileGene script;//FileGeneスクリプト

   
    //-----------アニメーション関係の変数の宣言---------------

    //GetComponentを用いてAnimatorコンポーネントを取り出す.
    [SerializeField] private Animator animator;

    public bool StartAction = false;//アニメーション終了後にプレイヤーの処理開始

    //-----------------------------------------------


    [SerializeField] private Vector2 origin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//リジットボディの取得


        script = GameObject.Find("Clickjudge").GetComponent<FileGene>();//FileGeneスクリプト取得

        fuptime = uptime;//プレイヤー上昇時間を保存

    }

    private void Update()
    {
        //クリック処理はUpdateでしましょう
        bool stage1 = animator.GetBool("Stage1");//プレイヤーアニメーターからbool型のStage1を持ってくる

        //ステージ1の時のみ登場アニメーションを画面外からやってくるアニメーションにする
        if (managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage1" && stage1)
        {
            Debug.Log("ステージ1である");
            animator.Play("Stage1PlayerStart");
            animator.SetBool("Stage1", false);//一度だけアニメーション再生させるためfalseに
        }

       
        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {
            animator.SetFloat("AniSpeed", 0.6f); //アニメーションを再生させる

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stage1PlayerStart"))
            {
                StartAction = false;//登場アニメーション再生中のため移動処理をクリック反応させない
                Debug.Log("Animation finished");
            }
            else
            {
                Debug.Log("BB");
                StartAction = true;
            }


            if(Objhit)//プレイヤーの向いている方向にある判定に壁orブロックが当たっているとき
            {
                if(moving)//なおかつプレイヤーが動いているときに上昇を許可
                {
                    TimeStart = true;
                }
            }
            else
            {
                TimeStart = false;
            }

            if (!managerAccessor.Instance.dataMagager.noTapArea　&& !managerAccessor.Instance.dataMagager.objMaxFrag)
            {
                if(StartAction)
                {

                    // 移動中でなければクリックを受け付ける
                    if (script.posupdate && setblock)
                    {
                        //※移動中に再度入力しても左右判定を受け付けない不具合！！

                        //クリックした場所の左右判定を取る
                        if (origin.x <= managerAccessor.Instance.dataMagager.clickPosition.x)//右
                        {
                            //offset = new Vector2(0.5f * playerSize, 0f);//右向き
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("右");
                        }
                        else//左
                        {
                            // offset = new Vector2(-0.5f * playerSize, 0f);//左向き
                            transform.eulerAngles = new Vector3(0, 180, 0);
                            Debug.Log("左");
                        }

                      
                        // 移動を開始
                        managerAccessor.Instance.dataMagager.isMoving = true;//プレイヤー全体の移動処理
                        moving = true;//そのプレイヤー自身の移動フラグもON

                    }
                }

               
            }
               

            if(moving)
            {
                animator.SetBool("Moving", true);//移動時のアニメーションに切り替え
            }
            else
            {
                animator.SetBool("Moving", false);//停止時のアニメーションに切り替え
            }

            if (setblock)
            {
                animator.SetBool("Wallhit", false);//壁から落ちるアニメーション終了
                uptime = fuptime;
            }

        }
    
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {
            //他のプレイヤーがゲームオーバーになると自信の移動処理を止める
            if (managerAccessor.Instance.dataMagager.playerlost || managerAccessor.Instance.dataMagager.isShutDown)
            {
                Debug.Log("ｗｗｗｗ");
                MoveFinish();//移動処理終了
            }

            //FreezeRotationのみオンにする（Freezeは上書きできる）
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            rb.WakeUp();//動いていないとリジットボディが止まってしまうのでここで再起動

            

                //落下処理　とりあえず今は落ちたら初期位置に戻る
                if (transform.position.y <= -10)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;//プレイヤー敗北フラグをON
                managerAccessor.Instance.dataMagager.fallDeth = true;//落下死の判定取得
                Destroy(this.gameObject);
            }

            

            // 移動中の場合は移動する
            if (moving)
            {
                if(!managerAccessor.Instance.dataMagager.playerlost || !managerAccessor.Instance.dataMagager.isShutDown)
                {
                    // キャラクターのX座標をクリックされた位置に向けて移動
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(managerAccessor.Instance.dataMagager.clickPosition.x, transform.position.y), speed * Time.deltaTime);

                    origin = transform.position;

                    // 移動が終わったらフラグを解除
                    if (transform.position.x == managerAccessor.Instance.dataMagager.clickPosition.x)
                    {
                        //Debug.Log("cccc");
                        MoveFinish();//移動処理終了
                    }
                }
               
               

            }
          
            //プレイヤーがオブジェクトに当たっていたら上昇する処理を開始
            if(TimeStart)
            {
                if (!managerAccessor.Instance.dataMagager.playerlost || !managerAccessor.Instance.dataMagager.isShutDown)
                {
                    //設定されたプレイヤー上昇時間分だけプレイヤーが上昇する
                    if (uptime >= 0)
                    {
                        // Debug.Log("あたり");
                        uptime -= Time.deltaTime;//プレイヤー上昇時間減少

                        this.rb.AddForce(transform.up * jumpForce);
                    }
                    else
                    {
                        animator.SetBool("Wallhit", true);//壁から落ちるアニメーション開始
                        MoveFinish();//プレイヤー上昇時間が0になるとプレイヤーの移動を止める
                    }
                }

                    
            }
           

          
            

        }
        else//エディットモードの時
        {
             MoveFinish();//移動処理終了
            animator.SetFloat("AniSpeed", 0.0f); // 一時停止

            //Rigidbodyを制限する
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //移動終了時の処理
    private void MoveFinish()
    {
        if (moving)
        {
            script.playercount--;//プレイヤーの数-1

            TimeStart = false;//移動終了後に再度上昇しないようにする

            //ray_hit = false;//移動終了後に再度飛ばないようにRayのフラグを切る

            //playerPosition = transform.position;//プレイヤーが動いた場所を取得する

            moving = false;//目的地にたどり着いたプレイヤーの移動処理終了
        }
    }

    //当たり判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        //ゴールした時
        if (other.gameObject.CompareTag("Goal"))
        {
            //まだ誰もゴールしていないとき
            if (other.gameObject.GetComponent<Goal>().goalChara)
            {
                //ゴールしているキャラのカウントプラス
                managerAccessor.Instance.dataMagager.goalPlayerNum++;
                other.gameObject.GetComponent<Goal>().GoalEffect_animator.SetBool("Goal", true);//ゴールアニメーション再生
                other.gameObject.GetComponent<Goal>().change = true;//ゴミ箱のイラストを変える
                other.gameObject.GetComponent<Goal>().goalChara = false;
               

                script.playercount--;//プレイヤーの数-1
                Destroy(gameObject);//自身も削除
            }
        }
    }

}
