using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> selectObjs = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
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

    public void CopyButton()
    {
        DataManager dataManager = managerAccessor.Instance.dataMagager;

        dataManager.copyObjsData.Clear();
        for (int i = 0; i < dataManager.selectObjs.Count; i++)
        {
            Debug.Log("aaa");
            dataManager.copyObjsData.Add(dataManager.selectObjs[i]);
        }
    }
}
