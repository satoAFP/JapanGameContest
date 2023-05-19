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

    //�S�~���̃C���X�g�ύX�t���O
    public bool change = false;

    private void Update()
    {
        if(change)
        {
            StartCoroutine("ChangeDastBox");
        }
    }

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

    IEnumerator ChangeDastBox()
    {
        yield return new WaitForSeconds(0.15f);

        DastBox_animator.SetBool("dastboxchange", true);//�S�~���̃C���X�g�ω�
    }

}
