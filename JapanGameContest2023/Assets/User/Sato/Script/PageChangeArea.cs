using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageChangeArea : MonoBehaviour
{
    [SerializeField, Header("切り替えたいオブジェクト")] public List<GameObject> stage;


    private void Start()
    {
        //tabボタンに番号振り分け
        transform.GetChild(0).gameObject.GetComponent<TabButton>().number = 0;

        //ステージ内の1つ目のタブのみ表示
        for (int i = 1; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
            //配列1以降のtabボタンにも番号振り分け
            transform.GetChild(i).gameObject.GetComponent<TabButton>().number = i;
            transform.GetChild(i).gameObject.GetComponent<TabButton>().selectPanel.SetActive(false);
        }
    }


    public void ChangeTab(int num)
    {
        //移動中は変更できない
        if (!managerAccessor.Instance.dataMagager.isMoving)
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
            transform.GetChild(num - 1).gameObject.GetComponent<TabButton>().isPutButton = true;
        }
    }


}
