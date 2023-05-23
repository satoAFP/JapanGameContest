using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    //サイズ変更時持っている縁の位置の名前
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

    public enum MouseDirection
    {
        RIGHT_UP,
        LEFT_DOWN,
        LEFT_UP,
        RIGHT_DOWN,
        NONE,
    }

    [SerializeField, Header("編集モード切替")] private bool isEdit;

    [SerializeField, Header("ドット描画時の間隔")] private float wide;

    [SerializeField, Header("サイズ変更できる幅")] private Vector2 changeSizeWidth;

    //最初の一回しか通らない処理用
    private bool first = true;
    private bool first2 = true;
    private bool first3 = false;
    private bool first4 = true;
    private bool first5 = true;

    private List<GameObject> Objs=new List<GameObject>();//選択されているオブジェクト格納用
    private Vector3 clickStartPos;                      //クリック開始時の初期位置
    private GameObject clone;                           //選択範囲表示用オブジェクト
    private bool selectionMode = false;                 //オブジェクトを選択しているかどうか
    private bool editMode = false;                      //オブジェクトを選択しているかどうか
    private Vector3 beforePos = new Vector3(0, 0, 0);   //一フレーム前のマウスの位置
    private Vector2 judgeStartPos;                      //掴める範囲(左下)
    private Vector2 judgeEndPos;                        //掴める範囲(右上)
    private bool onEdge = false;                        //ドットの枠の縁に乗っているとき
    private int onPos = (int)ChangeSizePosName.NONE;    //今乗っている縁の場所(列挙参照)
    private Vector2 onStartPos;                         //拡大縮小時の初期位置
    private Vector2 backUpSquare;                       //ドットの四角のバックアップデータ
    private List<Vector3> memsize = new List<Vector3>();//選択されているオブジェクトのサイズデータ
    private List<Vector3> mempos = new List<Vector3>(); //選択されているオブジェクトの位置データ
    private Vector2 memjudgeStartPos;                   //選択されているオブジェクトの掴める範囲(左下)
    private Vector2 memjudgeEndPos;                     //選択されているオブジェクトの掴める範囲(右上)


    private Vector2 startPos;                           //クリックしたときの初期位置
    private Vector2 usePos;                             //初期位置代入
    private Vector2 square;                             //四角の縦横の長さ
    private int checkPos = 0;                           //クリック後、startPosを原点に縦横それぞれの位置チェック用
    private int dotNum;                                 //ドットの数格納用
    private Vector2 inUsePos;                           //usePosの修正値代入用

    //DataManagerでも使用
    [System.NonSerialized] public List<GameObject> cloneDot = new List<GameObject>();//ドット格納用


    // Update is called once per frame
    void FixedUpdate()
    {
        //キャラを操作中は選択できない
        if (!managerAccessor.Instance.dataMagager.playMode || isEdit) 
        {
            //選択されてるオブジェクトが格納される
            Objs = managerAccessor.Instance.dataMagager.selectObjsData;

            //オブジェクトが選択されている時
            if (!selectionMode)
            {
                //マウス座標をワールド座標に変換
                Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

                CheckRangeSize();


                if (Objs.Count == 0) 
                {
                    editMode = false;
                }
                else if (onEdge)
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
        else
        {
            //playmodeの時は強制的に枠線を消す
            if(cloneDot.Count!=0)
            {
                //ドットの初期化
                for (int i = 0; i < cloneDot.Count; i++)
                {
                    Destroy(cloneDot[i]);
                }
            }
            cloneDot.Clear();
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

                Debug.Log("aaa");
                //選択中
                selectionMode = true;
                //マウスの移動量で選択範囲算出
                Vector3 inputData = Input.mousePosition - clickStartPos;
                //移動量調整
                inputData.x /= 108;
                inputData.y /= 108;
                inputData.z /= 108;
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
                checkPos = (int)MouseDirection.RIGHT_UP;

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
                    checkPos = (int)MouseDirection.LEFT_DOWN;
                }
                //startPosから見て左上にいた時
                else if (dataManager.MouseWorldChange().x < startPos.x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
                //startPosから見て右下にいた時
                else if (dataManager.MouseWorldChange().y < startPos.y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }

                //ドットの描画
                DotDraw(startPos, usePos, square, dotNum);


                first3 = true;
            }
            else
            {
                //クリック終了時のマウスの座標取得
                if (first3)
                {
                    //右上
                    if (checkPos == (int)MouseDirection.RIGHT_UP)
                    {
                        judgeStartPos = startPos;
                        judgeEndPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    }
                    //左下
                    else if (checkPos == (int)MouseDirection.LEFT_DOWN)
                    {
                        judgeStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                        judgeEndPos = startPos;
                    }
                    //左上
                    else if (checkPos == (int)MouseDirection.LEFT_UP)
                    {
                        judgeStartPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeStartPos.y = startPos.y;
                        judgeEndPos.x = startPos.x;
                        judgeEndPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                    }
                    //右下
                    else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
                    {
                        judgeStartPos.x = startPos.x;
                        judgeStartPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                        judgeEndPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeEndPos.y = startPos.y;
                    }
                    first3 = false;
                }

                //オブジェクトが何も選択されていなかったとき
                if (Objs.Count == 0) 
                {
                    //ドットの初期化
                    for (int i = 0; i < cloneDot.Count; i++)
                    {
                        Destroy(cloneDot[i]);
                    }
                    cloneDot.Clear();

                    //画面外へ飛ばす
                    judgeStartPos = new Vector3(999, 999, 999);
                    judgeEndPos = new Vector3(999, 999, 999);
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
            //選択開始時記憶しておきたい変数
            if (first4)
            {
                onStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                startPos = judgeStartPos;
                backUpSquare.x = Mathf.Abs(judgeEndPos.x - judgeStartPos.x);
                backUpSquare.y = Mathf.Abs(judgeEndPos.y - judgeStartPos.y);
                memjudgeStartPos = judgeStartPos;
                memjudgeEndPos = judgeEndPos;
                memsize.Clear();
                mempos.Clear();
                for (int i = 0; i < Objs.Count; i++)
                {
                    memsize.Add(Objs[i].transform.localScale);
                    mempos.Add(Objs[i].transform.localPosition);
                }
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

            //描画の初期位置を左下に設定
            checkPos = (int)MouseDirection.RIGHT_UP;


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
                BlockRatioChange(new Vector3(0, -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);
                if (judgeEndPos.y < dataManager.MouseWorldChange().y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }
            }
            if (onPos == (int)ChangeSizePosName.RIGHT)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                judgeEndPos.x = setStartPos.x + square.x;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, 0, 0), onPos);

                if (judgeStartPos.x > dataManager.MouseWorldChange().x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
            }
            if (onPos == (int)ChangeSizePosName.UP)
            {
                square.x = backUpSquare.x;
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(0, dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                if (judgeStartPos.y > dataManager.MouseWorldChange().y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }
            }
            if (onPos == (int)ChangeSizePosName.LEFT)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), 0, 0), onPos);

                if (judgeEndPos.x < dataManager.MouseWorldChange().x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_DOWN)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeEndPos.x = setStartPos.x + square.x;
                judgeStartPos.y = setStartPos.y;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);

                checkPos = GetCheckPos(new Vector2(judgeStartPos.x, judgeEndPos.y), dataManager.MouseWorldChange());
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_UP)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.x = setStartPos.x + square.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                checkPos = GetCheckPos(new Vector2(judgeStartPos.x, -judgeStartPos.y), new Vector2(dataManager.MouseWorldChange().x, -dataManager.MouseWorldChange().y));
            }
            if (onPos == (int)ChangeSizePosName.LEFT_UP)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                checkPos = GetCheckPos(new Vector2(-judgeEndPos.x, -judgeStartPos.y), -dataManager.MouseWorldChange());
            }
            if (onPos == (int)ChangeSizePosName.LEFT_DOWN)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeStartPos = setStartPos;
                //サイズ変更時の移動量設定
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);

                checkPos = GetCheckPos(new Vector2(-judgeEndPos.x, judgeEndPos.y), new Vector2(-dataManager.MouseWorldChange().x, dataManager.MouseWorldChange().y));
            }

            square.x = Mathf.Abs(square.x);
            square.y = Mathf.Abs(square.y);

            //サイズ変更適用処理
            BlockSizeChange(new Vector3(square.x / backUpSquare.x, square.y / backUpSquare.y, 0));

            //描画位置設定用座標のスタート位置初期化
            usePos = setStartPos;


            //ドットを打つ数を計算
            dotNum = (int)((Mathf.Abs(square.x * 2) + Mathf.Abs(square.y * 2)) / wide);
            //初期位置分ずらす
            square += setStartPos;



            //ドットの初期化
            for (int i = 0; i < cloneDot.Count; i++)
                Destroy(cloneDot[i]);
            cloneDot.Clear();

            

            //ドットの描画
            DotDraw(setStartPos, usePos, square, dotNum);

            first5 = true;
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
            if (checkPos == (int)MouseDirection.LEFT_DOWN)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.y = -UsePos.y;
                inUsePos += StartPos * 2;
            }
            //startPosから見て左上にいた時
            else if (checkPos == (int)MouseDirection.LEFT_UP)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.x += StartPos.x * 2;
            }
            //startPosから見て右下にいた時
            else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
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
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.DOWN;
            }
            //右判定
            if (judgeEndPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.RIGHT;
                rangeChecks[(int)ChangeSizePosName.RIGHT] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT;
            }
            //上判定
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeEndPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.UP;
                rangeChecks[(int)ChangeSizePosName.UP] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.UP;
            }
            //左判定
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeStartPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.LEFT;
                rangeChecks[(int)ChangeSizePosName.LEFT] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT;
            }

            //右下判定
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_DOWN;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT_DOWN;
            }
            //右上判定
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_UP;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT_UP;
            }
            //左上判定
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_UP;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT_UP;
            }
            //左下判定
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_DOWN;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT_DOWN;
            }
        }

        

        //ドットの枠の上にカーソルがあるかどうか
        if (rangeChecks[(int)ChangeSizePosName.DOWN] || rangeChecks[(int)ChangeSizePosName.RIGHT] ||
            rangeChecks[(int)ChangeSizePosName.UP] || rangeChecks[(int)ChangeSizePosName.LEFT])
        {
            onEdge = true;
            managerAccessor.Instance.dataMagager.onEdge = true;
        }
        else
        {
            //マウスが押されているときは変更しない
            if (!Input.GetMouseButton(0))
            {
                onEdge = false;
                managerAccessor.Instance.dataMagager.onEdge = false;

                //ひっくり返った時四角の座標を変更
                if (first5)
                {
                    //それぞれのバックアップデータ
                    Vector2 _judgeStartPos = judgeStartPos;
                    Vector2 _judgeEndPos = judgeEndPos;


                    //左下
                    if (checkPos == (int)MouseDirection.LEFT_DOWN)
                    {
                        judgeStartPos = _judgeEndPos;
                        judgeEndPos = _judgeStartPos;
                    }
                    //左上
                    else if (checkPos == (int)MouseDirection.LEFT_UP)
                    {
                        judgeStartPos.x = _judgeEndPos.x;
                        judgeEndPos.x = _judgeStartPos.x;
                    }
                    //右下
                    else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
                    {
                        judgeStartPos.y = _judgeEndPos.y;
                        judgeEndPos.y = _judgeStartPos.y;
                    }

                    first5 = false;
                }
            }

        }



    }


    //ブロックのサイズ変更適用関数
    //引数1　ブロックのサイズ変更量
    private void BlockSizeChange(Vector3 MovePower)
    {
        //オブジェクトが選択されているとき
        if (memsize.Count != 0)
        {
            //選択されているオブジェクトに加算
            for (int i = 0; i < Objs.Count; i++)
            {
                //サイズを変える比率入力用
                Objs[i].transform.localScale = Mul(memsize[i], MovePower);
            }
            
        }
    }


    //サイズ変更時位置を比率に合わせる関数
    //引数1　初期位置から見たマウスの移動量
    //引数2　どの場所を持っているか
    private void BlockRatioChange(Vector3 MovePower, int OnPos)
    {
        //オブジェクトが選択されているとき
        if (mempos.Count != 0)
        {
            //選択されているオブジェクトに加算
            for (int i = 0; i < Objs.Count; i++)
            {
                //囲われている中での縁から見た比率
                Vector3 ratio = new Vector3(0, 0, 0);
                //座標入力用
                Vector3 input = new Vector3(0, 0, 0);

                //それぞれ触れらてるいる場所
                switch (OnPos)
                {
                    case (int)ChangeSizePosName.DOWN:
                        //比率の計算
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        //比率分入力
                        Objs[i].transform.localPosition = mempos[i] - Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.RIGHT:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        Objs[i].transform.localPosition = mempos[i] + Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.UP:
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        Objs[i].transform.localPosition = mempos[i] + Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.LEFT:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        Objs[i].transform.localPosition = mempos[i] - Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.RIGHT_DOWN:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        input.x = mempos[i].x + Mul(MovePower, ratio).x;
                        input.y = mempos[i].y - Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.RIGHT_UP:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        input.x = mempos[i].x + Mul(MovePower, ratio).x;
                        input.y = mempos[i].y + Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.LEFT_UP:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        input.x = mempos[i].x - Mul(MovePower, ratio).x;
                        input.y = mempos[i].y + Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.LEFT_DOWN:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        input.x = mempos[i].x - Mul(MovePower, ratio).x;
                        input.y = mempos[i].y - Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                }
            }
        }
    }


    //原点から見てマウスが何処にいるかチェックする関数
    //引数1　原点
    //引数2　マウスの座標
    //戻り値　現在のマウスの方向
    private int GetCheckPos(Vector2 origin,Vector2 mouse)
    {
        int checkpos = 0;

        //原点から見て右上にいるとき
        if (origin.x < mouse.x && origin.y < mouse.y) 
        {
            checkpos = (int)MouseDirection.RIGHT_DOWN;
        }
        //原点から見て左下にいるとき
        else if (origin.x > mouse.x && origin.y > mouse.y)
        {
            checkpos = (int)MouseDirection.LEFT_UP;
        }
        //原点から見て左上にいるとき
        else if (origin.x > mouse.x && origin.y < mouse.y)
        {
            checkpos = (int)MouseDirection.LEFT_DOWN;
        }
        //原点から見て右下にいるとき
        else if (origin.x < mouse.x && origin.y > mouse.y)
        {
            checkpos = (int)MouseDirection.RIGHT_UP;
        }

        return checkpos;
    }


    //vector3同士の掛け算関数
    private Vector3 Mul(Vector3 a,Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

}
