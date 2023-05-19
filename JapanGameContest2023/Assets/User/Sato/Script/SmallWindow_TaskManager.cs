using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow_TaskManager : MonoBehaviour
{
    [SerializeField, Header("CPU�\���p�X���C�_�[")] private Slider CPUSlider;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Image FillImage;

    [SerializeField, Header("CPU���l�\���p�e�L�X�g")] private Text CPUText;

    [SerializeField, Header("�X���C�_�[�̐F�ύX�p")] private Color[] color;


    private int blockChildObj = 0;  //��������u���b�N�̐��i�[

    private int objMax = 0;         //�V�[�����ɏo����u���b�N�̍ő吔

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��������u���b�N�����擾
        blockChildObj = managerAccessor.Instance.objDataManager.blockParent.transform.childCount;
        objMax = managerAccessor.Instance.dataMagager.objMax;

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

    }
}
