using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�A�j���[�^�[�ɒ���SE������X�N���v�g�ł�(�v���C���[�p�j

public class SEPlay : MonoBehaviour
{

    [Header("�����Ɍ��ʉ�������")]
    [SerializeField] private AudioClip walkse;//���sSE
    [SerializeField] private AudioClip fallse;//����SE
   
    private AudioSource audioSource;

    public bool startse = false;//SE��炷�t���O

    public bool onese = true;//�Ăяo���ꂽ�Ƃ���x��������炷

    public bool playfallse = false;//����SE��炷�t���O(true�̓A�j���[�^�[�ł���)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();//�X�N���v�g�擾
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //���݃v���C���[������ł��Ȃ����SE��炷
        if(!managerAccessor.Instance.dataMagager.fallDeth||
           !managerAccessor.Instance.dataMagager.infectionDeth ||
           !managerAccessor.Instance.dataMagager.overDeth)
        {
            if (startse)//�A�j���[�^�[�̕���startse��true�ɂ���
            {
                if (onese)
                {
                    if (!managerAccessor.Instance.dataMagager.playerlost && !managerAccessor.Instance.dataMagager.isShutDown)
                    {
                        audioSource.PlayOneShot(walkse);
                        onese = false;
                        Debug.Log("sss");
                    }
                }
            }
            else
            {
                onese = true;
            }


            if(playfallse)
            {
                StartFallSE();
                playfallse = false;//�����Ńt���O������
            }

        }
        
    }



    public void StartFallSE()
    {
        Debug.Log("staru");
        audioSource.PlayOneShot(fallse);
    }
}
