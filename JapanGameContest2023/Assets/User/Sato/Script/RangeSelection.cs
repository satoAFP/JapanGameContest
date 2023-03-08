using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    [SerializeField, Header("範囲選択表示用オブジェクト")]
    private GameObject selectionObj;

    private bool first = true;
    private bool first2 = true;
    private Vector3 clickStartPos;
    private GameObject clone;
    private bool selectionMode = false;
    private bool editMode = false;
    private Vector3 beforePos = new Vector3(0, 0, 0);


    // Update is called once per frame
    void FixedUpdate()
    {
        List<GameObject> Objs = managerAccessor.Instance.dataMagager.selectObjs;

        if (!selectionMode)
        {
            for (int i = 0; i < Objs.Count; i++)
            {
                Vector3 nowMousePos = MouseWorldChange();
                if (Objs[i].transform.localPosition.x - Objs[i].transform.localScale.x < nowMousePos.x &&
                    Objs[i].transform.localPosition.x + Objs[i].transform.localScale.x > nowMousePos.x &&
                    Objs[i].transform.localPosition.y - Objs[i].transform.localScale.x < nowMousePos.y &&
                    Objs[i].transform.localPosition.y + Objs[i].transform.localScale.x > nowMousePos.y)
                {
                    Debug.Log("aaa");
                    editMode = true;
                    break;
                }
                else
                    editMode = false;
            }
        }

        if (editMode)
        {
            if (Input.GetMouseButton(0))
            {
                if (first2)
                {
                    //長押し状態を解除するまではいらないようにする
                    first2 = false;
                }
                else
                {
                    Vector3 movePower = MouseWorldChange() - beforePos;
                    
                    for (int i = 0; i < Objs.Count; i++)
                    {
                        Objs[i].transform.localPosition += movePower;
                    }
                }

                editMode = true;
                beforePos = MouseWorldChange();
            }
        }
        else
            first2 = true;

        if (!editMode)
            SelectObj();

        

    }


    //オブジェクト選択範囲表示関数
    private void SelectObj()
    {
        //長押しで選択範囲表示
        if (Input.GetMouseButton(0))
        {
            if (first)
            {
                //選択されているオブジェクトデータの削除
                managerAccessor.Instance.dataMagager.selectObjs.Clear();

                //選択開始時の初期位置記憶
                clickStartPos = Input.mousePosition;

                //範囲選択用オブジェクトの初期座標設定
                clone = Instantiate(selectionObj);
                clone.transform.localPosition = MouseWorldChange();

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

            //選択外
            selectionMode = false;
        }
    }

    //マウス座標をワールド座標変換関数
    private Vector3 MouseWorldChange()
    {
        //選択開始時の初期位置記憶
        Vector3 mousePos = Input.mousePosition;
        // Z軸修正
        mousePos.z = 10f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return screenToWorldPointPosition;
    }


}
