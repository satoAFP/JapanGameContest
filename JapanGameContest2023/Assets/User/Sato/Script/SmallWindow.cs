using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow : MonoBehaviour
{
    [SerializeField, Header("SmallWindow�����")] private GameObject smallWindow;
    [SerializeField, Header("NoTapArea�����")] private GameObject noTapArea;
    [SerializeField, Header("StageName�����")] private Text stageName;
    [SerializeField, Header("�X�N�V�������摜���i�[����I�u�W�F�N�g")] private Image gameImg;


    private bool isScreenShot = false;//�X�N���[���V���b�g���������ǂ���
    private bool isOnTab = false;//�J�[�\�����^�u�̏�ɏ���Ă���Ƃ�

    //�ŏ������ʂ�Ȃ�
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //�K�v�ȏ��̎擾
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;

        //�}�E�X�����W���ɂ���Ƃ�
        if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
        {
            if (first)
            {
                //���g�̐e�̐e�̃I�u�W�F�N�g�Ɋi�[����Ă���X�e�[�W��SetActive��true�̎��X�N�V�����o����
                if (transform.parent.parent.GetComponent<PageChangeArea>().stage[transform.parent.GetComponent<TabButton>().number].activeSelf)
                {
#if UNITY_EDITOR
                    CaptureScreenShot("Assets/Resources/" + transform.parent.name + ".png");
#else
                    CaptureScreenShot("Resources/" + transform.parent.name + ".png");
#endif

                    //��ʂ̃X�N���[���V���b�g
                    isScreenShot = true;
                }
                //�X�N�V���̃^�C�����O��҂�
                StartCoroutine("ScreenShotWait");

                //SmallWindow�̃^�u�̖��O��ς���
                stageName.text = managerAccessor.Instance.sceneMoveManager.GetSceneName() + "-" + (transform.parent.GetComponent<TabButton>().number + 1);

                //�J�[�\�����^�u�ɏ���Ă���Ƃ�
                isOnTab = true;
                first = false;
            }

        }
        else
        {
            //�K�v�ȏ��̎擾
            Vector2 npos = noTapArea.GetComponent<RectTransform>().position;
            Vector2 nsize = noTapArea.GetComponent<RectTransform>().sizeDelta;

            //�^�u�ɃJ�[�\��������Ă����Ƃ�&&NoTapArea�ɃJ�[�\��������Ă邢��Ƃ�
            if (npos.x - (nsize.x / 2) < mouse.x && npos.x + (nsize.x / 2) > mouse.x &&
                npos.y - (nsize.y / 2) < mouse.y && npos.y + (nsize.y / 2) > mouse.y && isOnTab) 
            {
                //�{�^�������������삵����
                if(transform.parent.GetComponent<TabButton>().isPutButton)
                {
                    //SmallWindow������
                    transform.parent.GetComponent<TabButton>().isPutButton = false;
                    noTapArea.SetActive(false);
                    smallWindow.SetActive(false);
                    first = true;
                    isOnTab = false;
                }
            }
            else
            {
                smallWindow.SetActive(false);
                noTapArea.SetActive(false);
                first = true;
                isOnTab = false;
            }
        }
    }



    //��ʑS�̂̃X�N���[���V���b�g��ۑ�����
    //����1 �ۑ�����K�w�Ɩ��O
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath);
    }

    //�t�@�C���̉摜���Q�Ƃ��A�I�u�W�F�N�g�ɓ\��t����֐�
    //����1 �Q�Ƃ���K�w�Ɖ摜�̖��O
    private void ImgPaste(string path)
    {
        //�摜�̕\������
        byte[] data = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data);
        gameImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    //�X�N���[���V���b�g�̏�������������҂��߂̊֐�
    private IEnumerator ScreenShotWait()
    {
        yield return new WaitForSeconds(0.1f);

        if (isScreenShot)
        {
#if UNITY_EDITOR
            ImgPaste("Assets/Resources/" + transform.parent.name + ".png");
#else
            ImgPaste("VIRUS PURGE_Data/Resources/" + transform.parent.name + ".png");
#endif
        }

        //SmallWindow�\��
        smallWindow.SetActive(true);
        noTapArea.SetActive(true);
    }
}
