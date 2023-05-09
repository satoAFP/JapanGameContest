using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour
{
    [SerializeField, Header("ステージ数表示用テキスト")] private Text stageText;

    [SerializeField, Header("クリアタイム表示用テキスト")] private Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        stageText.text = " " + SceneManager.GetActiveScene().name;
        timeText.text = managerAccessor.Instance.dataMagager.timeText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
