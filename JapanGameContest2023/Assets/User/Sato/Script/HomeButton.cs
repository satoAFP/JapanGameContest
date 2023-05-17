using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("�z�[���E�B���h�E")] private GameObject homeWindow;

    //�V���b�g�_�E���Ȃǂ�\����\��������֐�
    public void WindowButton()
    {
        homeWindow.SetActive(!homeWindow.activeSelf);
    }

    //�Q�[�����I��������֐�
    public void ShutdownButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
