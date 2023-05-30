using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjs : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MoveBlock")
        {
            //選択されたオブジェクトを追加
            managerAccessor.Instance.dataMagager.selectObjsData.Add(collision.gameObject);
            collision.gameObject.GetComponent<MoveObj>().objNum = managerAccessor.Instance.dataMagager.objNum;

            //コピーデータリセット処理
            if (managerAccessor.Instance.dataMagager.copyReset)
            {
                managerAccessor.Instance.dataMagager.copyObjsData.Clear();
                managerAccessor.Instance.dataMagager.copyReset = false;
            }

            //コピー用データを記憶
            managerAccessor.Instance.dataMagager.copyObjsData.Add(collision.gameObject);

            //選択されているオブジェクトに入れるナンバーを進ませる
            managerAccessor.Instance.dataMagager.objNum++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MoveBlock")
        {
            //範囲選択中選択を外すと選択が解除される処理
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
