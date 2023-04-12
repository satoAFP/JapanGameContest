using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    [SerializeField, Header("�͈͑I��\���p�I�u�W�F�N�g")]
    private GameObject selectionObj;

    private bool first = true;
    private Vector3 clickStartPos;
    private GameObject clone;
    private bool selectionMode = false;
    private bool editMode = false;
    private Vector3 beforePos = new Vector3(0, 0, 0);


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
                    {
                        Objs[i].transform.localPosition += movePower;
                    }
                }
            }
            //�I�u�W�F�N�g���I������Ă��Ȃ���
            else
            {
                //�I�u�W�F�N�g�I��͈͕\��
                SelectObj();
            }
        }

        //�}�E�X�̍��W���L��
        beforePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }


    //�I�u�W�F�N�g�I��͈͕\���֐�
    private void SelectObj()
    {
        GameObject selectUI = managerAccessor.Instance.dataMagager.rightClickUIClone;
        if (selectUI != null &&
            selectUI.GetComponent<RectTransform>().position.x + (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) > Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.x - (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) < Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.y + (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) > Input.mousePosition.y &&
            selectUI.GetComponent<RectTransform>().position.y - (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) < Input.mousePosition.y)
        {

        }
        else
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
                    clone = Instantiate(selectionObj);
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



}
