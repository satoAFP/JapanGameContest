using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //---------player関係（移動・ジャンプ）関係の変数宣言-----------------

    private float speed;//プレイヤー速度

    private float playerSize = 1f; // プレイヤーの幅

    [SerializeField, Header("ジャンプ力")] private float jumpForce = 350f;//プレイヤージャンプ力

    private int jumpCount = 0;//ジャンプを複数入力させない

    //移動判定用の変数(マウス用）
    bool isMoving = false;

    //ブロックにぶつかった時のプレイヤーの移動
    bool hitMoving = false;

    private Rigidbody2D rb;//プレイヤーリジッドボディ

    public Vector2 firstpos;//初期位置（仮）

    private Vector2 playerPosition;//現在のプレイヤーの位置


    //-----------Click関係の関数--------------------

    // クリックされた位置
    private Vector3 clickPosition;

    [SerializeField, Header("生成する移動指標オブジェクト")]
    private GameObject prefab;

    [SerializeField]
    private GameObject CreateObj;//移動指標オブジェクトを入れる（削除命令に使う）

    //-----------ray関係の変数の宣言---------------

    [SerializeField] private Vector2 origin_x;//rayの原点(X方向）

    [SerializeField] private Vector2 origin_y;//rayの原点(Y方向）

    private Vector2 direction;//rayの方向ベクトル

    private Vector2 offset;//オフセット（Rayの開始位置)

    private bool isGrounded; // 着地しているかどうか

    [SerializeField] private LayerMask layermask;//レイヤーマスク

    [SerializeField] private LayerMask groundlayermask;//地面判定用のレイヤーマスク
   
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

        // プレイヤーの中心からのオフセットを計算する
        offset = new Vector2(0.5f * playerSize, 0f);//はじめは右向き

        //取得するレイヤーを獲得（左右判定用）
        layermask = LayerMask.GetMask("CreateBlock","Block");//ここに追加したいレイヤー名を入れるとlayermaskがレイヤー判定を取るようになる
        //取得するレイヤーを獲得（足元判定用）
        groundlayermask = LayerMask.GetMask("Ground","Block");//ここに追加したいレイヤー名を入れるとgroundlayermaskがレイヤー判定を取るようになる

    }

    private void Update()
    {
        //クリック処理はUpdateでしましょう

        //Rayの原点＝プレイヤーの現在の位置
        origin_x = (Vector2)transform.position + offset;//(X方向）
        origin_y = (Vector2)transform.position;//(Y方向）

        direction = transform.right;//X方向を指す

        //プレイヤーの向いている向きにRayを飛ばす
        RaycastHit2D hit = Physics2D.Raycast(origin_x, direction, ray_length,layermask);

        // プレイヤーの足元にRayを飛ばす
        RaycastHit2D g_hit = Physics2D.Raycast(origin_y, Vector2.down, g_ray_lenght, groundlayermask);

        // Rayの可視化
        Debug.DrawLine(origin_x, origin_x + direction * ray_length, Color.red);//左右判定用のRay
        Debug.DrawLine(transform.position, transform.position + Vector3.down * g_ray_lenght, Color.blue);//着地判定用のRay


        //左右判定用のRayが当たった時の処理
        if (hit.collider != null)
        {
            Debug.DrawLine(origin_x, hit.point, Color.green);//デバッグ用のRayを可視化する処理

            // 当たったオブジェクトが自身でなければ、何かしらの処理をする
            if (hit.collider.gameObject != gameObject)
            {
                // Debug.Log("Hit object: " + hit.collider.gameObject.name);

                int layer = hit.collider.gameObject.layer;//Rayが当たったオブジェクトのレイヤーを入れる
                Debug.Log("当たったオブジェクトのレイヤーは" + LayerMask.LayerToName(layer) + "です。");

                //Rayが当たったのが移動指標オブジェクトの場合、ジャンプ処理をしない
                if (LayerMask.LayerToName(layer) == "CreateBlock")
                {
                    Debug.Log("tobanai");
                }
                //ジャンプ処理を行う
                else if (LayerMask.LayerToName(layer) == "Block")
                {
                    if (jumpCount < 1)
                    {
                        this.rb.AddForce(transform.up * jumpForce);
                        jumpCount++;
                    }
                }
            }

        }

        // Rayが地面に当たった場合、isGroundedをtrueにする
        if (g_hit.collider != null)
        {
            Debug.Log("じめんあり");
            Debug.DrawLine(origin_y, g_hit.point, Color.yellow);//デバッグ用のRayを可視化する処理
            isGrounded = true;
        }
        else
        {
            Debug.Log("じめんなし");
            isGrounded = false;
        }



        // 移動中でなければクリックを受け付ける
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Debug.Log("移動");
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
                Debug.Log("右");
            }
            else//左
            {
                offset = new Vector2(-0.5f * playerSize, 0f);//左向き
                transform.eulerAngles = new Vector3(0, 180, 0);
                Debug.Log("左");
            }

            // 移動を開始
            isMoving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {

            //デバッグ用のキー移動処理(終わったら消す）---------------------------------

            //if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
            //{
            //    isMoving = false;//移動処理を強制終了
            //    this.rb.AddForce(transform.up * jumpForce);
            //    jumpCount++;
            //}

            //--------------------------------------------------

            //FreezeRotationのみオンにする（Freezeは上書きできる）
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            speed = 5.0f;

            //落下処理（仮）　とりあえず今は落ちたら初期位置に戻る
            if (transform.position.y <= -10)
            {
                Debug.Log("やり直す");
                MoveFinish();//移動処理を強制終了
                transform.position = firstpos;
            }

           
            // 移動中の場合は移動する
            if (isMoving)
            {
                //Debug.Log("a");
                // キャラクターのX座標をクリックされた位置に向けて移動
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // 移動が終わったらフラグを解除
                if (transform.position.x == clickPosition.x)
                {
                    //Debug.Log("b");
                    playerPosition = transform.position;//playerPositionを更新
                    MoveFinish();//移動処理終了
                }
            }
            else if (hitMoving)
            {
                Debug.Log("akys");
                MoveFinish();//移動処理終了
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x+0.01f, transform.position.y), speed * Time.deltaTime);
                playerPosition = transform.position;//playerPositionを更新
            }

          
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
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("ぶつかってるhc");
            hitMoving = false;//
            jumpCount = 0;
        }

        //ブロックにぶつかったとき
        if (other.gameObject.CompareTag("MoveBlock"))
        {
            Debug.Log("ぶつかってる");
            // キャラクターのX座標をクリックされた位置に向けて移動
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y), speed * Time.deltaTime);
            MoveFinish();//移動処理を強制終了
            hitMoving = true;//ブロックにぶつかったときの挙動を行う
        }
    }

}
