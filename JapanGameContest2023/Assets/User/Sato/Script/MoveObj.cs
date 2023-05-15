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
            //編集モードの時のみ
            if (!managerAccessor.Instance.dataMagager.playMode)
            {
                managerAccessor.Instance.dataMagager.onBlock = true;
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            //編集モードの時のみ
            if (!managerAccessor.Instance.dataMagager.playMode)
            {
                managerAccessor.Instance.dataMagager.onBlock = false;
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Goal" ||
            collision.gameObject.tag == "DecoyFile")
        {
            managerAccessor.Instance.dataMagager.onBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        }
    }
}
