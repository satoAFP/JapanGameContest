using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField, Header("複製する星のオブジェクト")] private GameObject starObj;

    [SerializeField, Header("複製する星の数")] private int starNum;

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            if(first)
            {
                for (int i = 0; i < starNum; i++) 
                {
                    GameObject clone = Instantiate(starObj);
                    clone.GetComponent<TapEffect_Star>().Move(AngleToVector2((i + 1) * (360 / starNum)));
                }

                first = false;
            }
        }
        else
        {
            first = true;
        }
    }

    //角度から移動ベクトルを求める関数
    private Vector2 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }
}
