using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    [SerializeField, Header("ホームウィンドウ")] private GameObject homeWindow;

    //シャットダウンなどを表示非表示させる関数
    public void WindowButton()
    {
        homeWindow.SetActive(!homeWindow.activeSelf);
    }

    //ゲームを終了させる関数
    public void ShutdownButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }
}
