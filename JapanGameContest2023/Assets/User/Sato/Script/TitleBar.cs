using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBar : MonoBehaviour
{
    [SerializeField, Header("ステージ名表示テキスト")] private Text titleText;

    //最初の一回しか通らない用
    private bool first = true;

    private void Update()
    {
        if(first)
        {
            titleText.text = managerAccessor.Instance.sceneMoveManager.GetSceneName();
            first = false;
        }
    }

    public void MoveStageSelect()
    {
        managerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
    }


}
