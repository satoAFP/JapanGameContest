using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField, Header("GameOverPanel")] private GameObject gameoverPanel;

    // Update is called once per frame
    void Update()
    {
        if (managerAccessor.Instance.dataMagager.playerlost == true) 
        {
            gameoverPanel.SetActive(true);
        }


    }
}
