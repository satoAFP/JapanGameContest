using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    [SerializeField, Header("エラーウィンドウ")] private GameObject errorWindow;

    [SerializeField, Header("エラーウィンドウを大量に出すタイミング")] private int windowPopTime;

    private float countTime = 0.0f;                     //deltaTimeの数値代入用
    private int windowPopCount = 0;                     //エラーウィンドウを出すタイミング
    private Vector3 windowPopPos = new Vector3(0, 0, 0);//エラーウィンドウを出す座標
    private DataManager dataManager;                    //dataManager取得用

    //1度だけ実行する処理用
    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //dataManager取得
        dataManager = managerAccessor.Instance.dataMagager;

        if (first)
        {
            //時間制限の半分の時間を取得
            windowPopCount = dataManager.stageTime / 2;
            first = false;
        }

        //時間加算
        countTime += Time.deltaTime;
        
        //制限時間の半分の時間になるとエラーウィンドウを出し始める
        if (dataManager.stageTime / 2 <= (int)countTime) 
        {
            //エラーウィンドウを出すタイミングを管理
            if (windowPopCount <= (int)countTime)
            {
                //決められた時間になるまで1秒枚に表示
                if (!(dataManager.stageTime - windowPopTime <= (int)countTime))
                {
                    windowPopCount++;
                    DuplicationErrorWindow(windowPopPos);
                }
                //決められた時間になると1フレームに1枚表示
                else
                {
                    DuplicationErrorWindow(windowPopPos);
                }
                //表示する座標をずらす
                windowPopPos += new Vector3(0.2f, 0.2f, 0);
            }
        }
        //制限時間になると死亡判定
        if (dataManager.stageTime <= (int)countTime) 
        {
            managerAccessor.Instance.dataMagager.playerlost = true;
            managerAccessor.Instance.dataMagager.timeDeth = true;
        }
    }

    /// <summary>
    /// エラーウィンドウ複製用関数
    /// </summary>
    /// <param name="pos">複製する座標</param>
    private void DuplicationErrorWindow(Vector3 pos)
    {
        GameObject clone = Instantiate(errorWindow, gameObject.transform);
        clone.transform.localPosition = pos;
    }
}
