using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [SerializeField, Header("�S�[������L�����̐�")] private GameObject ClearPanel;

    [SerializeField, Header("�S�[������L�����̐�")] private int charaNum;

    //��񂵂��ʂ�Ȃ�
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        //�S�[���̐������L�����N�^�[����������
        if (charaNum == managerAccessor.Instance.dataMagager.goalPlayerNum) 
        {
            if (first)
            {
                //�S�[���������Ƃ��L��
                for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++)
                {
                    //���݂̃X�e�[�W���ƈ�v������
                    if (managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage" + (i + 1))
                    {
                        PlayerPrefs.SetInt("Stage" + i, 1);
                        PlayerPrefs.Save();
                    }
                }

                //�N���A�p�l���̕\��
                ClearPanel.SetActive(true);

                first = false;
            }
        }
    }
}
