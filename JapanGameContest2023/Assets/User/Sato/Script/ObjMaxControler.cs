using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMaxControler : MonoBehaviour
{
    [SerializeField,Header("出せるオブジェクトの最大数")] private int objMax;


    // Update is called once per frame
    void Update()
    {
        int childObj = transform.childCount;

        Debug.Log(childObj);

        if (childObj >= objMax) 
        {
            managerAccessor.Instance.dataMagager.objMaxFrag = true;
        }
        else
        {
            managerAccessor.Instance.dataMagager.objMaxFrag = false;
        }


    }
}
