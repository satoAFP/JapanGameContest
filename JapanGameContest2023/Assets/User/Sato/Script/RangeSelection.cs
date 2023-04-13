using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    [SerializeField, Header("�h�b�g�`�掞�̊Ԋu")] private float wide;

    
    //�ŏ��̈�񂵂��ʂ�Ȃ������p
    private bool first = true;
    private bool first2 = true;

    private Vector3 clickStartPos;                      //�N���b�N�J�n���̏����ʒu
    private GameObject clone;                           //�I��͈͕\���p�I�u�W�F�N�g
    private bool selectionMode = false;                 //�I�u�W�F�N�g��I�����Ă��邩�ǂ���
    private bool editMode = false;                      //�I�u�W�F�N�g��I�����Ă��邩�ǂ���
    private Vector3 beforePos = new Vector3(0, 0, 0);   //��t���[���O�̃}�E�X�̈ʒu

    private List<GameObject> cloneDot = new List<GameObject>();//�h�b�g�i�[�p
    private Vector2 startPos;                           //�N���b�N�����Ƃ��̏����ʒu
    private Vector2 square;                             //�l�p�̏c���̒���
    private int dotNum;                                 //�h�b�g�̐��i�[�p


    // Update is called once per frame
    void FixedUpdate()
    {
        //�L�����𑀍쒆�͑I���ł��Ȃ�
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //�I������Ă�I�u�W�F�N�g���i�[�����
            List<GameObject> Objs = managerAccessor.Instance.dataMagager.selectObjsData;

            //�I�u�W�F�N�g���I������Ă��鎞
            if (!selectionMode)
            {
                for (int i = 0; i < Objs.Count; i++)
                {
                    //�}�E�X���W�����[���h���W�ɕϊ�
                    Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    //�I������Ă���I�u�W�F�N�g���ɃJ�[�\��������ꍇ
                    if (Objs[i].transform.localPosition.x - Objs[i].transform.localScale.x / 2 < nowMousePos.x &&
                        Objs[i].transform.localPosition.x + Objs[i].transform.localScale.x / 2 > nowMousePos.x &&
                        Objs[i].transform.localPosition.y - Objs[i].transform.localScale.x / 2 < nowMousePos.y &&
                        Objs[i].transform.localPosition.y + Objs[i].transform.localScale.x / 2 > nowMousePos.y)
                    {
                        //�I�u�W�F�N�g�I����Ԃɂ���
                        editMode = true;
                        break;
                    }
                    else
                    {
                        //�}�E�X���N���b�N����Ă����Ԃ̏ꍇ�A�I����Ԃ��������Ȃ�
                        if (!Input.GetMouseButton(0))
                            editMode = false;
                    }
                }
            }

            //�I�u�W�F�N�g���I������Ă��鎞
            if (editMode)
            {
                if (Input.GetMouseButton(0))
                {
                    //1�t���[���O�Ƃ̌덷���Z�o
                    Vector3 movePower = managerAccessor.Instance.dataMagager.MouseWorldChange() - beforePos;

                    //�I������Ă���I�u�W�F�N�g�ɉ��Z
                    for (int i = 0; i < Objs.Count; i++)
                        Objs[i].transform.localPosition += movePower;

                    //�h�b�g�̘g�ɂ��ړ��ʉ��Z
                    for (int i = 0; i < cloneDot.Count; i++) 
                        cloneDot[i].transform.localPosition += movePower;

                }
            }
            //�I�u�W�F�N�g���I������Ă��Ȃ���
            else
            {
                //�I�u�W�F�N�g�I��͈͕\��
                SelectObj();
                //�h�b�g�̘g�\��
                Dotline();
            }
        }

        //�}�E�X�̍��W���L��
        beforePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }


    //�I�u�W�F�N�g�I��͈͕\���֐�
    private void SelectObj()
    {
        GameObject selectUI = managerAccessor.Instance.dataMagager.rightClickUIClone;
        if (!(selectUI != null &&
            selectUI.GetComponent<RectTransform>().position.x + (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) > Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.x - (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) < Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.y + (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) > Input.mousePosition.y &&
            selectUI.GetComponent<RectTransform>().position.y - (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) < Input.mousePosition.y))
        {
            //�������őI��͈͕\��
            if (Input.GetMouseButton(0))
            {
                if (first)
                {
                    //�I������Ă���I�u�W�F�N�g�f�[�^�̍폜
                    managerAccessor.Instance.dataMagager.selectObjsData.Clear();

                    //�I���J�n���̏����ʒu�L��
                    clickStartPos = Input.mousePosition;

                    //�͈͑I��p�I�u�W�F�N�g�̏������W�ݒ�
                    clone = Instantiate(managerAccessor.Instance.objDataManager.selectionObj);
                    clone.transform.localPosition = managerAccessor.Instance.dataMagager.MouseWorldChange();

                    //��������Ԃ���������܂ł͂���Ȃ��悤�ɂ���
                    first = false;
                }

                //�I��
                selectionMode = true;
                //�}�E�X�̈ړ��ʂőI��͈͎Z�o
                Vector3 inputData = Input.mousePosition - clickStartPos;
                //�ړ��ʒ���
                inputData.x /= 107;
                inputData.y /= 107;
                inputData.z /= 107;
                //�I��͈͓���
                clone.transform.localScale = inputData;
            }
            else
            {
                //�폜
                Destroy(clone);
                first = true;

                managerAccessor.Instance.dataMagager.copyReset = true;

                //�I������Ă���Ƃ��̃I�u�W�F�N�g�ԍ����Z�b�g
                managerAccessor.Instance.dataMagager.objNum = 0;

                //�I���O
                selectionMode = false;
            }
        }
    }


    //�h�b�g�̘g�\���p�֐�
    private void Dotline()
    {
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //�I���J�n����1�t���[���ڂ̃}�E�X�̍��W�擾
            if (Input.GetMouseButton(0))
            {
                if (first2)
                {
                    startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    first2 = false;
                }
            }
            else
                first2 = true;

            //�͈͑I������
            if (Input.GetMouseButton(0))
            {
                //DataManager�擾
                DataManager dataManager = managerAccessor.Instance.dataMagager;
                //�����ʒu���
                Vector2 usePos = startPos;
                //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
                int checkPos = 0;

                //�l�p�̃T�C�Y�v�Z
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //�h�b�g��ł����v�Z
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //�����ʒu�����炷
                square += startPos;

                //�h�b�g�̏�����
                for (int i = 0; i < cloneDot.Count; i++)
                    Destroy(cloneDot[i]);
                cloneDot.Clear();

                //startPos���猩�č����ɂ�����
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 1;
                //startPos���猩�č���ɂ�����
                else if (dataManager.MouseWorldChange().x < startPos.x)
                    checkPos = 2;
                //startPos���猩�ĉE���ɂ�����
                else if (dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 3;

                //�h�b�g��`��
                for (int i = 0; i < dotNum; i++)
                {
                    //��
                    if (usePos.x < square.x && usePos.y == startPos.y)
                    {
                        //�h�b�g��ł��W�����炷
                        usePos.x += wide;

                        //�����l�p����͂ݏo�����ꍇ�A�o���������̕`��ʒu�ɏC������
                        if (usePos.x > square.x)
                        {
                            usePos.y += usePos.x - square.x;
                            usePos.x = square.x;
                        }
                    }
                    //�E
                    else if (usePos.y < square.y && usePos.x == square.x)
                    {
                        usePos.y += wide;

                        if (usePos.y > square.y)
                        {
                            usePos.x -= usePos.y - square.y;
                            usePos.y = square.y;
                        }
                    }
                    //��
                    else if (usePos.x > startPos.x && usePos.y == square.y)
                    {
                        usePos.x -= wide;

                        if (usePos.x < startPos.x)
                        {
                            usePos.y -= startPos.x - usePos.x;
                            usePos.x = startPos.x;
                        }
                    }
                    //��
                    else if (usePos.y > startPos.y && usePos.x == startPos.x)
                    {
                        usePos.y -= wide;

                        if (usePos.y < startPos.y)
                        {
                            usePos.x -= startPos.y - usePos.y;
                            usePos.y = startPos.y;
                        }
                    }

                    //�C���l����p
                    Vector2 inUsePos = usePos;

                    //startPos���猩�č����ɂ�����
                    if (checkPos == 1)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.y = -usePos.y;
                        inUsePos += startPos * 2;
                    }
                    //startPos���猩�č���ɂ�����
                    else if (checkPos == 2)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.x += startPos.x * 2;
                    }
                    //startPos���猩�ĉE���ɂ�����
                    else if (checkPos == 3)
                    {
                        inUsePos.y = -usePos.y;
                        inUsePos.y += startPos.y * 2;
                    }

                    //�h�b�g�̕`��
                    cloneDot.Add(Instantiate(managerAccessor.Instance.objDataManager.dotObj));
                    cloneDot[i].transform.position = inUsePos;
                }

            }
        }
    }

}
