using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("ホームウィンドウ")] private GameObject homeWindow;

    [SerializeField, Header("ホームウィンドウ")] private GameObject noTapArea;

    [SerializeField, Header("シャットダウン後の画像")] private GameObject endImg;

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
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        yield return new WaitForSeconds(managerAccessor.Instance.dataMagager.loadTime);
        endImg.SetActive(true);
        yield return new WaitForSeconds(3.0f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }
}
