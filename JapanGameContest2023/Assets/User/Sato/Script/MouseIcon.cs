using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] cursor;

    [SerializeField] private Sprite[] arrow;



    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }
}
