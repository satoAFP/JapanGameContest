using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleSystem : MonoBehaviour
{
    [SerializeField, Header("���ԕ\���p")] private Text timeText;

    [SerializeField, Header("���ɂ��\���p")] private Text dayText;

    [SerializeField, Header("LoginMenu")] private GameObject loginMenu;


    //���ݎ����擾�p
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
        dayText.text = dt.Month.ToString() + "��" + dt.Day.ToString() + "��";
    }

    public void ClickTitle()
    {
        anim.SetBool("putButton", true);
    }
}
