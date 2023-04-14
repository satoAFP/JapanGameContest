using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 5f;

    // クリックされた位置
    private Vector3 clickPosition;

    // 移動中かどうかのフラグ
    private bool isMoving = false;

    [SerializeField] private Vector2 origin;//rayの原点

    private Vector2 direction;//rayの方向ベクトル

    [SerializeField] private LayerMask[] layermask;//レイヤーマスク

    int layerMask = ~(1 << 8);

    [SerializeField,Header("テスト用Rayの長さ調整")]
    private float ray_length = 5.0f;

    void Update()
    {
        origin = transform.position;

        direction = transform.right;//X方向を指す

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, ray_length, layermask[0]);

        // Rayの可視化
        Debug.DrawLine(origin, origin + direction * ray_length, Color.red);

        if (hit.collider != null)
        {
            Debug.DrawLine(origin, hit.point, Color.green);

            // 当たったオブジェクトが自身でなければ、何かしらの処理をする
            if (hit.collider.gameObject != gameObject)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }

        }


        // Debug.DrawRay(ray.origin, ray.direction * ray_length, Color.yellow);

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
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(clickPosition.x, transform.position.y), moveSpeed * Time.deltaTime);

            // 移動が終わったらフラグを解除
            if (transform.position.x == clickPosition.x)
            {
                isMoving = false;
            }
        }

    }

  
}
