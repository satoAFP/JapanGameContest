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

    [SerializeField, Header("ゴール数表示用テキスト")] private Text goalText;

    [SerializeField, Header("タスクマネージャー表示")] private GameObject taskManagement;


    [SerializeField,Header("出せるオブジェクトの最大数")] private int objMax;

    [SerializeField, Header("スライダーの色変更用")] private Color[] color;

    //動かせるブロックの数格納
    private int blockChildObj = 0;

    //ゴールの数格納
    private int goalChildObj = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //動かせるブロック数を取得
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        goalChildObj = managerAccessor.Instance.objDataManager.goalParent.transform.childCount;

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
        //経過時間表示
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
        //現在のblockの数表示
        blockText.text = blockChildObj.ToString();
        //現在のゴールの数表示
        goalText.text = goalChildObj.ToString();
    }

    //CPUパネル
    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }

}
