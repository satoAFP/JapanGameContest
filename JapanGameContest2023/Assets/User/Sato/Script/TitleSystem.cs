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
        dayText.text = dt.Month.ToString() + "月" + dt.Day.ToString() + "日" + "(" + Week(dt.Year, dt.Month, dt.Day) + ")";

        
    }

    //年月日から曜日を求める関数
    private string Week(int y, int m, int d)
    {
        //一週間
        string[] week = { "日", "月", "火", "水", "木", "金", "土", };
        //ツェラーの公式
        int w= (y + y / 4 - y / 100 + y / 400 + (13 * m + 8) / 5 + d) % 7;

        return week[w];
    }

    public void ClickTitle()
    {
        anim.SetBool("putButton", true);
    }
}
