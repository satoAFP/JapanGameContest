using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> selectObjs = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        managerAccessor.Instance.dataMagager = this;
    }
}
