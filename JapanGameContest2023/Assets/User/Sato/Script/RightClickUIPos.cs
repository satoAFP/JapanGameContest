using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickUIPos : MonoBehaviour
{
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        //ƒLƒƒƒ‰‚ğ‘€ì’†‚Í‘I‘ğ‚Å‚«‚È‚¢
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            if (Input.GetMouseButton(1))
            {

                if (first)
                {
                    Destroy(managerAccessor.Instance.dataMagager.rightClickUIClone);
                    ObjDataManager objm = managerAccessor.Instance.objDataManager;
                    managerAccessor.Instance.dataMagager.rightClickUIClone = Instantiate(objm.rightClickUI);
                    managerAccessor.Instance.dataMagager.rightClickUIClone.transform.localPosition = Input.mousePosition;
                    managerAccessor.Instance.dataMagager.rightClickUIClone.transform.parent = objm.canvas.transform;
                    first = false;
                }
            }
            else
                first = true;

            //for (int i = 0; i < managerAccessor.Instance.dataMagager.copyObjsData.Count; i++)
            //{
            //    Debug.Log(managerAccessor.Instance.dataMagager.objsCopy);
            //}

        }

        //if (Input.GetMouseButton(0))
        //    Destroy(clone);
    }

    
}
