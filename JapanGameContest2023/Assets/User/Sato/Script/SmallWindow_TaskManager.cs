using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow_TaskManager : MonoBehaviour
{
    [SerializeField, Header("SmallWindow入れる")] private GameObject smallWindow;
    [SerializeField, Header("NoTapArea入れる")] private GameObject noTapArea;

    [SerializeField, Header("CPU表示用スライダー")] private Slider CPUSlider;

    [SerializeField, Header("スライダーの色変更用")] private Image FillImage;

    [SerializeField, Header("CPU数値表示用テキスト")] private Text CPUText;

    [SerializeField, Header("スライダーの色変更用")] private Color[] color;


    private int blockChildObj = 0;  //動かせるブロックの数格納

    private int objMax = 0;         //シーン内に出せるブロックの最大数

    private bool isOnTab = false;   //カーソルがタブの上に乗っているとき

    //最初しか通らない
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //動かせるブロック数を取得
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        objMax = managerAccessor.Instance.dataMagager.objMax;

        //必要な情報の取得
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;

        //マウスが座標内にいるとき
        if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
        {
            if (first)
            {
                //カーソルが乗っているとき出す
                smallWindow.SetActive(true);
                noTapArea.SetActive(true);

                //カーソルがタブに乗っているとき
                isOnTab = true;
                first = false;
            }

        }
        else
        {
            //必要な情報の取得
            Vector2 npos = noTapArea.GetComponent<RectTransform>().position;
            Vector2 nsize = noTapArea.GetComponent<RectTransform>().sizeDelta;

            //タブにカーソルが乗っていたとき&&NoTapAreaにカーソルが乗ってるいるとき
            if (!(npos.x - (nsize.x / 2) < mouse.x && npos.x + (nsize.x / 2) > mouse.x &&
                npos.y - (nsize.y / 2) < mouse.y && npos.y + (nsize.y / 2) > mouse.y && isOnTab))
            {
                smallWindow.SetActive(false);
                noTapArea.SetActive(false);
                first = true;
                isOnTab = false;
            }
        }

        //最大数未満の時
        if (blockChildObj < objMax)
        {
            FillImage.color = color[0];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //最大数の時
        else if (blockChildObj == objMax)
        {
            FillImage.color = color[1];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //最大数を超えた時
        else if (blockChildObj > objMax)
        {
            FillImage.color = color[2];
            managerAccessor.Instance.dataMagager.objMaxFrag = true;
        }
        Debug.Log((float)blockChildObj / (float)objMax);

        //CPUの使用率を入力
        CPUSlider.value = (float)blockChildObj / (float)objMax;
        CPUText.text = (((float)blockChildObj / (float)objMax) * 100).ToString("N1") + "%";

    }
}
