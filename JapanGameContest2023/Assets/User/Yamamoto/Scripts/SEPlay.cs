using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlay : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("���sSE")] private AudioClip walkse;

    private AudioSource audioSource;

    public bool startse = false;

    public bool onese = true;//�Ăяo���ꂽ�Ƃ���x��������炷



    void Start()
    {
        audioSource = GetComponent<AudioSource>();//�X�N���v�g�擾
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!managerAccessor.Instance.dataMagager.fallDeth||
           !managerAccessor.Instance.dataMagager.infectionDeth ||
           !managerAccessor.Instance.dataMagager.overDeth)
        {
            if (startse)
            {
                if (onese)
                {
                    audioSource.PlayOneShot(walkse);
                    onese = false;
                    Debug.Log("sss");
                }
            }
            else
            {
                onese = true;
            }
        }
        
    }

    void SEstart()
    {

    }
}
