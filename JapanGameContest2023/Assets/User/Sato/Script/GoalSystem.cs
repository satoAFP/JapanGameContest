using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [SerializeField, Header("ゴールするキャラの数")] private GameObject ClearPanel;

    [SerializeField, Header("ゴールするキャラの数")] private int charaNum;


    // Update is called once per frame
    void Update()
    {
        if (charaNum == managerAccessor.Instance.dataMagager.goalPlayerNum) 
        {
            ClearPanel.SetActive(true);
        }

    }
}
