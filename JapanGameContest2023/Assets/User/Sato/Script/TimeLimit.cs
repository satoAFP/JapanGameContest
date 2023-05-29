using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    [SerializeField, Header("エラーウィンドウ")] private GameObject errorWindow;

    [SerializeField, Header("エラーウィンドウ表示時SE")] private AudioClip errorWindowSE;

    [SerializeField, Header("エラーウィンドウ最初を出す座標")] private Vector3 windowPopPos;

    [SerializeField, Header("エラーウィンドウ出す間隔")] private int windowInterval;

    [SerializeField, Header("エラーウィンドウを大量に出すタイミング")] private int windowPopTime;

    [SerializeField, Header("エラーウィンドウを大量に出す時の間隔")] private int windowPopTimeInterval;

    [SerializeField, Header("エラーウィンドウをずらす距離")] private Vector3 shiftPos;

    [SerializeField, Header("エラーウィンドウが画面から出たときずらす距離")] private Vector3 outShiftPos;

    [SerializeField, Header("エラーウィンドウが切り返すY座標")] private float restartPosY;

    private float countTime = 0.0f;                     //deltaTimeの数値代入用
    private int windowPopCount = 0;                     //エラーウィンドウを出すタイミング
    private DataManager dataManager;                    //dataManager取得用
    private GameObject clone;                           //エラーウィンドウ複製用
    private AudioSource audio;                          //SE再生用
    private int frameCount = 0;                         //フレーム計測用

    //1度だけ実行する処理用
    private bool first = true;

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

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

        //キャラが死ぬとエラーウィンドウが出ない
        if (!managerAccessor.Instance.dataMagager.playerlost)
        {
            //制限時間の半分の時間になるとエラーウィンドウを出し始める
            if (dataManager.stageTime / 2 <= (int)countTime)
            {
                //エラーウィンドウを出すタイミングを管理
                if (windowPopCount <= (int)countTime)
                {
                    //決められた時間になるまで1秒枚に表示
                    if (!(dataManager.stageTime - windowPopTime <= (int)countTime))
                    {
                        windowPopCount += windowInterval;
                        DuplicationErrorWindow(new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-4.0f, 4.0f)));
                    }
                    //決められた時間になるとwindowPopTimeIntervalフレームに1枚表示
                    else
                    {
                        if (frameCount % windowPopTimeInterval == 0)
                        {
                            DuplicationErrorWindow(windowPopPos);

                            //表示する座標をずらす
                            windowPopPos += shiftPos;

                            //折り返し処理
                            if (clone.transform.localPosition.y >= restartPosY)
                            {
                                windowPopPos -= outShiftPos;
                            }
                        }
                    }
                }
            }
            //制限時間になると死亡判定
            if (dataManager.stageTime <= (int)countTime)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
                managerAccessor.Instance.dataMagager.timeDeth = true;
            }

            //フレーム加算
            frameCount++;
        }
    }

    /// <summary>
    /// エラーウィンドウ複製用関数
    /// </summary>
    /// <param name="pos">複製する座標</param>
    private void DuplicationErrorWindow(Vector3 pos)
    {
        clone = Instantiate(errorWindow, gameObject.transform);
        clone.transform.localPosition = pos;
        audio.PlayOneShot(errorWindowSE);
    }
}
