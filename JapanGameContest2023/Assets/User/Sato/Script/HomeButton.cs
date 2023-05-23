using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("ホームウィンドウ")] private GameObject homeWindow;

    [SerializeField, Header("ホームウィンドウ")] private GameObject noTapArea;

    [SerializeField, Header("シャットダウン後の画像")] private GameObject endImg;

    [SerializeField, Header("ロードの画像")] private GameObject loadImg;

    [SerializeField, Header("UserNameText")] private Text userNameText;


    private void FixedUpdate()
    {
        //シャットダウンするときロード画像が回転する
        if(managerAccessor.Instance.dataMagager.sceneMoveStart)
        {
            loadImg.GetComponent<RectTransform>().eulerAngles += new Vector3(0, 0, 2);
        }

        userNameText.text = PlayerPrefs.GetString("userName", "");
    }

    //シャットダウンなどを表示非表示させる関数
    public void WindowButton()
    {
        homeWindow.SetActive(!homeWindow.activeSelf);
        noTapArea.SetActive(!noTapArea.activeSelf);
    }

    //ゲームを終了させる関数
    public void ShutdownButton()
    {
        StartCoroutine("CShutDown");
    }

    private IEnumerator CShutDown()
    {
        //ロードアニメーション再生
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        //シャットダウン画像表示
        endImg.SetActive(true);
        managerAccessor.Instance.dataMagager.isShutDown = true;
        yield return new WaitForSeconds(1.5f);

        //ゲームプレイ終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
