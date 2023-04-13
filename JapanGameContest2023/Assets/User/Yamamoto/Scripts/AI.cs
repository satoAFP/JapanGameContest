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

    void Update()
    {
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
