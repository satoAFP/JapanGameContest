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



}
