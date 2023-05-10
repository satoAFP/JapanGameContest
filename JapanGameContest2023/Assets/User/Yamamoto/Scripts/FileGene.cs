using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    public int playercount = 0;//プレイヤーの数を数える

    [SerializeField, Header("生成する移動指標オブジェクト")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//移動指標オブジェクトを入れる（削除命令に使う）

    private bool nocreate = false;//オブジェクト生成をさせないフラグ


    // Start is called before the first frame update
    void Start()
    {
        playercount = GameObject.FindGameObjectsWithTag("Player").Length;  
    }

    // Update is called once per frame
    void Update()
    {
        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {

            //Decoyファイルにふれたときおとりファイルを消去
            if (managerAccessor.Instance.dataMagager.onDecoyFile)
            {
                Destroy(CreateObj);
            }

            if (!managerAccessor.Instance.dataMagager.noTapArea)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // クリックされた位置を取得
                    managerAccessor.Instance.dataMagager.clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    managerAccessor.Instance.dataMagager.clickPosition.z = 0; // z座標を0に設定（2Dゲームなので）


                    if (CreateObj == null) //初めて移動指標オブジェクトを作るとき
                    {

                        CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//移動指標オブジェクト作成
                        nocreate = true;
                    }
                    else
                    {
                        //すでに移動指標オブジェクト作成が作られている場合
                        Destroy(CreateObj);//前回作ったものを削除
                        CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//新たに移動指標オブジェクト作成
                    }




                }
            }

                

            if (playercount == 0)//移動しているプレイヤーが0になるとCreateObj削除
            {
                Destroy(CreateObj);
                nocreate = false;
                playercount = GameObject.FindGameObjectsWithTag("Player").Length;//プレイヤーの数を再カウント
            }
           

        }

        
    }
}
