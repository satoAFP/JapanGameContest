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

    //�R���[�`���Ăяo���p
    public void SceneMoveName(string name)
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine(CSceneMoveName(name));
    }

    //���O����Q�Ƃ��ăV�[���ړ�
    private IEnumerator CSceneMoveName(string name)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }

    //�R���[�`���Ăяo���p
    public void SceneMoveNext()
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine("CSceneMoveNext");
    }

    //���̃X�e�[�W�ɃV�[���ړ�
    private IEnumerator CSceneMoveNext()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            if (SceneManager.GetActiveScene().name == "Stage" + (i + 1))
            {
                if (SceneManager.GetActiveScene().name != "Stage" + managerAccessor.Instance.dataMagager.stageNum)
                {
                    SceneManager.LoadScene("Stage" + (i + 2));
                }
                else
                {
                    SceneManager.LoadScene("StageSelect");
                }
            }
        }
    }

    //�����[�h����
    public void SceneMoveRetry()
    {
        StartCoroutine("CSceneMoveRetry");
    }

    private IEnumerator CSceneMoveRetry()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�V�[���̖��O�擾�֐�
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


}
