using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjs : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {

            managerAccessor.Instance.dataMagager.selectObjs.Add(collision.gameObject);
        }
    }
}
