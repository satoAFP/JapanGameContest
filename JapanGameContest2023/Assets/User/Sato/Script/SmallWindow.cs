using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallWindow : MonoBehaviour
{
    [SerializeField, Header("SmallWindow入れる")] private GameObject smallWindow;
    [SerializeField, Header("NoTapArea入れる")] private GameObject noTapArea;
    [SerializeField, Header("StageName入れる")] private Text stageName;
    [SerializeField, Header("スクショした画像を格納するオブジェクト")] private Image gameImg;


    private bool isScreenShot = false;//スクリーンショットをしたかどうか
    private bool isOnTab = false;//カーソルがタブの上に乗っているとき

    //最初しか通らない
    private bool first = true;


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
                //自身の親の親のオブジェクトに格納されているステージのSetActiveがtrueの時スクショが出来る
                if (transform.parent.parent.GetComponent<PageChangeArea>().stage[transform.parent.GetComponent<TabButton>().number].activeSelf)
                {
#if UNITY_EDITOR
                    CaptureScreenShot("Assets/Resources/" + transform.parent.name + ".png");
#else
                    CaptureScreenShot("Resources/" + transform.parent.name + ".png");
#endif

                    //画面のスクリーンショット
                    isScreenShot = true;
                }
                //スクショのタイムラグを待つ
                StartCoroutine("ScreenShotWait");

                //SmallWindowのタブの名前を変える
                stageName.text = managerAccessor.Instance.sceneMoveManager.GetSceneName() + "-" + (transform.parent.GetComponent<TabButton>().number + 1);

                //カーソルがタブに乗っているとき
                isOnTab = true;
                first = false;
            }

        }
        else
        {
            //必要な情報の取得
            Vector2 npos = noTapArea.GetComponent<RectTransform>().position;
            Vector2 nsize = noTapArea.GetComponent<RectTransform>().sizeDelta;

            //タブにカーソルが乗っていたとき&&NoTapAreaにカーソルが乗ってるいるとき
            if (npos.x - (nsize.x / 2) < mouse.x && npos.x + (nsize.x / 2) > mouse.x &&
                npos.y - (nsize.y / 2) < mouse.y && npos.y + (nsize.y / 2) > mouse.y && isOnTab) 
            {
                //ボタンが正しく動作した後
                if(transform.parent.GetComponent<TabButton>().isPutButton)
                {
                    //SmallWindowを消す
                    transform.parent.GetComponent<TabButton>().isPutButton = false;
                    noTapArea.SetActive(false);
                    smallWindow.SetActive(false);
                    first = true;
                    isOnTab = false;
                }
            }
            else
            {
                smallWindow.SetActive(false);
                noTapArea.SetActive(false);
                first = true;
                isOnTab = false;
            }
        }
    }



    //画面全体のスクリーンショットを保存する
    //引数1 保存する階層と名前
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath);
    }

    //ファイルの画像を参照し、オブジェクトに貼り付ける関数
    //引数1 参照する階層と画像の名前
    private void ImgPaste(string path)
    {
        //画像の表示処理
        byte[] data = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data);
        gameImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    //スクリーンショットの処理をいったん待つための関数
    private IEnumerator ScreenShotWait()
    {
        yield return new WaitForSeconds(0.1f);

        if (isScreenShot)
        {
#if UNITY_EDITOR
            ImgPaste("Assets/Resources/" + transform.parent.name + ".png");
#else
            ImgPaste("VIRUS PURGE_Data/Resources/" + transform.parent.name + ".png");
#endif
        }

        //SmallWindow表示
        smallWindow.SetActive(true);
        noTapArea.SetActive(true);
    }
}
