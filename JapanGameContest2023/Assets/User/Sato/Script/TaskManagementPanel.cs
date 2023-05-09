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

    [SerializeField, Header("�S�[�����\���p�e�L�X�g")] private Text goalText;

    [SerializeField, Header("�^�X�N�}�l�[�W���[�\��")] private GameObject taskManagement;


    [SerializeField,Header("�o����I�u�W�F�N�g�̍ő吔")] private int objMax;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;

    //��������u���b�N�̐��i�[
    private int blockChildObj = 0;

    //�S�[���̐��i�[
    private int goalChildObj = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //��������u���b�N�����擾
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        goalChildObj = managerAccessor.Instance.objDataManager.goalParent.transform.childCount;

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


        //CPU�̎g�p�������
        CPUSlider.value = (float)blockChildObj / (float)objMax;
        CPUText.text = (((float)blockChildObj / (float)objMax) * 100).ToString("N1") + "%";
        //�o�ߎ��ԕ\��
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
        //���݂�block�̐��\��
        blockText.text = blockChildObj.ToString();
        //���݂̃S�[���̐��\��
        goalText.text = goalChildObj.ToString();
    }

    //CPU�p�l��
    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }

}
