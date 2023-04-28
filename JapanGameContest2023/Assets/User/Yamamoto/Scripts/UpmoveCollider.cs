using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpmoveCollider : MonoBehaviour
{

    [SerializeField] private GameObject Player;//プレイヤーオブジェクト取得

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {
            player.setblock = true;//プレイヤーの足判定がついている時
            Debug.Log("set");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {
            player.setblock = true;//プレイヤーの足判定がついている時
            Debug.Log("2set"+ other.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {
            player.setblock = false;//プレイヤーの足判定がついていない時
            Debug.Log("remove");
        }
    }
}
