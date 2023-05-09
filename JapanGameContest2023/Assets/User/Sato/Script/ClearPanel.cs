using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W���\���p�e�L�X�g")] private Text stageText;

    [SerializeField, Header("�N���A�^�C���\���p�e�L�X�g")] private Text timeText;


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
