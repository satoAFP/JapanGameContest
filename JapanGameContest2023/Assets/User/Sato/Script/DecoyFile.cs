using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyFile : MonoBehaviour
{
    [SerializeField, Header("���������Ƃ��̉摜")] private Sprite infectionFile;

    [SerializeField, Header("�A�j���[�V�����̑ҋ@����")] private int stopFrame;

    [SerializeField] private AudioClip infectionse;//�������ʉ�

    private AudioSource audioSource;


    private int FrameCount = 0;
    private Animator anim;

    [SerializeField, Header("���������G�t�F�N�g�A�j���[�V����")]
    private Animator effect_ani;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//�X�N���v�g�擾
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
           // Destroy(collision.gameObject.GetComponent<Player>().CreateObj);
            Destroy(collision.gameObject);
            //���������t�@�C���ɂȂ�
            //gameObject.GetComponent<SpriteRenderer>().sprite = infectionFile;


            //�����ŃA�j���[�V�����𗬂��t���O��ON
            effect_ani.SetBool("PlayerHit", true);
            audioSource.PlayOneShot(infectionse);//����SE�炷

            StartCoroutine("ChangeDecoyFile");//�R���[�`���J�n

            managerAccessor.Instance.dataMagager.onDecoyFile = true;
            managerAccessor.Instance.dataMagager.infectionDeth = true;
        }
    }

    IEnumerator ChangeDecoyFile()
    {
        yield return new WaitForSeconds(0.3f);//���ԍ��ŃA�j���[�V�������s��

        anim.SetBool("HitEnemy", true);
    }

}
