using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        managerAccessor.Instance.sceneMoveManager = this;
    }

    //名前から参照してシーン移動
    public void SceneMoveName(string name)
    {
        SceneManager.LoadScene(name);
    }



}
