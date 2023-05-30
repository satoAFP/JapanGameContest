using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjs : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MoveBlock")
        {
            //�I�����ꂽ�I�u�W�F�N�g��ǉ�
            managerAccessor.Instance.dataMagager.selectObjsData.Add(collision.gameObject);
            collision.gameObject.GetComponent<MoveObj>().objNum = managerAccessor.Instance.dataMagager.objNum;

            //�R�s�[�f�[�^���Z�b�g����
            if (managerAccessor.Instance.dataMagager.copyReset)
            {
                managerAccessor.Instance.dataMagager.copyObjsData.Clear();
                managerAccessor.Instance.dataMagager.copyReset = false;
            }

            //�R�s�[�p�f�[�^���L��
            managerAccessor.Instance.dataMagager.copyObjsData.Add(collision.gameObject);

            //�I������Ă���I�u�W�F�N�g�ɓ����i���o�[��i�܂���
            managerAccessor.Instance.dataMagager.objNum++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MoveBlock")
        {
            //�͈͑I�𒆑I�����O���ƑI������������鏈��
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < managerAccessor.Instance.dataMagager.selectObjsData.Count; i++) 
                {
                    if (managerAccessor.Instance.dataMagager.selectObjsData[i].GetComponent<MoveObj>().objNum == collision.GetComponent<MoveObj>().objNum)
                    {
                        managerAccessor.Instance.dataMagager.selectObjsData.RemoveAt(i);
                        managerAccessor.Instance.dataMagager.copyObjsData.RemoveAt(i);
                    }
                }
            }
        }
    }
}
