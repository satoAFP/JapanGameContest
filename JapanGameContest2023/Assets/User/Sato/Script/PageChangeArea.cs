using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageChangeArea : MonoBehaviour
{
    [SerializeField, Header("切り替えたいオブジェクト")] private List<GameObject> stage;


    private void Start()
    {
        //ステージ内の1つ目のタブのみ表示
        for (int i = 1; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
        }
    }


    public void ChangeTab(int num)
    {
        //一旦タブを非表示
        for (int i = 0; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
        }

        //押されたタブの表示
        stage[num - 1].SetActive(true);
    }


}
