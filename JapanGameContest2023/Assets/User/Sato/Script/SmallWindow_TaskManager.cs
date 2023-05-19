using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow_TaskManager : MonoBehaviour
{
    [SerializeField, Header("SmallWindow�����")] private GameObject smallWindow;
    [SerializeField, Header("NoTapArea�����")] private GameObject noTapArea;

    [SerializeField, Header("CPU�\���p�X���C�_�[")] private Slider CPUSlider;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Image FillImage;

    [SerializeField, Header("CPU���l�\���p�e�L�X�g")] private Text CPUText;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;


    private int blockChildObj = 0;  //��������u���b�N�̐��i�[

    private int objMax = 0;         //�V�[�����ɏo����u���b�N�̍ő吔

    private bool isOnTab = false;   //�J�[�\�����^�u�̏�ɏ���Ă���Ƃ�

    //�ŏ������ʂ�Ȃ�
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        //��������u���b�N�����擾
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        objMax = managerAccessor.Instance.dataMagager.objMax;

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
                //�J�[�\��������Ă���Ƃ��o��
                smallWindow.SetActive(true);
                noTapArea.SetActive(true);

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
            if (!(npos.x - (nsize.x / 2) < mouse.x && npos.x + (nsize.x / 2) > mouse.x &&
                npos.y - (nsize.y / 2) < mouse.y && npos.y + (nsize.y / 2) > mouse.y && isOnTab))
            {
                smallWindow.SetActive(false);
                noTapArea.SetActive(false);
                first = true;
                isOnTab = false;
            }
        }

        //�ő吔�����̎�
        if (blockChildObj < objMax)
        {
            FillImage.color = color[0];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //�ő吔�̎�
        else if (blockChildObj == objMax)
        {
            FillImage.color = color[1];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //�ő吔�𒴂�����
        else if (blockChildObj > objMax)
        {
            FillImage.color = color[2];
            managerAccessor.Instance.dataMagager.objMaxFrag = true;
        }
        Debug.Log((float)blockChildObj / (float)objMax);

        //CPU�̎g�p�������
        CPUSlider.value = (float)blockChildObj / (float)objMax;
        CPUText.text = (((float)blockChildObj / (float)objMax) * 100).ToString("N1") + "%";

    }
}
