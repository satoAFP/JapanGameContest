using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField, Header("�S�[������L�����̐�")] private int charaNum;


    private int charaCount = 0;

    // Update is called once per frame
    void Update()
    {
        


    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //��l���ɓ���������
        if (collision.gameObject.tag == "Player") 
        {
            Destroy(collision.gameObject);
            charaCount++;

            //�w�肵�����̃L�������S�[��������
            if (charaNum == charaCount)
            {
                GameObject clone = Instantiate(managerAccessor.Instance.objDataManager.clearPanel);
                clone.transform.parent = managerAccessor.Instance.objDataManager.canvas.transform;
            }
        }
    }


}
