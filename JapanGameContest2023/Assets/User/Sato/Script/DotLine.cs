using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotLine : MonoBehaviour
{
    [SerializeField, Header("囲うようのドット")] private GameObject dotObj;

    [SerializeField, Header("ドット描画時の間隔")] private float wide;


    //ドット格納用
    private List<GameObject> clone = new List<GameObject>();
    //クリックしたときの初期位置
    private Vector2 startPos;
    //四角の縦横の長さ
    private Vector2 square;
    //ドットの数格納用
    private int dotNum;


    //最初の一回しか通らない処理用
    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //選択開始時の1フレーム目のマウスの座標取得
            if (Input.GetMouseButton(0))
            {
                if (first)
                {
                    startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    first = false;
                }
            }
            else
                first = true;

            //範囲選択処理
            if (Input.GetMouseButton(0))
            {
                //DataManager取得
                DataManager dataManager = managerAccessor.Instance.dataMagager;
                //初期位置代入
                Vector2 usePos = startPos;
                //クリック後、startPosを原点に縦横それぞれの位置チェック用
                int checkPos = 0;

                //四角のサイズ計算
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //ドットを打つ数を計算
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //初期位置分ずらす
                square += startPos;

                //ドットの初期化
                for (int i = 0; i < clone.Count; i++)
                    Destroy(clone[i]);
                clone.Clear();

                //startPosから見て左下にいた時
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 1;
                //startPosから見て左上にいた時
                else if (dataManager.MouseWorldChange().x < startPos.x)
                    checkPos = 2;
                //startPosから見て右下にいた時
                else if (dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 3;

                //ドットを描画
                for (int i = 0; i < dotNum; i++)
                {
                    //下
                    if (usePos.x < square.x && usePos.y == startPos.y)
                    {
                        //ドットを打つ座標をずらす
                        usePos.x += wide;

                        //もし四角からはみ出した場合、出た分を次の描画位置に修正する
                        if (usePos.x > square.x)
                        {
                            usePos.y += usePos.x - square.x;
                            usePos.x = square.x;
                        }
                    }
                    //右
                    else if (usePos.y < square.y && usePos.x == square.x)
                    {
                        usePos.y += wide;

                        if (usePos.y > square.y)
                        {
                            usePos.x -= usePos.y - square.y;
                            usePos.y = square.y;
                        }
                    }
                    //上
                    else if (usePos.x > startPos.x && usePos.y == square.y)
                    {
                        usePos.x -= wide;

                        if (usePos.x < startPos.x)
                        {
                            usePos.y -= startPos.x - usePos.x;
                            usePos.x = startPos.x;
                        }
                    }
                    //左
                    else if (usePos.y > startPos.y && usePos.x == startPos.x)
                    {
                        usePos.y -= wide;

                        if (usePos.y < startPos.y)
                        {
                            usePos.x -= startPos.y - usePos.y;
                            usePos.y = startPos.y;
                        }
                    }

                    //修正値代入用
                    Vector2 inUsePos = usePos;

                    //startPosから見て左下にいた時
                    if (checkPos == 1)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.y = -usePos.y;
                        inUsePos += startPos * 2;
                    }
                    //startPosから見て左上にいた時
                    else if (checkPos == 2)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.x += startPos.x * 2;
                    }
                    //startPosから見て右下にいた時
                    else if (checkPos == 3)
                    {
                        inUsePos.y = -usePos.y;
                        inUsePos.y += startPos.y * 2;
                    }

                    //ドットの描画
                    clone.Add(Instantiate(dotObj));
                    clone[i].transform.position = inUsePos;
                }

            }
        }
    }
}
