using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    //ゲーム開発時にプレイヤーの数を数える

    [SerializeField, Header("プレイヤーの数だけ取得")]
    private GameObject[] Players;

    [SerializeField]
    private int ListCount;//Playersの長さを数える

    private int previousCount;//プレイヤー数更新のための変数

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");//Playerタグを持っているオブジェクト取得
        ListCount = Players.Length;//Playersの長さを取得

        

        // 前回のフレームでのプレイヤーの数と現在のプレイヤーの数が違う場合、ログを出力する
        if (previousCount != ListCount)
        {
            Debug.Log("プレイヤーの数が変更されました。現在のプレイヤーの数は" + ListCount + "です。");
            previousCount = ListCount;
        }

    }
}
