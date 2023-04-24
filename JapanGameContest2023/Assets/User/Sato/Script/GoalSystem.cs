using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [SerializeField, Header("ゴールするキャラの数")] private GameObject ClearPanel;

    [SerializeField, Header("ゴールするキャラの数")] private int charaNum;

    [System.NonSerialized] public int goalCount;


    // Update is called once per frame
    void Update()
    {
        if (charaNum == goalCount) 
        {
            ClearPanel.SetActive(true);
        }

    }
}
