using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickUIPos : MonoBehaviour
{
    private bool first = true;
    private GameObject clone = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            
            if (first)
            {
                Destroy(clone);
                ObjDataManager objm = managerAccessor.Instance.objDataManager;
                clone = Instantiate(objm.rightClickUI);
                clone.transform.localPosition = Input.mousePosition;
                clone.transform.parent = objm.canvas.transform;
                first = false;
            }
        }
        else
            first = true;

        for(int i=0; i< managerAccessor.Instance.dataMagager.copyObjsData.Count;i++)
        {
            Debug.Log(managerAccessor.Instance.dataMagager.copyObjsData[i]);
        }



        //if (Input.GetMouseButton(0))
        //    Destroy(clone);
    }

    
}
