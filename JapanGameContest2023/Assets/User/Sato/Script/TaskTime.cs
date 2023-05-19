using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TaskTime : MonoBehaviour
{
    [SerializeField, Header("���ԕ\���p")] private Text timeText;

    [SerializeField, Header("���ɂ��\���p")] private Text dayText;


    //���ݎ����擾�p
    private DateTime dt;



    // Update is called once per frame
    void Update()
    {
        dt = DateTime.Now;
        timeText.text = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
        dayText.text = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
    }
}
