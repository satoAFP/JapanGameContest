using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [SerializeField, Header("�S�[������L�����̐�")] private GameObject ClearPanel;

    [SerializeField, Header("�S�[������L�����̐�")] private int charaNum;


    // Update is called once per frame
    void Update()
    {
        if (charaNum == managerAccessor.Instance.dataMagager.goalPlayerNum) 
        {
            ClearPanel.SetActive(true);
        }

    }
}
