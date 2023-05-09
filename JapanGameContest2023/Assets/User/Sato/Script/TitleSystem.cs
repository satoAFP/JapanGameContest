using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleSystem : MonoBehaviour
{
    [SerializeField, Header("時間表示用")] private Text timeText;

    [SerializeField, Header("日にち表示用")] private Text dayText;

    [SerializeField, Header("LoginMenu")] private GameObject loginMenu;


    //現在時刻取得用
    private DateTime dt;

    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dt = DateTime.Now;
        timeText.text = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
        dayText.text = dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
    }

    public void ClickTitle()
    {
        anim.SetBool("putButton", true);
    }
}
