using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjMaxControler : MonoBehaviour
{
    [SerializeField, Header("CPU�\���p�X���C�_�[")] private Slider CPUSlider;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Image FillImage;

    [SerializeField, Header("CPU���l�\���p�e�L�X�g")] private Text CPUText;

    [SerializeField, Header("�^�X�N�}�l�[�W���[�\��")] private GameObject taskManagement;


    [SerializeField,Header("�o����I�u�W�F�N�g�̍ő吔")] private int objMax;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;

    // Update is called once per frame
    void Update()
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

        //CPU�̎g�p�������
        CPUSlider.value = (float)childObj / (float)objMax;
        CPUText.text = (((float)childObj / (float)objMax) * 100).ToString("N1") + "%";
    }


    public void CPUPanel()
    {
        taskManagement.SetActive(!taskManagement.activeSelf);
    }


}
