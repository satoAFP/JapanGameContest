using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    public enum ChangeSizePosName
    {
        DOWN,
        RIGHT,
        UP,
        LEFT,
        RIGHT_DOWN,
        RIGHT_UP,
        LEFT_UP,
        LEFT_DOWN,
        NONE,
    }


    [SerializeField, Header("ドット描画時の間隔")] private float wide;

    [SerializeField, Header("サイズ変更できる幅")] private Vector2 changeSizeWidth;

    //最初の一回しか通らない処理用
    private bool first = true;
    private bool first2 = true;
    private bool first3 = false;
    private bool first4 = true;

    private List<GameObject> Objs=new List<GameObject>();//選択されているオブジェクト格納用
    private Vector3 clickStartPos;                      //クリック開始時の初期位置
    private GameObject clone;                           //選択範囲表示用オブジェクト
    private bool selectionMode = false;                 //オブジェクトを選択しているかどうか
    private bool editMode = false;                      //オブジェクトを選択しているかどうか
    private Vector3 beforePos = new Vector3(0, 0, 0);   //一フレーム前のマウスの位置
    [SerializeField] private Vector2 judgeStartPos;                      //掴める範囲(左下)
    [SerializeField] private Vector2 judgeEndPos;                        //掴める範囲(右上)
    private bool onEdge = false;                        //ドットの枠の縁に乗っているとき
    private int onPos = (int)ChangeSizePosName.NONE;    //今乗っている縁の場所(列挙参照)
    private Vector2 onStartPos;                         //拡大縮小時の初期位置
    private Vector2 backUpSquare;                       //ドットの四角のバックアップデータ
    private List<Vector3> memsize = new List<Vector3>();//選択されているオブジェクトのサイズデータ

    private List<GameObject> cloneDot = new List<GameObject>();//ドット格納用
    private Vector2 startPos;                           //クリックしたときの初期位置
    private Vector2 usePos;                             //初期位置代入
    [SerializeField]private Vector2 square;                             //四角の縦横の長さ
    [SerializeField] private int checkPos = 0;                           //クリック後、startPosを原点に縦横それぞれの位置チェック用
    [SerializeField] private int dotNum;                                 //ドットの数格納用
    private Vector2 inUsePos;                           //usePosの修正値代入用


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Objs.Count != 0)
            Debug.Log(Objs[0].transform.localScale);
        //キャラを操作中は選択できない
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //選択されてるオブジェクトが格納される
            Objs = managerAccessor.Instance.dataMagager.selectObjsData;

            //オブジェクトが選択されている時
            if (!selectionMode)
            {
                //マウス座標をワールド座標に変換
                Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

                CheckRangeSize();

                if (onEdge)
                {
                    //オブジェクト選択状態にする
                    editMode = true;
                }
                //ドットの枠内の場合動かせるようになる
                else if (judgeStartPos.x < nowMousePos.x && judgeEndPos.x > nowMousePos.x &&
                    judgeStartPos.y < nowMousePos.y && judgeEndPos.y > nowMousePos.y) 
                {
                    //オブジェクト選択状態にする
                    editMode = true;
                }
                else
                {
                    //マウスがクリックされている状態の場合、選択状態を解除しない
                    if (!Input.GetMouseButton(0))
                        editMode = false;
                }
            }

            //オブジェクトが選択されている時
            if (editMode)
            {
                if (onEdge)
                {
                    ChangeRangeSize();

                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        //1フレーム前との誤差を算出
                        Vector3 movePower = managerAccessor.Instance.dataMagager.MouseWorldChange() - beforePos;

                        //選択されているオブジェクトに加算
                        for (int i = 0; i < Objs.Count; i++)
                            Objs[i].transform.localPosition += movePower;

                        //ドットの枠にも移動量加算
                        for (int i = 0; i < cloneDot.Count; i++)
                            cloneDot[i].transform.localPosition += movePower;

                        //ドットの枠も移動量加算
                        judgeStartPos.x += movePower.x;
                        judgeStartPos.y += movePower.y;
                        judgeEndPos.x += movePower.x;
                        judgeEndPos.y += movePower.y;

                    }
                }
            }
            //オブジェクトが選択されていない時
            else
            {
                //オブジェクト選択範囲表示
                SelectObj();
                //ドットの枠表示
                Dotline();
            }
        }

        //マウスの座標を記憶
        beforePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }


    //オブジェクト選択範囲表示関数
    private void SelectObj()
    {
        GameObject selectUI = managerAccessor.Instance.dataMagager.rightClickUIClone;
        if (!(selectUI != null &&
            selectUI.GetComponent<RectTransform>().position.x + (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) > Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.x - (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) < Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.y + (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) > Input.mousePosition.y &&
            selectUI.GetComponent<RectTransform>().position.y - (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) < Input.mousePosition.y))
        {
            //長押しで選択範囲表示
            if (Input.GetMouseButton(0))
            {
                if (first)
                {
                    //選択されているオブジェクトデータの削除
                    managerAccessor.Instance.dataMagager.selectObjsData.Clear();

                    //選択開始時の初期位置記憶
                    clickStartPos = Input.mousePosition;

                    //範囲選択用オブジェクトの初期座標設定
                    clone = Instantiate(managerAccessor.Instance.objDataManager.selectionObj);
                    clone.transform.localPosition = managerAccessor.Instance.dataMagager.MouseWorldChange();

                    //長押し状態を解除するまではいらないようにする
                    first = false;
                }

                //選択中
                selectionMode = true;
                //マウスの移動量で選択範囲算出
                Vector3 inputData = Input.mousePosition - clickStartPos;
                //移動量調整
                inputData.x /= 107;
                inputData.y /= 107;
                inputData.z /= 107;
                //選択範囲入力
                clone.transform.localScale = inputData;
            }
            else
            {
                //削除
                Destroy(clone);
                first = true;

                managerAccessor.Instance.dataMagager.copyReset = true;

                //選択されているときのオブジェクト番号リセット
                managerAccessor.Instance.dataMagager.objNum = 0;

                //選択外
                selectionMode = false;
            }
        }
    }


    //ドット位置計算用関数
    private void Dotline()
    {
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //選択開始時の1フレーム目のマウスの座標取得
            if (Input.GetMouseButton(0))
            {
                if (first2)
                {
                    startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    first2 = false;
                }
            }
            else
                first2 = true;

            //範囲選択処理
            if (Input.GetMouseButton(0))
            {
                //DataManager取得
                DataManager dataManager = managerAccessor.Instance.dataMagager;
                //初期位置代入
                usePos = startPos;
                //クリック後、startPosを原点に縦横それぞれの位置チェック用
                checkPos = 0;

                //四角のサイズ計算
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //ドットを打つ数を計算
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //初期位置分ずらす
                square += startPos;

                //ドットの初期化
                for (int i = 0; i < cloneDot.Count; i++)
                {
                    Destroy(cloneDot[i]);
                }
                cloneDot.Clear();

                //startPosから見て左下にいた時
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                {
                    checkPos = 1;
                }
                //startPosから見て左上にいた時
                else if (dataManager.MouseWorldChange().x < startPos.x)
                {
                    checkPos = 2;
                }
                //startPosから見て右下にいた時
                else if (dataManager.MouseWorldChange().y < startPos.y)
                {
                    checkPos = 3;
                }

                //ドットの描画
                DotDraw(startPos, usePos, square, dotNum);


                first3 = true;
            }
            else
            {
                //クリック終了時のマウスの座標取得
                if(first3)
                {
                    //右上
                    if (checkPos == 0)
                    {
                        judgeStartPos = startPos;
                        judgeEndPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    }
                    //左下
                    else if (checkPos == 1)
                    {
                        judgeStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                        judgeEndPos = startPos;
                    }
                    //左上
                    else if (checkPos == 2)
                    {
                        judgeStartPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeStartPos.y = startPos.y;
                        judgeEndPos.x = startPos.x;
                        judgeEndPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                    }
                    //右下
                    else if (checkPos == 3)
                    {
                        judgeStartPos.x = startPos.x;
                        judgeStartPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                        judgeEndPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeEndPos.y = startPos.y;
                    }
                    first3 = false;
                }
            }
        }
    }


    //縁のサイズ変更用関数
    private void ChangeRangeSize()
    {
        //選択開始時の1フレーム目のマウスの座標取得
        if (Input.GetMouseButton(0))
        {
            if (first4)
            {
                onStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                startPos = judgeStartPos;
                backUpSquare.x = Mathf.Abs(judgeEndPos.x - judgeStartPos.x);
                backUpSquare.y = Mathf.Abs(judgeEndPos.y - judgeStartPos.y);
                memsize.Clear();
                for (int i = 0; i < Objs.Count; i++)
                {
                    memsize.Add(Objs[i].transform.localScale);
                }
                //Debug.Log(memsize[0]);
                first4 = false;
            }
        }
        else
            first4 = true;

        //範囲選択処理
        if (Input.GetMouseButton(0))
        {
            //DataManager取得
            DataManager dataManager = managerAccessor.Instance.dataMagager;

            

            //初期位置から離れた距離
            Vector2 setStartPos = judgeStartPos;

            //四角のサイズ変更時それぞれの数値変更
            if (onPos == (int)ChangeSizePosName.DOWN) 
            {
                //四角のサイズ変更
                square.x = backUpSquare.x;
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                //初期位置の設定
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                //ゲーム全体で判定をする四角の座標更新
                judgeStartPos.y = setStartPos.y;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(0, -(dataManager.MouseWorldChange().y - onStartPos.y), 0));
            }
            if (onPos == (int)ChangeSizePosName.RIGHT)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                judgeEndPos.x = setStartPos.x + square.x;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, 0, 0));
            }
            if (onPos == (int)ChangeSizePosName.UP)
            {
                square.x = backUpSquare.x;
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(0, dataManager.MouseWorldChange().y - onStartPos.y, 0));
            }
            if (onPos == (int)ChangeSizePosName.LEFT)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), 0, 0));
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_DOWN)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeEndPos.x = setStartPos.x + square.x;
                judgeStartPos.y = setStartPos.y;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, -(dataManager.MouseWorldChange().y - onStartPos.y), 0));
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_UP)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.x = setStartPos.x + square.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, dataManager.MouseWorldChange().y - onStartPos.y, 0));
            }
            if (onPos == (int)ChangeSizePosName.LEFT_UP)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), dataManager.MouseWorldChange().y - onStartPos.y, 0));
            }
            if (onPos == (int)ChangeSizePosName.LEFT_DOWN)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeStartPos = setStartPos;
                //サイズ変更時の移動量設定
                BlockSizeChange(memsize, new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), -(dataManager.MouseWorldChange().y - onStartPos.y), 0));
            }

            

            //描画位置設定用座標のスタート位置初期化
            usePos = setStartPos;


            //ドットを打つ数を計算
            dotNum = (int)((square.x * 2 + square.y * 2) / wide);
            //初期位置分ずらす
            square += setStartPos;



            //ドットの初期化
            for (int i = 0; i < cloneDot.Count; i++)
                Destroy(cloneDot[i]);
            cloneDot.Clear();

            //描画の初期位置を左下に設定
            checkPos = 0;

            //ドットの描画
            DotDraw(setStartPos, usePos, square, dotNum);

        }
    }


    //ドット描画用関数
    private void DotDraw(Vector2 StartPos, Vector2 UsePos, Vector2 Square, int DotNum)
    {
        //ドットを描画
        for (int i = 0; i < DotNum; i++)
        {
            //下
            if (UsePos.x < Square.x && UsePos.y == StartPos.y)
            {
                //ドットを打つ座標をずらす
                UsePos.x += wide;

                //もし四角からはみ出した場合、出た分を次の描画位置に修正する
                if (UsePos.x > Square.x)
                {
                    UsePos.y += UsePos.x - Square.x;
                    UsePos.x = Square.x;
                }
            }
            //右
            else if (UsePos.y < Square.y && UsePos.x == Square.x)
            {
                UsePos.y += wide;

                if (UsePos.y > Square.y)
                {
                    UsePos.x -= UsePos.y - Square.y;
                    UsePos.y = Square.y;
                }
            }
            //上
            else if (UsePos.x > StartPos.x && UsePos.y == Square.y)
            {
                UsePos.x -= wide;

                if (UsePos.x < StartPos.x)
                {
                    UsePos.y -= StartPos.x - UsePos.x;
                    UsePos.x = StartPos.x;
                }
            }
            //左
            else if (UsePos.y > StartPos.y && UsePos.x == StartPos.x)
            {
                UsePos.y -= wide;

                if (UsePos.y < StartPos.y)
                {
                    UsePos.x -= StartPos.y - UsePos.y;
                    UsePos.y = StartPos.y;
                }
            }

            //修正値代入用
            inUsePos = UsePos;

            //startPosから見て左下にいた時
            if (checkPos == 1)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.y = -UsePos.y;
                inUsePos += StartPos * 2;
            }
            //startPosから見て左上にいた時
            else if (checkPos == 2)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.x += StartPos.x * 2;
            }
            //startPosから見て右下にいた時
            else if (checkPos == 3)
            {
                inUsePos.y = -UsePos.y;
                inUsePos.y += StartPos.y * 2;
            }

            //ドットの描画
            cloneDot.Add(Instantiate(managerAccessor.Instance.objDataManager.dotObj));
            cloneDot[i].transform.position = inUsePos;
        }
    }



    //今どの向きの縁を触っているかチェックする関数
    private void CheckRangeSize()
    {
        //マウスのワールド座標
        Vector2 mousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
        //過度の判定とる用
        bool[] rangeChecks = new bool[4];

        //クリックされているときは、押されている場所を変更しない
        if (!Input.GetMouseButton(0))
        {
            //押された場所の初期化
            onPos = (int)ChangeSizePosName.NONE;
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                rangeChecks[i] = false;
            }

            //下判定
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeStartPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.DOWN;
                rangeChecks[(int)ChangeSizePosName.DOWN] = true;
            }
            //右判定
            if (judgeEndPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.RIGHT;
                rangeChecks[(int)ChangeSizePosName.RIGHT] = true;
            }
            //上判定
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeEndPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.UP;
                rangeChecks[(int)ChangeSizePosName.UP] = true;
            }
            //左判定
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeStartPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.LEFT;
                rangeChecks[(int)ChangeSizePosName.LEFT] = true;
            }

            //右下判定
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_DOWN;
            }
            //右上判定
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_UP;
            }
            //左上判定
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_UP;
            }
            //左下判定
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_DOWN;
            }
        }

        

        //ドットの枠の上にカーソルがあるかどうか
        if (rangeChecks[(int)ChangeSizePosName.DOWN] || rangeChecks[(int)ChangeSizePosName.RIGHT] ||
            rangeChecks[(int)ChangeSizePosName.UP] || rangeChecks[(int)ChangeSizePosName.LEFT])
        {
            onEdge = true;
        }
        else
        {
            //マウスが押されているときは変更しない
            if (!Input.GetMouseButton(0))
                onEdge = false;

        }



    }



    private void BlockSizeChange(List<Vector3> MemSize, Vector3 MovePower)
    {
        if (MemSize.Count != 0)
        {
            //選択されているオブジェクトに加算
            for (int i = 0; i < Objs.Count; i++)
                Objs[i].transform.localScale = MemSize[i] + MovePower;
            
        }
    }

}
