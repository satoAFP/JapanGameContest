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

    [SerializeField, Header("�e�L�X�g�\���p�I�u�W�F�N�g")] private GameObject textObj;

    [SerializeField, Header("�e�L�X�g�\���p�I�u�W�F�N�g(NoTapArea)")] private GameObject textNoTapArea;

    private List<GameObject> stages = new List<GameObject>();   //�X�e�[�W�L���p
    private int frameCount = 0;                                 //�_�u���N���b�N�̊Ԋu���J�E���g
    private bool oneClick = false;                            �@//���ڃN���b�N���ꂽ����
    private bool doubleClick = false;                           //���ڃN���b�N���ꂽ����
    private int stageNumber = 999;                              //�X�e�[�W�̔ԍ�
    private int firstStageNumber = 999;                         //�ŏ��N���b�N�����Ƃ��̃X�e�[�W�ԍ�
    private int secondStageNumber = 999;                        //���ڃN���b�N�����Ƃ��̃X�e�[�W�ԍ�
    private GameObject textClone = null;                        //README�\���p
    private bool isText = false;                                //�e�L�X�g��I�����Ă��邩

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

            //�e�L�X�g�t�@�C���̕������̐e�A���O�A�摜�̕ύX
            textClone = Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj);
            textClone.transform.parent = stageParent.transform;
            textClone.transform.GetChild(0).GetComponent<Text>().text = "README.txt";

            //�X�e�[�W�̐���
            for (int i = 0; i < stage; i++)
            {
                //�N���A�󋵃f�[�^���擾
                stageClear = PlayerPrefs.GetInt("Stage" + i, 0);

                //�������̐e�A���O�A�摜�̕ύX
                stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
                stages[i].transform.parent = stageParent.transform;
                stages[i].transform.GetChild(0).GetComponent<Text>().text = "Stage" + (i + 1) + ".exe";
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


        //HomeWindow���ɃJ�[�\���������Ƃ�
        if (!managerAccessor.Instance.dataMagager.noTapArea)
        {
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

            //�J�[�\�������킹���Ƃ��p�l�����o��
            textClone.transform.GetChild(2).gameObject.SetActive(false);

            //�}�E�X���e�L�X�g�t�@�C���̍��W���ɂ���Ƃ�
            if (textClone.GetComponent<RectTransform>().position.x - textClone.GetComponent<RectTransform>().sizeDelta.x + 60 < Input.mousePosition.x &&
                textClone.GetComponent<RectTransform>().position.x + textClone.GetComponent<RectTransform>().sizeDelta.x - 60 > Input.mousePosition.x &&
                textClone.GetComponent<RectTransform>().position.y - textClone.GetComponent<RectTransform>().sizeDelta.y + 40 < Input.mousePosition.y &&
                textClone.GetComponent<RectTransform>().position.y + textClone.GetComponent<RectTransform>().sizeDelta.y - 60 > Input.mousePosition.y)
            {
                textClone.transform.GetChild(2).gameObject.SetActive(true);
            }


            if (!managerAccessor.Instance.dataMagager.sceneMoveStart)
            {
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
                        textClone.transform.GetChild(1).gameObject.SetActive(false);


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
                                isText = false;
                                break;
                            }
                            else
                            {
                                stageNumber = 999;
                                secondStageNumber = 999;
                            }
                        }

                        //���N���b�N���ꂽ��&&���ڂƓ��ڂ̃X�e�[�W�ԍ��������Ƃ�&&�N���b�N�����ԍ������݂��Ă���Ƃ�
                        if (oneClick && firstStageNumber == secondStageNumber && secondStageNumber != 999 || isText)
                        {
                            doubleClick = true;
                        }


                        //�}�E�X�����W���ɂ���Ƃ�
                        if (textClone.GetComponent<RectTransform>().position.x - textClone.GetComponent<RectTransform>().sizeDelta.x + 60 < Input.mousePosition.x &&
                            textClone.GetComponent<RectTransform>().position.x + textClone.GetComponent<RectTransform>().sizeDelta.x - 60 > Input.mousePosition.x &&
                            textClone.GetComponent<RectTransform>().position.y - textClone.GetComponent<RectTransform>().sizeDelta.y + 40 < Input.mousePosition.y &&
                            textClone.GetComponent<RectTransform>().position.y + textClone.GetComponent<RectTransform>().sizeDelta.y - 60 > Input.mousePosition.y)
                        {
                            //�I�������A�C�R����I����Ԃɂ���
                            textClone.transform.GetChild(1).gameObject.SetActive(true);
                            isText = true;
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
            }
        }

        //�_�u���N���b�N�̔��������܂ł̎��Ԍv������
        if (oneClick)
        {
            if (clickFrameRate == frameCount) 
            {
                oneClick = false;
                isText = false;
                frameCount = 0;
            }
            frameCount++;
        }

        //�_�u���N���b�N�ɐ��������Ƃ�
        if (doubleClick)
        {
            //�e�L�X�g��I�����Ă��Ȃ��Ƃ�
            if (!isText)
            {
                if (stageNumber != 999)
                {
                    //�X�e�[�W�ړ�
                    managerAccessor.Instance.sceneMoveManager.SceneMoveName("Stage" + stageNumber);
                }
            }
            else
            {
                //�e�L�X�g�\��
                StartCoroutine("LoadAni");
                isText = false;
                doubleClick = false;
            }
        }
    }



    public void EndText()
    {
        textObj.SetActive(false);
        textNoTapArea.SetActive(false);
        isText = false;
    }

    //�e�L�X�g�̏o���A�j���[�V����
    private IEnumerator LoadAni()
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        textObj.SetActive(true);
        textNoTapArea.SetActive(true);
        textClone.transform.GetChild(1).gameObject.SetActive(false);
        managerAccessor.Instance.dataMagager.sceneMoveStart = false;
    }

}
