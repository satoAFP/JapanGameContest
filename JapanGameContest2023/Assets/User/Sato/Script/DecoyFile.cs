using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyFile : MonoBehaviour
{
    [SerializeField, Header("���������Ƃ��̉摜")] private Sprite infectionFile;

    [SerializeField, Header("�A�j���[�V�����̑ҋ@����")] private int stopFrame;


    private int FrameCount = 0;
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�t�@�C���ɓ������
        if(managerAccessor.Instance.dataMagager.onDecoyFile)
        {
            FrameCount++;
            //�ݒ肵�Ă���t���[���ɂȂ�ƃQ�[���I�[�o�[
            if (stopFrame <= FrameCount) 
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //�L�����폜
            Destroy(collision.gameObject.GetComponent<Player>().CreateObj);
            Destroy(collision.gameObject);
            //���������t�@�C���ɂȂ�
            //gameObject.GetComponent<SpriteRenderer>().sprite = infectionFile;

            anim.SetBool("HitEnemy", true);

            managerAccessor.Instance.dataMagager.onDecoyFile = true;
            managerAccessor.Instance.dataMagager.infectionDeth = true;
        }
    }

}
