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
            collision.gameObject.GetComponent<MoveObj>().objNum = 0;

            //�R�s�[�f�[�^���Z�b�g����
            if (managerAccessor.Instance.dataMagager.copyReset)
            {
                managerAccessor.Instance.dataMagager.copyObjsData.Clear();
                managerAccessor.Instance.dataMagager.copyReset = false;
            }

            //�R�s�[�p�f�[�^���L��
            managerAccessor.Instance.dataMagager.copyObjsData.Add(collision.gameObject);
            //�V�����L�������ꍇ�R�s�[�{�^���������܂œ\��t����Ȃ�
            managerAccessor.Instance.dataMagager.objsCopy = false;

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MoveBlock")
        {
            Debug.Log("aaa");

        }
    }
}
