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

    private int previousCount;//�v���C���[���X�V�̂��߂̕ϐ�

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");//Player�^�O�������Ă���I�u�W�F�N�g�擾
        ListCount = Players.Length;//Players�̒������擾

        

        // �O��̃t���[���ł̃v���C���[�̐��ƌ��݂̃v���C���[�̐����Ⴄ�ꍇ�A���O���o�͂���
        if (previousCount != ListCount)
        {
            Debug.Log("�v���C���[�̐����ύX����܂����B���݂̃v���C���[�̐���" + ListCount + "�ł��B");
            previousCount = ListCount;
        }

    }
}
