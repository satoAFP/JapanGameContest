using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManagementPanel : MonoBehaviour
{
    [SerializeField, Header("CPU�\���p�X���C�_�[")] private Slider CPUSlider;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Image FillImage;

    [SerializeField, Header("CPU���l�\���p�e�L�X�g")] private Text CPUText;

    [SerializeField, Header("�^�C���\���p�e�L�X�g")] private Text timeText;

    [SerializeField, Header("�u���b�N���\���p�e�L�X�g")] private Text blockText;

    [SerializeField, Header("�^�X�N�}�l�[�W���[�\��")] private GameObject taskManagement;


    [SerializeField,Header("�o����I�u�W�F�N�g�̍ő吔")] private int objMax;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;


    //���ԕ\���p
    private int frame = 0;
    private int second = 0;
    private int minute = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //��������u���b�N�����擾
        int childObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;


        //�ő吔�����̎�
        if(childObj < objMax)
        {
            FillImage.color = color[0];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //�ő吔�̎�
        else if (childObj == objMax) 
        {
            FillImage.color = color[1];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }
        //�ő吔�𒴂�����
        else if (childObj > objMax)
        {
            FillImage.color = color[2];
            managerAccessor.Instance.dataMagager.objMaxFrag = true;
        }

        //���Ԍv�Z
        TimeCount();

        //CPU�̎g�p�������
        CPUSlider.value = (float)childObj / (float)objMax;
        CPUText.text = (((float)childObj / (float)objMax) * 100).ToString("N1") + "%";
        //�o�ߎ��ԕ\��
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
        //���݂�block�̐��\��
        blockText.text = childObj.ToString();
    }

    //CPU�p�l��
    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }

    //���ԕ\���p
    private void TimeCount()
    {
        frame++;

        if (frame >= 50) 
        {
            second++;
            frame = 0;
        }

        if (second >= 60) 
        {
            minute++;
            second = 0;
        }

    }

}
