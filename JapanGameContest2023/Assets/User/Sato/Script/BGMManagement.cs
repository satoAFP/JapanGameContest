using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManagement : MonoBehaviour
{
    //AudioSource�擾�p
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I�[�o�[�̎��A�V���b�g�_�E���̎�BGM������
        if (managerAccessor.Instance.dataMagager.playerlost || managerAccessor.Instance.dataMagager.isShutDown) 
        {
            audio.volume = 0;
        }
    }
}
