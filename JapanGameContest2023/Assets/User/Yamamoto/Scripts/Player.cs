using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;//プレイヤー速度

    private float jumpForce = 350f;//プレイヤージャンプ力

    private int jumpCount = 0;//複数入力させない

    private Rigidbody2D rb;//プレイヤーリジッドボディ

    public Vector2 firstpos;//初期位置（仮）

    //移動判定用の変数(マウス用）
    bool isMoving = false;

    // クリックされた位置
    private Vector3 clickPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(managerAccessor.Instance.dataMagager.playMode)
        {
            Vector2 position = transform.position;

            if(position.y<=-10)//落下処理（仮）　とりあえず今は落ちたら初期位置に戻る
            {
                Debug.Log("やり直す");

                position = firstpos;//初期位置に戻す
            }

            //移動処理（キー）
            //speed = 0.05f;

            //if (Input.GetKey(KeyCode.A))
            //{
            //    position.x -= speed;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    position.x += speed;
            //}

            //if (Input.GetKey(KeyCode.W) && this.jumpCount < 1)
            //{
            //    this.rb.AddForce(transform.up * jumpForce);
            //    jumpCount++;
            //}

            //transform.position = position;

            speed = 5.0f;

            // 移動中でなければクリックを受け付ける
            if (!isMoving && Input.GetMouseButtonDown(0))
            {
                Debug.Log("移動");
                // クリックされた位置を取得
                clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0; // z座標を0に設定（2Dゲームなので）

                // 移動を開始
                isMoving = true;
            }

            // 移動中の場合は移動する
            if (isMoving)
            {
                // キャラクターのX座標をクリックされた位置に向けて移動
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), speed * Time.deltaTime);

                // 移動が終わったらフラグを解除
                if (transform.position.x == clickPosition.x)
                {
                    isMoving = false;
                }
            }

          
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            jumpCount = 0;
        }
    }

}
