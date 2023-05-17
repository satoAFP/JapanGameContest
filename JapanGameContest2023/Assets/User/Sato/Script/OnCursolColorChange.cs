using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCursolColorChange : MonoBehaviour
{
    [SerializeField, Header("アルファ値")] private float alpha;

    // Update is called once per frame
    void Update()
    {
        //必要な情報の取得
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;
        Color color = gameObject.GetComponent<Image>().color;
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //オブジェクト内にカーソルが入っている時、切り替える
        if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
        {
            color.a = alpha;
            gameObject.GetComponent<Image>().color = color;

            //モードチェンジのみカーソルが禁止になる場合がる
            if (dataManager.isMoving || dataManager.onBlock) 
            {
                if (gameObject.transform.parent.gameObject.name == "ChangeButton")
                {
                    managerAccessor.Instance.dataMagager.isNoClick = true;
                }
            }
            else
            {
                dataManager.isNoClick = false;
            }
        }
        else
        {
            color.a = 0;
            gameObject.GetComponent<Image>().color = color;

            if (gameObject.transform.parent.gameObject.name == "ChangeButton")
            {
                managerAccessor.Instance.dataMagager.isNoClick = false;
            }
        }
    }
}
