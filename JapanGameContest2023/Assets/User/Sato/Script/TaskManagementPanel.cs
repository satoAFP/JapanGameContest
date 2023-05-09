using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManagementPanel : MonoBehaviour
{
    [SerializeField, Header("CPU表示用スライダー")] private Slider CPUSlider;

    [SerializeField, Header("スライダーの色変更用")] private Image FillImage;

    [SerializeField, Header("CPU数値表示用テキスト")] private Text CPUText;

    [SerializeField, Header("タイム表示用テキスト")] private Text timeText;

    [SerializeField, Header("ブロック数表示用テキスト")] private Text blockText;

    [SerializeField, Header("タスクマネージャー表示")] private GameObject taskManagement;


    [SerializeField,Header("出せるオブジェクトの最大数")] private int objMax;

    [SerializeField, Header("スライダーの色変更用")] private Color[] color;


    //時間表示用
    private int frame = 0;
    private int second = 0;
    private int minute = 0;

    // Update is called once per frame
    void FixedUpdate()
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

        //時間計算
        TimeCount();

        //CPUの使用率を入力
        CPUSlider.value = (float)childObj / (float)objMax;
        CPUText.text = (((float)childObj / (float)objMax) * 100).ToString("N1") + "%";
        //経過時間表示
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
        //現在のblockの数表示
        blockText.text = childObj.ToString();
    }

    //CPUパネル
    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }

    //時間表示用
    private void TimeCount()
    {
        frame++;

        if (frame >= 50) 
        {
            second++;
            frame = 0;
        }

        if (second >= 60) 
        {
            minute++;
            second = 0;
        }

    }

}
