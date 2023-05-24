using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjhitCollider : MonoBehaviour
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
        // ���������I�u�W�F�N�g�̃��C���[�𔻒肷��
        int layer = other.gameObject.layer;

        if (LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
        {
            // ����̃��C���[�ɓ��������ꍇ�̏���
            player.Objhit = true;//�㏸�J�n�t���OON

        }

       
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        // ���������I�u�W�F�N�g�̃��C���[�𔻒肷��
        int layer = other.gameObject.layer;

        if (LayerMask.LayerToName(layer) == "Block" || LayerMask.LayerToName(layer) == "Ground")
        {
            // ����̃��C���[�ɓ��������ꍇ�̏���
            Debug.Log("���ꂽ��");
            player.Objhit = false;//�㏸�J�n�t���OOFF
        }
    }


}
