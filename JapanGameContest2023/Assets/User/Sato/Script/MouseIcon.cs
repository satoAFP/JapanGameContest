using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIcon : MonoBehaviour
{
    //サイズ変更時持っている縁の位置の名前
    public enum ChangeSizePosName
    {
        DOWN,
        RIGHT,
        UP,
        LEFT,
        RIGHT_DOWN,
        RIGHT_UP,
        LEFT_UP,
        LEFT_DOWN,
        NONE,
    }


    [SerializeField, Header("カーソルの画像")] private Sprite cursor;

    [SerializeField, Header("矢印の画像")] private Sprite arrow;

    [SerializeField, Header("ロードするオブジェクト")] private GameObject loadImg;

    [SerializeField, Header("マウスの位置ずれた差分加算用")] private Vector3 cursorMove;


    private Vector3 loadRotate = new Vector2(0, 0);

    // Update is called once per frame
    void FixedUpdate()
    {
        //マウスの位置に合わせる
        gameObject.GetComponent<RectTransform>().position = Input.mousePosition + cursorMove;

        //カーソルがそれぞれの縁に乗っているとき画像を矢印に変える
        if (managerAccessor.Instance.dataMagager.onEdge && !managerAccessor.Instance.dataMagager.playMode)  
        {
            gameObject.GetComponent<Image>().sprite = arrow;

            if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.DOWN ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.UP) 
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT_DOWN ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT_UP)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 45);
            }
            else if (managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.RIGHT_UP ||
                managerAccessor.Instance.dataMagager.whereEdge == (int)ChangeSizePosName.LEFT_DOWN)
            {
                gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -45);
            }
        }
        //通常時
        else
        {
            gameObject.GetComponent<Image>().sprite = cursor;
            gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
        }

        //シーン移動が始まるとロード中の画像に代わる
        if (managerAccessor.Instance.dataMagager.sceneMoveStart)
        {
            //カーソル非表示
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            //ロード画像表示
            loadImg.SetActive(true);
            //回転処理
            loadRotate.z -= managerAccessor.Instance.dataMagager.loadRotate;
            loadImg.GetComponent<RectTransform>().eulerAngles = loadRotate;
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            loadImg.SetActive(false);
            loadImg.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }

    }
}
