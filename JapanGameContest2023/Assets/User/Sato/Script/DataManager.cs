using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> selectObjsData = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> copyObjsData = new List<GameObject>();

    [System.NonSerialized] public GameObject rightClickUIClone = null;

    [System.NonSerialized] public bool objsCopy = false;
    [System.NonSerialized] public bool copyReset = true;

    [System.NonSerialized] public bool playMode = true;

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

    public void ModeChange()
    {
        playMode = !playMode;
    }

    public void CopyButton()
    {
        objsCopy = true;
    }

    public void PasteButton()
    {
        Debug.Log(managerAccessor.Instance.dataMagager.copyObjsData[0].name);
        Vector3 moveAmount = MouseWorldChange() - managerAccessor.Instance.dataMagager.copyObjsData[0].transform.localPosition;

        for (int i = 0; i < managerAccessor.Instance.dataMagager.copyObjsData.Count; i++) 
        {
            GameObject clone = Instantiate(managerAccessor.Instance.dataMagager.copyObjsData[i]);
            clone.transform.localPosition += moveAmount;
        }
    }
}
