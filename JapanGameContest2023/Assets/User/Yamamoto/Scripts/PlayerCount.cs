using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    //�Q�[���J�����Ƀv���C���[�̐��𐔂���

    [SerializeField, Header("�v���C���[�̐������擾")]
    private GameObject[] Players;

    [SerializeField]
    private int ListCount;//Players�̒����𐔂���

    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");

        ListCount = Players.Length;//Players�̒������擾
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
