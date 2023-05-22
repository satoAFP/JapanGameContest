using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickUIPos : MonoBehaviour
{
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        //�L�����𑀍쒆�͑I���ł��Ȃ�
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            if (Input.GetMouseButton(1))
            {

                if (first)
                {
                    //�Â��E�N���b�NUI������
                    Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);
                    ObjDataManager objm = managerAccessor.Instance.objDataManager;
                    //�V�����E�N���b�NUI�̐���
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
                //UI�擾�p
                RectTransform rightclickUIClone;

                if (managerAccessor.Instance.dataMagager.rightClickUIClone != null)
                {
                    //UI�擾
                    rightclickUIClone = managerAccessor.Instance.dataMagager.rightClickUIClone.GetComponent<RectTransform>();
                    Vector2 mouse = Input.mousePosition;

                    //UI�̊O���N���b�N�����Ƃ�UI��������
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
