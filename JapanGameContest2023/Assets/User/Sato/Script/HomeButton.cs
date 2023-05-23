using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("�z�[���E�B���h�E")] private GameObject homeWindow;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    [SerializeField, Header("�V���b�g�_�E����̉摜")] private GameObject endImg;

    [SerializeField, Header("���[�h�̉摜")] private GameObject loadImg;

    [SerializeField, Header("UserNameText")] private Text userNameText;


    private void FixedUpdate()
    {
        //�V���b�g�_�E������Ƃ����[�h�摜����]����
        if(managerAccessor.Instance.dataMagager.sceneMoveStart)
        {
            loadImg.GetComponent<RectTransform>().eulerAngles -= new Vector3(0, 0, 2);
        }

        //HomeWindow���o�Ă��ď����摜���ɃJ�[�\��������Ƃ���bool�^���Ƃ�
        if(homeWindow.activeSelf)
        {
            RectTransform tra = homeWindow.GetComponent<RectTransform>();

            if (tra.position.x - (tra.sizeDelta.x / 2) < Input.mousePosition.x &&
                tra.position.x + (tra.sizeDelta.x / 2) > Input.mousePosition.x &&
                tra.position.y - (tra.sizeDelta.y / 2) < Input.mousePosition.y &&
                tra.position.y + (tra.sizeDelta.y / 2) > Input.mousePosition.y) 
            {
                managerAccessor.Instance.dataMagager.isOnHomeWindow = true;
            }
            else
            {
                managerAccessor.Instance.dataMagager.isOnHomeWindow = false;

                //�g���ɂ��Ȃ����N���b�N����Ə�����
                if (Input.GetMouseButton(0))
                {
                    RectTransform button = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
                    //�{�^���̏�ŃN���b�N�������ĕ\������Ȃ����߂̏���
                    if (!(button.position.x - (button.sizeDelta.x / 2) < Input.mousePosition.x &&
                        button.position.x + (button.sizeDelta.x / 2) > Input.mousePosition.x &&
                        button.position.y - (button.sizeDelta.y / 2) < Input.mousePosition.y &&
                        button.position.y + (button.sizeDelta.y / 2) > Input.mousePosition.y))
                    {
                        homeWindow.SetActive(false);
                        noTapArea.SetActive(false);
                    }
                }
            }
        }
        else
        {
            managerAccessor.Instance.dataMagager.isOnHomeWindow = false;
        }


        userNameText.text = PlayerPrefs.GetString("userName", "");
    }

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
        //���[�h�A�j���[�V�����Đ�
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        //�V���b�g�_�E���摜�\��
        endImg.SetActive(true);
        managerAccessor.Instance.dataMagager.isShutDown = true;
        yield return new WaitForSeconds(1.5f);

        //�Q�[���v���C�I��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
