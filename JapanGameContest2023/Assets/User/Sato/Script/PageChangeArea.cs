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
            transform.GetChild(i).gameObject.GetComponent<TabButton>().selectPanel.SetActive(false);
        }
    }


    public void ChangeTab(int num)
    {
        //キャラを操作中は選択できない
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //一旦タブを非表示
            for (int i = 0; i < stage.Count; i++)
            {
                stage[i].SetActive(false);
                transform.GetChild(i).gameObject.GetComponent<TabButton>().selectPanel.SetActive(false);
            }

            //押されたタブの表示
            stage[num - 1].SetActive(true);
            transform.GetChild(num - 1).gameObject.GetComponent<TabButton>().selectPanel.SetActive(true);
        }
    }


}
