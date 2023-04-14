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
    private Vector2 backUpSquare/* = new Vector2(0, 0)*/;   //ドットの四角のバックアップデータ

    private List<GameObject> cloneDot = new List<GameObject>();//ドット格納用
    private Vector2 startPos;                           //クリックしたときの初期位置
    private Vector2 usePos;                             //初期位置代入
    [SerializeField]private Vector2 square;                             //四角の縦横の長さ
    private int checkPos = 0;                           //クリック後、startPosを原点に縦横それぞれの位置チェック用
    private int dotNum;                                 //ドットの数格納用


    // Update is called once per frame
    void FixedUpdate()
    {

        //キャラを操作中は選択できない
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //選択されてるオブジェクトが格納される
            List<GameObject> Objs = managerAccessor.Instance.dataMagager.selectObjsData;

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
                    Destroy(cloneDot[i]);
                cloneDot.Clear();

                //startPosから見て左下にいた時
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 1;
                //startPosから見て左上にいた時
                else if (dataManager.MouseWorldChange().x < startPos.x)
                    checkPos = 2;
                //startPosから見て右下にいた時
                else if (dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 3;

                //ドットの描画
                DotDraw();


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
                backUpSquare = square;
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
            //初期位置代入
            //usePos = startPos;
            //クリック後、startPosを原点に縦横それぞれの位置チェック用
            //checkPos = 0;

            //四角のサイズ計算
            //square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
            //square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

            

            if (onPos == (int)ChangeSizePosName.DOWN) 
            {
                startPos.y = dataManager.MouseWorldChange().y - onStartPos.y;
                square.y = backUpSquare.y + Mathf.Abs(dataManager.MouseWorldChange().y - onStartPos.y);
            }

            //ドットを打つ数を計算
            dotNum = (int)((square.x * 2 + square.y * 2) / wide);
            //初期位置分ずらす
            //square = startPos;

            //ドットの初期化
            for (int i = 0; i < cloneDot.Count; i++)
                Destroy(cloneDot[i]);
            cloneDot.Clear();

            

            //ドットの描画
            DotDraw();


            //DotDraw関数の引数設定しろ！


            first3 = true;
        }
    }


    //ドット描画用関数
    private void DotDraw()
    {
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
            cloneDot.Add(Instantiate(managerAccessor.Instance.objDataManager.dotObj));
            cloneDot[i].transform.position = inUsePos;
        }
    }


    //今どの向きの縁を触っているかチェックする関数
    private void CheckRangeSize()
    {
        Vector2 mousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

        onPos = (int)ChangeSizePosName.NONE;
        bool[] rangeChecks = new bool[4];

        for (int i = 0; i < rangeChecks.Length; i++)
            rangeChecks[i] = false;

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
        if(rangeChecks[(int)ChangeSizePosName.DOWN]&& rangeChecks[(int)ChangeSizePosName.RIGHT])
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

        //ドットの枠の上にカーソルがあるかどうか
        if (rangeChecks[(int)ChangeSizePosName.DOWN] || rangeChecks[(int)ChangeSizePosName.RIGHT] ||
            rangeChecks[(int)ChangeSizePosName.UP] || rangeChecks[(int)ChangeSizePosName.LEFT])
            onEdge = true;
        else
            onEdge = false;



    }

}
