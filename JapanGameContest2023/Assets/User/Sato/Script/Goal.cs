using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [System.NonSerialized] public bool goalChara = true;

    //�S�[�����̃G�t�F�N�g�A�j���[�^�[�擾
    public Animator GoalEffect_animator;

    //�S�~���Ƀv���C���[�������Ă���A�j���[�V����
    public Animator DastBox_animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���ɓ���������
        if (collision.gameObject.tag == "Player") 
        {
            //�S�[������Ƃ����ɂ͂�������Ȃ�
            if (goalChara)
            {
               
            }
        }
    }


}
