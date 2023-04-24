using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameObject.SetActive(true);
        }


    }
}
