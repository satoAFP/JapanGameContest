using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //�I�����ꂽ�I�u�W�F�N�g
    [System.NonSerialized] public List<GameObject> selectObjsData = new List<GameObject>();
    //�R�s�[�p�I�����ꂽ�I�u�W�F�N�g
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();

    //�R�s�[�p�E�N���b�N�������Ă΂��UI
    [System.NonSerialized] public GameObject rightClickUIClone = null;

    //�R�s�[��I�����������f�p
    [System.NonSerialized] public bool objsCopy = false;
    //�R�s�[�f�[�^�̍폜���ŏ��̈�񂵂��s��Ȃ����߂̃t���O
    [System.NonSerialized] public bool copyReset = true;

    //���ԕ\���p�e�L�X�g
    [System.NonSerialized] public string timeText = null;

    //�I�u�W�F�N�g���ő吔�ɂȂ������ς��t���O
    [System.NonSerialized] public bool objMaxFrag = false;

    //�I�u�W�F�N�g��I���������A���ɏ��ƕς��
    [System.NonSerialized] public bool onEdge = false;

    //����Ă͂����Ȃ��u���b�N�ɃI�u�W�F�N�g������Ƃ����[�h�`�F���W�ł��Ȃ�
    [System.NonSerialized] public bool onBlock = false;

    //�ǂ̉��ɏ���Ă��邩(8�͉��������Ă��Ȃ��f�[�^)
    [System.NonSerialized] public int whereEdge = 8;

    //NoTapArea�ɃJ�[�\��������Ƃ��؂�ւ��
    [System.NonSerialized] public bool noTapArea = false;

    //��l���������邩�ҏW���[�h�ɓ��邩�؂�ւ��p�t���O
    [System.NonSerialized] public bool playMode = true;

    //��l���������邩�ҏW���[�h�ɓ��邩�؂�ւ��p�t���O
    [System.NonSerialized] public int objNum = 0;

    //��l���������������̃t���O
    [System.NonSerialized] public bool playerlost = false;

    //�}�E�X���N���b�N�������W���擾
    [System.NonSerialized] public Vector3 clickPosition;

    //�v���C���[�̐��𐔂���i�e�v���C���[�̈ړ������Ă��邩���m�F���邽�߂Ɏg���j
    [System.NonSerialized] public int playercount = 0;

    //��l���̈ړ��t���O
    [System.NonSerialized] public bool isMoving = false;

    //��l����Decoy�t�@�C���ɐG�ꂽ�Ƃ�
    [System.NonSerialized] public bool onDecoyFile = false;

    //�S�[���ɓ�������l���̐�
    [System.NonSerialized] public int goalPlayerNum = 0;


    //�V�[���؂�ւ��J�n
    [System.NonSerialized] public bool sceneMoveStart = false;


    //�����������Ƃ�
    [System.NonSerialized] public bool fallDeth = false;
    //�E�C���X�Ɋ��������Ƃ�
    [System.NonSerialized] public bool infectionDeth = false;
    //CPU�g�p�ʂ𒴂����Ƃ�
    [System.NonSerialized] public bool overDeth = false;



    [Header("�S�X�e�[�W��")] public int stageNum;


    private GameObject clonePanel = null;


    //���ԕ\���p
    private int frame = 0;
    private int second = 0;
    private int minute = 0;

    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        managerAccessor.Instance.dataMagager = this;
    }

    private void FixedUpdate()
    {
        managerAccessor.Instance.dataMagager.TimeCount();
    }


    //���ԕ\���p
    public void TimeCount()
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

        managerAccessor.Instance.dataMagager.timeText = minute.ToString("d2") + " : " + second.ToString("d2");
    }


    //�}�E�X���W�����[���h���W�ϊ��֐�
    public Vector3 MouseWorldChange()
    {
        //�I���J�n���̏����ʒu�L��
        Vector3 mousePos = Input.mousePosition;
        // Z���C��
        mousePos.z = 10f;
        // �}�E�X�ʒu���W���X�N���[�����W���烏�[���h���W�ɕϊ�����
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return screenToWorldPointPosition;
    }

    //�v���C���[�h�ؑ֊֐�
    public void ModeChange()
    {
        if (!managerAccessor.Instance.dataMagager.isMoving &&
            !managerAccessor.Instance.dataMagager.onBlock) 
        {
            //�v���C���[�h�ؑ֊֐�
            playMode = !playMode;

            //�I�u�W�F�N�g�̐������E�𒴂��Ă������؂�ւ���ƕ���
            if (managerAccessor.Instance.dataMagager.objMaxFrag)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
                managerAccessor.Instance.dataMagager.overDeth = true;
            }

            //�p�l���̕�������э폜
            if (playMode)
            {
                Destroy(clonePanel);

                //���[�h�`�F���W�̉摜�ύX
                managerAccessor.Instance.objDataManager.modeChangeObj.GetComponent<Image>().sprite = managerAccessor.Instance.objDataManager.playModeImg;
            }
            else
            {
                clonePanel = Instantiate(managerAccessor.Instance.objDataManager.editPanel);
                clonePanel.transform.position = new Vector3(0, 0, 0);

                //���[�h�`�F���W�̉摜�ύX
                managerAccessor.Instance.objDataManager.modeChangeObj.GetComponent<Image>().sprite = managerAccessor.Instance.objDataManager.editModeImg;
            }
        }
    }

    //�R�s�[�{�^���p�֐�
    public void CopyButton()
    {
        if (managerAccessor.Instance.dataMagager.copyObjsData.Count != 0)
        {
            //�R�s�[��������Ă��锻��
            managerAccessor.Instance.dataMagager.objsCopy = true;

            //�{�^���������ꂽ��UI��������
            Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);

        }
    }

    //�y�[�X�g�{�^���p�֐�
    public void PasteButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //�R�s�[��������Ă���Ƃ�
        if (managerAccessor.Instance.dataMagager.objsCopy)
        {

            //�}�E�X�ƌ��������I�u�W�F�N�g�Ƃ̈ړ��ʌv�Z
            Vector3 moveAmount = MouseWorldChange() - dataManager.copyObjsData[0].transform.localPosition;

            //�ȑO�I������Ă����I�u�W�F�N�g�f�[�^�폜
            dataManager.selectObjsData.Clear();

            //���̏ꏊ�ɕ\��
            for (int i = 0; i < dataManager.copyObjsData.Count; i++)
            {
                GameObject clone = Instantiate(dataManager.copyObjsData[i]);
                clone.transform.localPosition += moveAmount;
                clone.transform.parent = managerAccessor.Instance.objDataManager.blockParent.transform;
            }


            //�{�^���������ꂽ��UI��������
            Destroy(dataManager.rightClickUIClone);
        }
    }

    //�폜�{�^���p�֐�
    public void DeleteButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        //��������Ă���I�u�W�F�N�g�폜
        for (int i = 0; i < dataManager.copyObjsData.Count; i++)
        {
            Destroy(dataManager.selectObjsData[i]);
        }

        //�ȑO�I������Ă����I�u�W�F�N�g�f�[�^�폜
        dataManager.selectObjsData.Clear();

        //�{�^���������ꂽ��UI��������
        Destroy(dataManager.rightClickUIClone);

    }

    
}
