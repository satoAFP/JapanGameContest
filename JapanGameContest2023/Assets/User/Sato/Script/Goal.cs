using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField, Header("ゴールするキャラの数")] private int charaNum;


    private int charaCount = 0;

    // Update is called once per frame
    void Update()
    {
        


    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //主人公に当たった時
        if (collision.gameObject.tag == "Player") 
        {
            Destroy(collision.gameObject);
            charaCount++;

            //指定した数のキャラがゴールした時
            if (charaNum == charaCount)
            {
                GameObject clone = Instantiate(managerAccessor.Instance.objDataManager.clearPanel);
                clone.transform.parent = managerAccessor.Instance.objDataManager.canvas.transform;
            }
        }
    }


}
