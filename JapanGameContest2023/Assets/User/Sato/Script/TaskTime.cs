using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TaskTime : MonoBehaviour
{
    [SerializeField, Header("時間表示用")] private Text timeText;

    [SerializeField, Header("日にち表示用")] private Text dayText;


    //現在時刻取得用
    private DateTime dt;



    // Update is called once per frame
    void Update()
    {
        dt = DateTime.Now;
        timeText.text = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
        dayText.text = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
    }
}
