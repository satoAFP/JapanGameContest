using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjMaxControler : MonoBehaviour
{
    [SerializeField, Header("CPU表示用スライダー")] private Slider CPUSlider;

    [SerializeField, Header("スライダーの色変更用")] private Image FillImage;

    [SerializeField, Header("CPU数値表示用テキスト")] private Text CPUText;

    [SerializeField, Header("タスクマネージャー表示")] private GameObject taskManagement;


    [SerializeField,Header("出せるオブジェクトの最大数")] private int objMax;

    [SerializeField, Header("スライダーの色変更用")] private Color[] color;

    // Update is called once per frame
    void Update()
    {
        //動かせるブロック数を取得
        int childObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;


        //最大数未満の時
        if(childObj < objMax)
        {
            FillImage.color = color[0];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //最大数の時
        else if (childObj == objMax) 
        {
            FillImage.color = color[1];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //最大数を超えた時
        else if (childObj > objMax)
        {
            FillImage.color = color[2];
            managerAccessor.Instance.dataMagager.objMaxFrag = true;
        }

        //CPUの使用率を入力
        CPUSlider.value = (float)childObj / (float)objMax;
        CPUText.text = (((float)childObj / (float)objMax) * 100).ToString("N1") + "%";
    }


    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }


}
