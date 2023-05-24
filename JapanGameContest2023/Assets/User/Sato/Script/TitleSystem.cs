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
    //�A�j���[�^�[�擾�p
    private Animator anim;

    private bool isFrashTap = false;
    private bool isLoginTap = false;

    private bool first = true;
    private bool first2 = true;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dt = DateTime.Now;
        timeText.text = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
        dayText.text = dt.Month.ToString() + "��" + dt.Day.ToString() + "��" + "(" + Week(dt.Year, dt.Month, dt.Day) + ")";

        if (Input.GetKey(KeyCode.Return))
        {
            if (first)
            {
                if(!isFrashTap)
                {
                    isFrashTap = true;
                }
                else
                {
                    isLoginTap = true;
                }

                first = false;
            }
        }
        else
        {
            first = true;
        }

        if (isFrashTap)
        {
            if (first2)
                anim.SetBool("putButton", true);
            first2 = false;
        }
        if (isLoginTap)
        {
            managerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    //�N��������j�������߂�֐�
    private string Week(int y, int m, int d)
    {
        //��T��
        string[] week = { "��", "��", "��", "��", "��", "��", "�y", };
        //�c�F���[�̌���
        int w= (y + y / 4 - y / 100 + y / 400 + (13 * m + 8) / 5 + d) % 7;

        return week[w];
    }

    public void ClickTitle()
    {
        isFrashTap = true;
    }

    public void ClickLogIn()
    {
        isLoginTap = true;
    }
}
