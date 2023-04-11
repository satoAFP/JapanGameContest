using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//プレイヤー速度

    private float jumpForce = 350f;//プレイヤージャンプ力

    private int jumpCount = 0;//複数入力させない

    private Rigidbody2D rb;//プレイヤーリジッドボディ

    [SerializeField] private bool movechange;//移動処理を変更（true:マウス false:キー）

    //移動判定用の変数(マウス用）
    bool isMoving;

    public Vector2 firstpos;//初期位置（仮）

    Vector3 mousePos, worldPos;//（マウスの位置とクリックした位置）

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstpos = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(managerAccessor.Instance.dataMagager.playMode)
        {
            Vector2 position = transform.position;

            if(position.y<=-10)//落下処理（仮）　とりあえず今は落ちたら初期位置に戻る
            {
                Debug.Log("やり直す");

                position = firstpos;//初期位置に戻す
            }

            if (!movechange)
            {
                speed = 0.05f;

                if (Input.GetKey(KeyCode.A))
                {
                    position.x -= speed;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    position.x += speed;
                }

                if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
                {
                    this.rb.AddForce(transform.up * jumpForce);
                    jumpCount++;
                }
            }
            else
            {
                speed = 5.0f;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("もべ");
                    //マウスの座標取得
                    mousePos = Input.mousePosition;
                    //スクリーン座標をワールド座標に変換
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);


                    position = new Vector2(worldPos.x, firstpos.y);
                }

                //移動中なら処理を受け付けない
                //if (isMoving)
                //{
                //    return;
                //}

                //移動していない場合の処理
                //左クリックされたら
                //if (Input.GetMouseButtonDown(0))
                //{
                //    Debug.Log("もべ");

                //    //マウスの座標取得
                //    mousePos = Input.mousePosition;
                //    //スクリーン座標をワールド座標に変換
                //    worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
                //    //コルーチンスタート
                //    StartCoroutine(_move());
                //}
            }


            transform.position = position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            jumpCount = 0;
        }
    }

    //移動用コルーチン
    //IEnumerator _move()
    //{
    //    //移動フラグをtrue
    //    isMoving = true;

    //    //ワールド座標と自身の座標を比較しループ
    //    while ((worldPos - transform.position).sqrMagnitude > Mathf.Epsilon)
    //    {
    //        //指定した座標に向かって移動
    //        transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);
    //        //1フレーム待つ
    //        yield return null;
    //    }
    //    //移動フラグをfalse
    //    isMoving = false;
    //}
}
