using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjs : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            //選択されたオブジェクトを追加
            managerAccessor.Instance.dataMagager.selectObjsData.Add(collision.gameObject);

            //コピーデータリセット処理
            if (managerAccessor.Instance.dataMagager.copyReset)
            {
                managerAccessor.Instance.dataMagager.copyObjsData.Clear();
                managerAccessor.Instance.dataMagager.copyReset = false;
            }

            //コピー用データを記憶
            managerAccessor.Instance.dataMagager.copyObjsData.Add(collision.gameObject);
            //新しく記憶した場合コピーボタンを押すまで貼り付けれない
            managerAccessor.Instance.dataMagager.objsCopy = false;

            
        }
    }
}
