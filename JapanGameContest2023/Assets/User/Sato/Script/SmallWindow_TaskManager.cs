using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow_TaskManager : MonoBehaviour
{
    [SerializeField, Header("CPU表示用スライダー")] private Slider CPUSlider;

    [SerializeField, Header("スライダーの色変更用")] private Image FillImage;

    [SerializeField, Header("CPU数値表示用テキスト")] private Text CPUText;

    [SerializeField, Header("スライダーの色変更用")] private Color[] color;


    private int blockChildObj = 0;  //動かせるブロックの数格納

    private int objMax = 0;         //シーン内に出せるブロックの最大数

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //動かせるブロック数を取得
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        objMax = managerAccessor.Instance.dataMagager.objMax;

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


        //CPUの使用率を入力
        CPUSlider.value = (float)blockChildObj / (float)objMax;
        CPUText.text = (((float)blockChildObj / (float)objMax) * 100).ToString("N1") + "%";

    }
}
