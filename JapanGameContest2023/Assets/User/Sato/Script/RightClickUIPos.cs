using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickUIPos : MonoBehaviour
{
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        //キャラを操作中は選択できない
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            if (Input.GetMouseButton(1))
            {

                if (first)
                {
                    //古い右クリックUIを消去
                    Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);
                    ObjDataManager objm = managerAccessor.Instance.objDataManager;
                    //新しい右クリックUIの生成
                    managerAccessor.Instance.dataMagager.rightClickUIClone = Instantiate(objm.rightClickUI);
                    managerAccessor.Instance.dataMagager.rightClickUIClone.transform.localPosition = Input.mousePosition;
                    managerAccessor.Instance.dataMagager.rightClickUIClone.transform.parent = gameObject.transform;
                    first = false;
                }
            }
            else
                first = true;

            if(Input.GetMouseButton(0))
            {
                //UI取得用
                RectTransform rightclickUIClone;

                if (managerAccessor.Instance.dataMagager.rightClickUIClone != null)
                {
                    //UI取得
                    rightclickUIClone = managerAccessor.Instance.dataMagager.rightClickUIClone.GetComponent<RectTransform>();
                    Vector2 mouse = Input.mousePosition;

                    //UIの外をクリックしたときUIが消える
                    if (!(rightclickUIClone.position.x - (rightclickUIClone.sizeDelta.x / 2) < mouse.x &&
                        rightclickUIClone.position.x + (rightclickUIClone.sizeDelta.x / 2) > mouse.x &&
                        rightclickUIClone.position.y - (rightclickUIClone.sizeDelta.y / 2) < mouse.y &&
                        rightclickUIClone.position.y + (rightclickUIClone.sizeDelta.y / 2) > mouse.y)) 
                    {
                        Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);
                    }
                }
            }

        }
    }

    
}
