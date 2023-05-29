using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField, Header("ブルスクが出たときの原因")] private Text putText;

    [SerializeField, Header("ブルスクが出たときのヒント")] private Text hintText;


    [SerializeField, Header("ウイルスが落下したとき")] private string fallText;

    [SerializeField, Header("ウイルスが落下したときのヒント")] private string fallHintText;

    [SerializeField, Header("ウイルスが落下したときのヒント")] private GameObject fallImg;

    [SerializeField, Header("ウイルスが感染したとき")] private string infectionText;

    [SerializeField, Header("ウイルスが感染したときヒント")] private string infectionHintText;

    [SerializeField, Header("オブジェクト出しすぎの時")] private string overText;

    [SerializeField, Header("オブジェクト出しすぎの時ヒント")] private string overHintText;

    [SerializeField, Header("時間制限を超えた時")] private string timeText;

    [SerializeField, Header("時間制限を超えた時ヒント")] private string timeHintText;

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時パネルを出す
        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            fallImg.SetActive(false);

            //それぞれの死因でテキストを変える
            if (managerAccessor.Instance.dataMagager.fallDeth)
            {
                putText.text = fallText;
                hintText.text = fallHintText;
                fallImg.SetActive(true);
            }
            else if(managerAccessor.Instance.dataMagager.infectionDeth)
            {
                putText.text = infectionText;
                hintText.text = infectionHintText;
            }
            else if(managerAccessor.Instance.dataMagager.overDeth)
            {
                putText.text = overText;
                hintText.text = overHintText;
            }
            else if (managerAccessor.Instance.dataMagager.timeDeth)
            {
                putText.text = timeText;
                hintText.text = timeHintText;
            }
        }
    }
}
