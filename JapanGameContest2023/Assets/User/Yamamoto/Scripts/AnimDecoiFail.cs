using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDecoiFail : MonoBehaviour
{
    [SerializeField] private GameObject animdecoifail;//�X�e�[�W1�p�o��A�j���[�V�������Ƃ�t�@�C��

    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        animdecoifail.SetActive(false);//�ŏ��͔�\����
        Invoke(nameof(FileAppear), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if( player.StartAction )
        {
            animdecoifail.SetActive(false);//�o��A�j���[�V�������I���ƃt�@�C����\��
        }
    }

    void FileAppear()
    {
        animdecoifail.SetActive(true);//�����ŕ\��
    }

}
