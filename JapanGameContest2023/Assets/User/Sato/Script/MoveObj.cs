using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    [SerializeField, Header("重なってほしくないオブジェクトのタグ名")] private string[] tagNames;

    //選択されているときのオブジェクトナンバー取得用
    [System.NonSerialized] public int objNum;

    private Rigidbody2D rigidbody2D;    //リジットボディ取得用

    private void Start()
    {
        //複製時色もコピーされてしまうため、初期化で元の色に戻す
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);

        //リジットボディの取得
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //ブロックが乗っているとき
        if(managerAccessor.Instance.dataMagager.onBlock)
        {
            //処理軽減のため止まっているStayを動かす
            rigidbody2D.WakeUp();
        }
    }


    //乗ってはいけないオブジェクトの上にいるときの処理-----------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++) 
        {
            if(collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    //ブロックが乗っている判定に変更
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    //色の変更
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    //ブロックが乗っていない判定に変更
                    managerAccessor.Instance.dataMagager.onBlock = false;
                    //色の変更
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    //ブロックが乗っている判定に変更
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    //色の変更
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    managerAccessor.Instance.dataMagager.onBlock = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < tagNames.Length; i++)
        {
            if (collision.gameObject.tag == tagNames[i])
            {
                //編集モードの時のみ
                if (!managerAccessor.Instance.dataMagager.playMode)
                {
                    //ブロックが乗っていない判定に変更
                    managerAccessor.Instance.dataMagager.onBlock = false;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                }
            }
        }
    }
}
