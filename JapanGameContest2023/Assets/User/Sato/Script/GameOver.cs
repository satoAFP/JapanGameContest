using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        Debug.Log(managerAccessor.Instance.dataMagager.playerlost);

        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }


    }
}
