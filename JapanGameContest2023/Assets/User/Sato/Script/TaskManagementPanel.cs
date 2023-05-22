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

    [SerializeField, Header("�����Ă���E�C���X�̐��\���p�e�L�X�g")] private Text aliveVirusText;

    [SerializeField, Header("����ł���E�C���X�̐��\���p�e�L�X�g")] private Text dethVirusText;

    [SerializeField, Header("�u���b�N���\���p�e�L�X�g")] private Text blockText;

    [SerializeField, Header("�S�[�����\���p�e�L�X�g")] private Text goalText;

    [SerializeField, Header("�^�X�N�}�l�[�W���[�\��")] private GameObject taskManagement;
    [SerializeField, Header("NoTapArea�\��")] private GameObject notapArea;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;

    [SerializeField, Header("�t�@����SE")] private AudioSource funSE;
    [SerializeField, Header("�t�@����SE�t�F�[�h����")] private float moveFunSE;

    //��������u���b�N�̐��i�[
    private int blockChildObj = 0;

    //�V�[�����ɏo����u���b�N�̍ő吔
    private int objMax = 0;

    //�S�[���̐��i�[
    private int goalChildObj = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //��������u���b�N�����擾
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        objMax = managerAccessor.Instance.dataMagager.objMax;
        goalChildObj = managerAccessor.Instance.objDataManager.goalParent.transform.childCount;

        //�ő吔�����̎�
        if (blockChildObj < objMax)
        {
            FillImage.color = color[0];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;

            //�t�@���̉��ݒ�
            if (funSE.pitch > 0)
            {
                funSE.pitch -= moveFunSE;
            }
        }
        //�ő吔�̎�
        else if (blockChildObj == objMax) 
        {
            FillImage.color = color[1];
            managerAccessor.Instance.dataMagager.objMaxFrag = false;

            //�t�@���̉��ݒ�
            if (funSE.pitch > 1)
            {
                funSE.pitch -= moveFunSE;
            }
            else if(funSE.pitch < 1)
            {
                funSE.pitch += moveFunSE;
            }
        }
        //�ő吔�𒴂�����
        else if (blockChildObj > objMax)
        {
            FillImage.color = color[2];
            managerAccessor.Instance.dataMagager.objMaxFrag = true;

            //�t�@���̉��ݒ�
            if (funSE.pitch < 2)
            {
                funSE.pitch += moveFunSE;
            }
        }


        //CPU�̎g�p�������
        CPUSlider.value = (float)blockChildObj / (float)objMax;
        CPUText.text = (((float)blockChildObj / (float)objMax) * 100).ToString("N1") + "%";
        //�o�ߎ��ԕ\��
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
        //���݂̐����Ă���E�C���X�̐��\��
        aliveVirusText.text = (goalChildObj - managerAccessor.Instance.dataMagager.goalPlayerNum).ToString();
        //���݂̎���ł���E�C���X�̐��\��
        dethVirusText.text = managerAccessor.Instance.dataMagager.goalPlayerNum.ToString();
        //���݂�block�̐��\��
        blockText.text = blockChildObj.ToString();
        //���݂̃S�[���̐��\��
        goalText.text = goalChildObj.ToString();
    }

    //CPU�p�l���o��
    public void CPUPanelOn()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
        notapArea.SetActive(!notapArea.activeSelf);
    }

    //CPU�p�l���o��
    public void CPUPanelOff()
    {
        taskManagement.SetActive(false);
        notapArea.SetActive(false);
    }

}
