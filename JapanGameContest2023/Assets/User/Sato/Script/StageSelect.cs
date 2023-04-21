using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("縦に表示するステージの数")] private int verticalObjNum;

    [SerializeField, Header("ダブルクリックする間隔時間")] private int clickFrameRate;

    private List<GameObject> stages = new List<GameObject>();
    private int frameCount = 0;
    private Vector3 inputPos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            inputPos.y += 10 / verticalObjNum;
            if ((i + 1) % verticalObjNum == 0) 
            {
                inputPos.x += 10 / verticalObjNum;
                inputPos.y = 0;
            }

            stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
            stages[i].transform.position = inputPos;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
