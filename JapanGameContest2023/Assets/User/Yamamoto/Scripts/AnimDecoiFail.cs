using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDecoiFail : MonoBehaviour
{
    [SerializeField] private GameObject animdecoifail;//ステージ1用登場アニメーションおとりファイル

    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        animdecoifail.SetActive(false);//最初は非表示に
        Invoke(nameof(FileAppear), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if( player.StartAction )
        {
            animdecoifail.SetActive(false);//登場アニメーションが終わるとファイル非表示
        }
    }

    void FileAppear()
    {
        animdecoifail.SetActive(true);//ここで表示
    }

}
