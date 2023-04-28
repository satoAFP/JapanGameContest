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

    //次のステージにシーン移動
    public void SceneMoveNext()
    {
        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            if (SceneManager.GetActiveScene().name == "Stage" + (i + 1))
            {
                SceneManager.LoadScene("Stage" + (i + 2));
            }
        }
    }

    //リロード処理
    public void SceneMoveRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
