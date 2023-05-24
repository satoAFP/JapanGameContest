using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjhitCollider : MonoBehaviour
{
    [SerializeField] private GameObject Player;//プレイヤーオブジェクト取得

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 当たったオブジェクトのレイヤーを判定する
        int layer = other.gameObject.layer;

        if (LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
        {
            // 特定のレイヤーに当たった場合の処理
            player.Objhit = true;//上昇開始フラグON

        }

       
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        // 当たったオブジェクトのレイヤーを判定する
        int layer = other.gameObject.layer;

        if (LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
        {
            // 特定のレイヤーに当たった場合の処理
            Debug.Log("離れた時");
            player.Objhit = false;//上昇開始フラグOFF
        }
    }


}
