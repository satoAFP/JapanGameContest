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

    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");

        ListCount = Players.Length;//Playersの長さを取得
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
