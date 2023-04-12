using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    [SerializeField, Header("範囲選択表示用オブジェクト")]
    private GameObject selectionObj;

    private bool first = true;
    private Vector3 clickStartPos;
    private GameObject clone;
    private bool selectionMode = false;
    private bool editMode = false;
    private Vector3 beforePos = new Vector3(0, 0, 0);


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
                for (int i = 0; i < Objs.Count; i++)
                {
                    //マウス座標をワールド座標に変換
                    Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    //選択されているオブジェクト内にカーソルがある場合
                    if (Objs[i].transform.localPosition.x - Objs[i].transform.localScale.x / 2 < nowMousePos.x &&
                        Objs[i].transform.localPosition.x + Objs[i].transform.localScale.x / 2 > nowMousePos.x &&
                        Objs[i].transform.localPosition.y - Objs[i].transform.localScale.x / 2 < nowMousePos.y &&
                        Objs[i].transform.localPosition.y + Objs[i].transform.localScale.x / 2 > nowMousePos.y)
                    {
                        //オブジェクト選択状態にする
                        editMode = true;
                        break;
                    }
                    else
                    {
                        //マウスがクリックされている状態の場合、選択状態を解除しない
                        if (!Input.GetMouseButton(0))
                            editMode = false;
                    }
                }
            }

            //オブジェクトが選択されている時
            if (editMode)
            {
                if (Input.GetMouseButton(0))
                {
                    //1フレーム前との誤差を算出
                    Vector3 movePower = managerAccessor.Instance.dataMagager.MouseWorldChange() - beforePos;

                    //選択されているオブジェクトに加算
                    for (int i = 0; i < Objs.Count; i++)
                    {
                        Objs[i].transform.localPosition += movePower;
                    }
                }
            }
            //オブジェクトが選択されていない時
            else
            {
                //オブジェクト選択範囲表示
                SelectObj();
            }
        }

        //マウスの座標を記憶
        beforePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }


    //オブジェクト選択範囲表示関数
    private void SelectObj()
    {
        GameObject selectUI = managerAccessor.Instance.dataMagager.rightClickUIClone;
        if (selectUI != null &&
            selectUI.GetComponent<RectTransform>().position.x + (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) > Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.x - (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) < Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.y + (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) > Input.mousePosition.y &&
            selectUI.GetComponent<RectTransform>().position.y - (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) < Input.mousePosition.y)
        {

        }
        else
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
                    clone = Instantiate(selectionObj);
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



}
