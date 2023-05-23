using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManagement : MonoBehaviour
{
    //AudioSource取得用
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーの時、シャットダウンの時BGMを消す
        if (managerAccessor.Instance.dataMagager.playerlost || managerAccessor.Instance.dataMagager.isShutDown) 
        {
            audio.volume = 0;
        }
    }
}
