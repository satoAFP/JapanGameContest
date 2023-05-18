using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow : MonoBehaviour
{
    [SerializeField, Header("SmallWindow入れる")] private GameObject smallWindow;
    [SerializeField, Header("SmallWindow入れる")] private Image gameImg;

    private RectTransform rt;

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //必要な情報の取得
        Vector2 pos = gameObject.GetComponent<RectTransform>().position;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 mouse = Input.mousePosition;

        //マウスが座標内にいるとき
        if (pos.x - (size.x / 2) < mouse.x && pos.x + (size.x / 2) > mouse.x &&
            pos.y - (size.y / 2) < mouse.y && pos.y + (size.y / 2) > mouse.y)
        {
            if (first)
            {
                CaptureScreenShot("Assets/Resources/" + transform.parent.name + ".png");
                StartCoroutine("CSceneMoveRetry");
                smallWindow.SetActive(true);

                string path = "Assets/Resources/" + transform.parent.name + ".png";
                byte[] data = System.IO.File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                gameImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                first = false;
            }

        }
        else
        {
            smallWindow.SetActive(false);
            first = true;
        }
    }



    // 画面全体のスクリーンショットを保存する
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath);
    }


    private IEnumerator CSceneMoveRetry()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
