using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("�z�[���E�B���h�E")] private GameObject homeWindow;

    [SerializeField, Header("�z�[���E�B���h�E")] private GameObject noTapArea;

    [SerializeField, Header("�V���b�g�_�E����̉摜")] private GameObject endImg;

    //�V���b�g�_�E���Ȃǂ�\����\��������֐�
    public void WindowButton()
    {
        homeWindow.SetActive(!homeWindow.activeSelf);
        noTapArea.SetActive(!noTapArea.activeSelf);
    }

    //�Q�[�����I��������֐�
    public void ShutdownButton()
    {
        StartCoroutine("CShutDown");
    }

    private IEnumerator CShutDown()
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        endImg.SetActive(true);
        yield return new WaitForSeconds(3.0f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
