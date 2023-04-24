using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        managerAccessor.Instance.sceneMoveManager = this;
    }

    //���O����Q�Ƃ��ăV�[���ړ�
    public void SceneMoveName(string name)
    {
        SceneManager.LoadScene(name);
    }

    //���̃X�e�[�W�ɃV�[���ړ�
    public void SceneMoveNext()
    {
        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            if (SceneManager.GetActiveScene().name == "Stage" + (i + 1))
            {
                SceneManager.LoadScene("Stage" + (i + 2));
            }
        }
    }

    //�����[�h����
    public void SceneMoveRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
