using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] Icon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gameObject.GetComponent<SpriteRenderer>().sprite = Icon[0];

        transform.position = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }
}
