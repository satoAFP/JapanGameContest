using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("�t�@�C���̃N���A���̉摜")] private Sprite clearFile;

    [SerializeField, Header("�t�@�C���̃N���A����Ă��Ȃ����̉摜")] private Sprite noClearFile;

    [SerializeField, Header("�X�e�[�W�z�u���̐e�I�u�W�F�N�g")] private GameObject stageParent;

    [SerializeField, Header("�_�u���N���b�N����Ԋu����")] private int clickFrameRate;

    private List<GameObject> stages = new List<GameObject>();   //�X�e�[�W�L���p
    private int frameCount = 0;                                 //�_�u���N���b�N�̊Ԋu���J�E���g
    private bool oneClick = false;                            �@//���ڃN���b�N���ꂽ����
    private bool doubleClick = false;                           //���ڃN���b�N���ꂽ����
    private int stageNumber = 999;                              //�X�e�[�W�̔ԍ�
    private int firstStageNumber = 999;                         //�ŏ��N���b�N�����Ƃ��̃X�e�[�W�ԍ�
    private int secondStageNumber = 999;                        //�ŏ��N���b�N�����Ƃ��̃X�e�[�W�ԍ�

    //�ŏ��̈�񂾂�����
    private bool first1 = true;
    private bool clonefirst = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //�ŏ��̐���
        if(clonefirst)
        {
            //�X�e�[�W���擾
            int stage = managerAccessor.Instance.dataMagager.stageNum;

            //�X�e�[�W�̃N���A�󋵃f�[�^�擾�p
            int stageClear = 0;

            //�X�e�[�W�̐���
            for (int i = 0; i < stage; i++)
            {
                //�N���A�󋵃f�[�^���擾
                stageClear = PlayerPrefs.GetInt("Stage" + i, 0);

                //�������̐e�A���O�A�摜�̕ύX
                stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
                stages[i].transform.parent = stageParent.transform;
                stages[i].transform.GetChild(0).GetComponent<Text>().text = "STAGE" + (i + 1);
                if (stageClear == 1) 
                {
                    //�N���A����Ă�����
                    stages[i].GetComponent<Image>().sprite = clearFile;
                }
                else
                {
                    //�N���A����Ă��Ȃ�������
                    stages[i].GetComponent<Image>().sprite = noClearFile;
                }
            }
            clonefirst = false;
        }

        //�J�[�\�������킹���Ƃ��p�l�����o��
        for (int i = 0; i < stages.Count; i++)
        {
            RectTransform stageNum = stages[i].GetComponent<RectTransform>();
            stageNum.transform.GetChild(2).gameObject.SetActive(false);

            //�}�E�X�����W���ɂ���Ƃ�
            if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
            {
                stageNum.transform.GetChild(2).gameObject.SetActive(true);
            }
        }

        //�_�u���N���b�N
        if (Input.GetMouseButton(0))
        {
            //�������͔������Ȃ�
            if (first1)
            {
                for (int i = 0; i < stages.Count; i++)
                {
                    //��U�I����ԉ���
                    stages[i].GetComponent<RectTransform>().transform.GetChild(1).gameObject.SetActive(false);
                }

                for (int i = 0; i < stages.Count; i++)
                {
                    RectTransform stageNum = stages[i].GetComponent<RectTransform>();

                    //�}�E�X�����W���ɂ���Ƃ�
                    if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                        stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                        stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                        stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
                    {
                        //�N���b�N�����X�e�[�W�L��
                        stageNumber = i + 1;
                        secondStageNumber = i + 1;
                        //�I�������A�C�R����I����Ԃɂ���
                        stageNum.transform.GetChild(1).gameObject.SetActive(true);
                        break;
                    }
                    else
                    {
                        stageNumber = 999;
                        secondStageNumber = 999;
                    }
                }

                //���N���b�N���ꂽ��&&���ڂƓ��ڂ̃X�e�[�W�ԍ��������Ƃ�&&�N���b�N�����ԍ������݂��Ă���Ƃ�
                if (oneClick && firstStageNumber == secondStageNumber && secondStageNumber != 999)  
                {
                    doubleClick = true;
                }

                oneClick = true;
                first1 = false;
                firstStageNumber = secondStageNumber;
            }
        }
        else
        {
            first1 = true;
        }

        //�_�u���N���b�N�̔��������܂ł̎��Ԍv������
        if (oneClick)
        {
            if (clickFrameRate == frameCount) 
            {
                oneClick = false;
                frameCount = 0;
            }
            frameCount++;
        }

        //�_�u���N���b�N�ɐ��������Ƃ�
        if (doubleClick)
        {
            if (stageNumber != 999)
            {
                //�X�e�[�W�ړ�
                managerAccessor.Instance.sceneMoveManager.SceneMoveName("Stage" + stageNumber);
            }
        }
    }
}
