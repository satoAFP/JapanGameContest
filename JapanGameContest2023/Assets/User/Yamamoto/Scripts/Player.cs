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

    private float playerSize = 1f; // プレイヤーの幅

    [SerializeField, Header("ジャンプ力")] private float jumpForce = 350f;//プレイヤージャンプ力

    private bool JumpFlag = false;//現在ジャンプしているかのフラグ

    //現在プレイヤーが移動しているかを判別する
    bool isMoving = false;

    private Rigidbody2D rb;//プレイヤーリジッドボディ

    private Vector2 firstpos;//初期位置（仮）

    private Vector2 playerPosition;//現在のプレイヤーの位置

    //-----------Click関係の関数--------------------

    // クリックされた位置
    private Vector3 clickPosition;

    [SerializeField, Header("生成する移動指標オブジェクト")]
    private GameObject prefab;

    private GameObject CreateObj;//移動指標オブジェクトを入れる（削除命令に使う）

    private Vector2 mempos;//前フレーム時の座標
    private bool fream_move=false;//フレーム


    //-----------ray関係の変数の宣言---------------

    private Vector2 origin_x;//rayの原点(X方向）

    private Vector2 direction;//rayの方向ベクトル

    private Vector2 offset;//オフセット（Rayの開始位置)

   // private bool isGrounded; // 着地しているかどうか

    private bool ray_first = true;//何度もRayの処理が入ったとき一回だけ通す

    private LayerMask layermask;//レイヤーマスク

    //[SerializeField] private LayerMask groundlayermask;//地面判定用のレイヤーマスク
   
    [SerializeField, Header("テスト用Rayの長さ調整")]
    private float ray_length;

    [SerializeField, Header("着地判定用のRayの長さ")] private float g_ray_lenght;


    //-----------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//リジットボディの取得

        firstpos = this.transform.position;//プレイヤーの初期位置を取得

        playerPosition = firstpos;//最初はプレイヤーの初期位置を入れる

        fuptime = uptime;//プレイヤー上昇時間を保存

        // プレイヤーの中心からのオフセットを計算する
        offset = new Vector2(0.5f * playerSize, 0f);//はじめは右向き

        //取得するレイヤーを獲得（左右判定用）
        layermask = LayerMask.GetMask("CreateBlock","Block", "Ground");//ここに追加したいレイヤー名を入れるとlayermaskがレイヤー判定を取るようになる
        

    }

    private void Update()
    {
        //クリック処理はUpdateでしましょう
      
        //Rayの原点＝プレイヤーの現在の位置
        origin_x = (Vector2)transform.position + offset;//(X方向）
    
        direction = transform.right;//X方向を指す

        //プレイヤーの向いている向きにRayを飛ばす
        RaycastHit2D hit = Physics2D.Raycast(origin_x, direction, ray_length,layermask);

        // Rayの可視化
        Debug.DrawLine(origin_x, origin_x + direction * ray_length, Color.red);//左右判定用のRay
       
        //左右判定用のRayが当たった時の処理
        if (hit.collider != null)
        {
            Debug.DrawLine(origin_x, hit.point, Color.green);//デバッグ用のRayを可視化する処理

            // 当たったオブジェクトが自身でなければ、何かしらの処理をする
            if (hit.collider.gameObject != gameObject)
            {
                int layer = hit.collider.gameObject.layer;//Rayが当たったオブジェクトのレイヤーを入れる
                Debug.Log("当たったオブジェクトのレイヤーは" + LayerMask.LayerToName(layer) + "です。");

                //Rayが当たったのが移動指標オブジェクトの場合、ジャンプ処理をしない
                if (LayerMask.LayerToName(layer) == "CreateBlock")
                {
                    //Debug.Log("tobanai");
                }
                //ジャンプ処理を行う
                else 
                {
                    //特定のレイヤーにのみジャンプ処理を行う
                    if(LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
                    {
                        //移動中＆プレイヤー上昇時間が0ではないとき上昇する
                        if(isMoving)
                        {
                            if(uptime >= 0)
                            {
                                uptime -= Time.deltaTime;//プレイヤー上昇時間減少
                               
                                this.rb.AddForce(transform.up * jumpForce);
                            }
                            else
                            {
                                MoveFinish();//プレイヤー上昇時間が0になるとプレイヤーの移動を止める
                            }

                        }



                        //ジャンプフラグがfalseの時&現在プレイヤーが移動しているとき、ジャンプ処理実行
                        //if (!JumpFlag && isMoving)
                        //{
                           
                        //    //複数回ジャンプ処理を行わないように初めに当たったRayのみを反応させる
                        //    if (ray_first)
                        //    {
                        //        Debug.Log("J");
                        //        speed = 1.7f;
                        //        this.rb.AddForce(transform.up * jumpForce);
                        //        JumpFlag = true;
                        //        ray_first = false;
                        //    }

                        //}
                    }
                }
                //else
                //{
                //    Debug.Log("soreigai");
                //    //speed = fspeed;
                //    ray_first = true;
                //}
            }

        }
        else
        {
            Debug.Log("なにもあたってない");
            //speed = fspeed;
            ray_first = true;
        }
       
        // 移動中でなければクリックを受け付ける
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("移動");
            // クリックされた位置を取得
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // z座標を0に設定（2Dゲームなので）

            CreateObj = Instantiate(prefab, clickPosition, Quaternion.identity);//移動指標オブジェクト作成
            //ObjCount = CreateObj;//生成したオブジェクトを収納

            //クリックした場所の左右判定を取る
            if (playerPosition.x < clickPosition.x)//右
            {
                offset = new Vector2(0.5f * playerSize, 0f);//右向き
                transform.eulerAngles = new Vector3(0, 0, 0);
                //Debug.Log("右");
            }
            else//左
            {
                offset = new Vector2(-0.5f * playerSize, 0f);//左向き
                transform.eulerAngles = new Vector3(0, 180, 0);
               // Debug.Log("左");
            }

            // 移動を開始
            isMoving = true;
        }

        if(setblock)
        {
            Debug.Log("J");
            uptime = fuptime;
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {
            //FreezeRotationのみオンにする（Freezeは上書きできる）
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //落下処理（仮）　とりあえず今は落ちたら初期位置に戻る
            if (transform.position.y <= -10)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;//プレイヤー敗北フラグをON
                Destroy(this.gameObject);
            }

           
            // 移動中の場合は移動する
            if (isMoving)
            {
                //Debug.Log("a");
                // キャラクターのX座標をクリックされた位置に向けて移動
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // 移動が終わったらフラグを解除
                //前フレームの座標と今の座標を比べて、移動量が極端に少ない場合（壁にぶつかっている状態）処理を終了
                if (transform.position.x == clickPosition.x||Mathf.Abs(transform.position.x-mempos.x) < 0.03f)
                {
                    Debug.Log("b");
                    playerPosition = transform.position;//playerPositionを更新
                   // MoveFinish();//移動処理終了
                }

                if (transform.position.x == clickPosition.x)
                {
                    Debug.Log("cccc");
                    MoveFinish();//移動処理終了
                }
            }

            mempos = transform.position;//前フレームを保存

        }
        else//エディットモードの時
        {
             MoveFinish();//移動処理終了
            //Rigidbodyを制限する
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //移動終了時の処理
    private void MoveFinish()
    {
        if (isMoving)
        {
            Destroy(CreateObj);//移動指標オブジェクト削除

            isMoving = false;//移動処理終了
        }
    }

    //当たり判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {

            JumpFlag = false;
        }
    }

}
