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

    [SerializeField, Header("ウイルスが感染したとき")] private string infectionText;

    [SerializeField, Header("ウイルスが感染したときヒント")] private string infectionHintText;

    [SerializeField, Header("オブジェクト出しすぎの時")] private string overText;

    [SerializeField, Header("オブジェクト出しすぎの時ヒント")] private string overHintText;

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時パネルを出す
        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //それぞれの死因でテキストを変える
            if(managerAccessor.Instance.dataMagager.fallDeth)
            {
                putText.text = fallText;
                hintText.text = fallHintText;
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
        }
    }
}
