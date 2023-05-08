using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    //選択されているときのオブジェクトナンバー取得用
    [System.NonSerialized] public int objNum;


    //乗ってはいけないオブジェクトの上にいるときの処理-----------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = true;
            Debug.Log("aaa");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = false;
        }
    }
}
