using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField, Header("�u���X�N���o���Ƃ��̌���")] private Text putText;

    [SerializeField, Header("�u���X�N���o���Ƃ��̃q���g")] private Text hintText;


    [SerializeField, Header("�E�C���X�����������Ƃ�")] private string fallText;

    [SerializeField, Header("�E�C���X�����������Ƃ��̃q���g")] private string fallHintText;

    [SerializeField, Header("�E�C���X�����������Ƃ�")] private string infectionText;

    [SerializeField, Header("�E�C���X�����������Ƃ��q���g")] private string infectionHintText;

    [SerializeField, Header("�I�u�W�F�N�g�o�������̎�")] private string overText;

    [SerializeField, Header("�I�u�W�F�N�g�o�������̎��q���g")] private string overHintText;

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I�[�o�[���p�l�����o��
        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //���ꂼ��̎����Ńe�L�X�g��ς���
            if(managerAccessor.Instance.dataMagager.fallDeth)
            {
                putText.text = fallText;
                hintText.text = fallHintText;
            }
            else if(managerAccessor.Instance.dataMagager.infectionDeth)
            {
                putText.text = infectionText;
                hintText.text = infectionHintText;
            }
            else if(managerAccessor.Instance.dataMagager.overDeth)
            {
                putText.text = overText;
                hintText.text = overHintText;
            }
        }
    }
}
