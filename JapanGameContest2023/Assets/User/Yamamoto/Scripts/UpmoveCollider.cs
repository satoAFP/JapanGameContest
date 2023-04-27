using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpmoveCollider : MonoBehaviour
{

    [SerializeField] private GameObject Player;//�v���C���[�I�u�W�F�N�g�擾

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {
            player.setblock = true;//�v���C���[�̑����肪���Ă��鎞
            //Debug.Log("�����܂�");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("MoveBlock"))
        {
            player.setblock = false;//�v���C���[�̑����肪���Ă��鎞
            //Debug.Log("�͂Ȃ͂Ȃ���");
        }
    }
}
