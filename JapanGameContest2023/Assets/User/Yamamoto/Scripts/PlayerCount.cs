using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    //�Q�[���J�����Ƀv���C���[�̐��𐔂���

    [SerializeField, Header("�v���C���[�̐������擾")]
    private GameObject[] Players;



    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Players == null)
        {
            Debug.Log("nainaianmaonaia");
        }
    }
}
