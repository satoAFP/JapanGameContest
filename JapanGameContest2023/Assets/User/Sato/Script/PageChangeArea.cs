using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageChangeArea : MonoBehaviour
{
    [SerializeField, Header("切り替えたいオブジェクト")] private List<GameObject> stage;


    private int count = 0;

    private void Start()
    {
        for (int i = 1; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
        }
    }


    public void ChangeTab(int num)
    {
        for (int i = 0; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
        }

        stage[num - 1].SetActive(true);
    }


}
