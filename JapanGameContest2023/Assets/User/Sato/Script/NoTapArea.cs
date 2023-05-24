using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTapArea : MonoBehaviour
{
    [SerializeField, Header("noTapArea")] private List<GameObject> noTapAreas;

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < noTapAreas.Count; i++)
        {
            //必要な情報の取得
            Vector2 pos = noTapAreas[i].GetComponent<RectTransform>().position;
            Vector2 size = noTapAreas[i].GetComponent<RectTransform>().sizeDelta;
            Vector2 mouse = Input.mousePosition;

            if (noTapAreas[i].activeSelf)
            {
                Debug.Log(managerAccessor.Instance.dataMagager.noTapArea);
                //オブジェクト内にカーソルが入っている時、切り替える
                if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
                    pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
                {
                    managerAccessor.Instance.dataMagager.noTapArea = true;
                    break;
                }
                else
                {
                    managerAccessor.Instance.dataMagager.noTapArea = false;
                }
            }
            else
            {
                managerAccessor.Instance.dataMagager.noTapArea = false;
            }
        }

    }
}
