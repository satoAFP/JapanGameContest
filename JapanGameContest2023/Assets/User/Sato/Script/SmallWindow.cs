using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow : MonoBehaviour
{
    [SerializeField, Header("SmallWindow�����")] private GameObject smallWindow;
    [SerializeField, Header("�X�N�V�������摜���i�[����I�u�W�F�N�g")] private Image gameImg;


    private bool isScreenShot = false;//�X�N���[���V���b�g���������ǂ���

    //�ŏ������ʂ�Ȃ�
    private bool first = true;

    private void Start()
    {
        

        Debug.Log(transform.parent.name);
    }

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
                    //��ʂ̃X�N���[���V���b�g
                    CaptureScreenShot("Assets/Resources/" + transform.parent.name + ".png");
                    isScreenShot = true;
                }
                //�X�N�V���̃^�C�����O��҂�
                StartCoroutine("ScreenShotWait");


                first = false;
            }

        }
        else
        {
            smallWindow.SetActive(false);
            first = true;
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
            ImgPaste("Assets/Resources/" + transform.parent.name + ".png");
        }

        //SmallWindow�\��
        smallWindow.SetActive(true);
    }
}
