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
        if (managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Title")
        {
            PlayerPrefs.SetString("userName", managerAccessor.Instance.dataMagager.userName.text);
        }

        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine(CSceneMoveName(name));
    }

    //���O����Q�Ƃ��ăV�[���ړ�
    private IEnumerator CSceneMoveName(string name)
    {
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
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
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);

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
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine("CSceneMoveRetry");
    }

    private IEnumerator CSceneMoveRetry()
    {
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�V�[���̖��O�擾�֐�
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    //�X�e�[�W�N���A�f�[�^���Z�b�g
    public void ResetData()
    {
        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++)
        {
            PlayerPrefs.DeleteKey("Stage" + i);
        }
        managerAccessor.Instance.sceneMoveManager.SceneMoveName("Title");
    }
}
