using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlay : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("歩行SE")] private AudioClip walkse;

    private AudioSource audioSource;

    public bool startse = false;

    public bool onese = true;//呼び出されたとき一度だけ音を鳴らす



    void Start()
    {
        audioSource = GetComponent<AudioSource>();//スクリプト取得
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
