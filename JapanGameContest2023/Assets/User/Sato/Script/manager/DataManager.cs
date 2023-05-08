using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //�I�u�W�F�N�g���ő吔�ɂȂ������ς��t���O
    [System.NonSerialized] public bool objMaxFrag = false;

    //�I�u�W�F�N�g��I���������A���ɏ��ƕς��
    [System.NonSerialized] public bool onEdge = false;

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

    //��l���̔s�k�t���O�iON�ŃQ�[���I�[�o�[�j
    [System.NonSerialized] public bool loseflag = false;

    //��l���̈ړ��t���O
    [System.NonSerialized] public bool isMoving = false;


    [Header("�S�X�e�[�W��")] public int stageNum;

    private GameObject clonePanel = null;

    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        managerAccessor.Instance.dataMagager = this;
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
        playMode = !playMode;

        if(managerAccessor.Instance.dataMagager.objMaxFrag)
        {
            managerAccessor.Instance.dataMagager.playerlost = true;
        }

        //�p�l���̕�������э폜
        if(playMode)
        {
            Destroy(clonePanel);
        }
        else
        {
            clonePanel = Instantiate(managerAccessor.Instance.objDataManager.editPanel);
            clonePanel.transform.position = new Vector3(0, 0, 0);
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
                //���ɑI�����ꂽ��Ԃɂ��Ă���
                dataManager.selectObjsData.Add(clone);
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
