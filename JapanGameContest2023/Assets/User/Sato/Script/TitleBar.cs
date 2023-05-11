using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBar : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W���\���e�L�X�g")] private Text titleText;

    //�ŏ��̈�񂵂��ʂ�Ȃ��p
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
