using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    [SerializeField, Header("�͈͑I��\���p�I�u�W�F�N�g")]
    private GameObject selectionObj;

    private bool first = true;
    private bool first2 = true;
    private Vector3 clickStartPos;
    private GameObject clone;
    private bool selectionMode = false;
    private bool editMode = false;
    private Vector3 beforePos = new Vector3(0, 0, 0);


    // Update is called once per frame
    void FixedUpdate()
    {
        List<GameObject> Objs = managerAccessor.Instance.dataMagager.selectObjs;

        if (!selectionMode)
        {
            for (int i = 0; i < Objs.Count; i++)
            {
                Vector3 nowMousePos = MouseWorldChange();
                if (Objs[i].transform.localPosition.x - Objs[i].transform.localScale.x < nowMousePos.x &&
                    Objs[i].transform.localPosition.x + Objs[i].transform.localScale.x > nowMousePos.x &&
                    Objs[i].transform.localPosition.y - Objs[i].transform.localScale.x < nowMousePos.y &&
                    Objs[i].transform.localPosition.y + Objs[i].transform.localScale.x > nowMousePos.y)
                {
                    Debug.Log("aaa");
                    editMode = true;
                    break;
                }
                else
                    editMode = false;
            }
        }

        if (editMode)
        {
            if (Input.GetMouseButton(0))
            {
                if (first2)
                {
                    //��������Ԃ���������܂ł͂���Ȃ��悤�ɂ���
                    first2 = false;
                }
                else
                {
                    Vector3 movePower = MouseWorldChange() - beforePos;
                    
                    for (int i = 0; i < Objs.Count; i++)
                    {
                        Objs[i].transform.localPosition += movePower;
                    }
                }

                editMode = true;
                beforePos = MouseWorldChange();
            }
        }
        else
            first2 = true;

        if (!editMode)
            SelectObj();

        

    }


    //�I�u�W�F�N�g�I��͈͕\���֐�
    private void SelectObj()
    {
        //�������őI��͈͕\��
        if (Input.GetMouseButton(0))
        {
            if (first)
            {
                //�I������Ă���I�u�W�F�N�g�f�[�^�̍폜
                managerAccessor.Instance.dataMagager.selectObjs.Clear();

                //�I���J�n���̏����ʒu�L��
                clickStartPos = Input.mousePosition;

                //�͈͑I��p�I�u�W�F�N�g�̏������W�ݒ�
                clone = Instantiate(selectionObj);
                clone.transform.localPosition = MouseWorldChange();

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

            //�I���O
            selectionMode = false;
        }
    }

    //�}�E�X���W�����[���h���W�ϊ��֐�
    private Vector3 MouseWorldChange()
    {
        //�I���J�n���̏����ʒu�L��
        Vector3 mousePos = Input.mousePosition;
        // Z���C��
        mousePos.z = 10f;
        // �}�E�X�ʒu���W���X�N���[�����W���烏�[���h���W�ɕϊ�����
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return screenToWorldPointPosition;
    }


}
