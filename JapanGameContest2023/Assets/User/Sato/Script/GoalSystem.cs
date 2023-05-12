using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [SerializeField, Header("ゴールするキャラの数")] private GameObject ClearPanel;

    [SerializeField, Header("ゴールするキャラの数")] private int charaNum;

    //一回しか通らない
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        //ゴールの数だけキャラクターが入った時
        if (charaNum == managerAccessor.Instance.dataMagager.goalPlayerNum) 
        {
            if (first)
            {
                //ゴールしたことを記憶
                for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++)
                {
                    //現在のステージ名と一致した時
                    if (managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage" + (i + 1))
                    {
                        PlayerPrefs.SetInt("Stage" + i, 1);
                        PlayerPrefs.Save();
                    }
                }

                //クリアパネルの表示
                ClearPanel.SetActive(true);

                first = false;
            }
        }
    }
}
