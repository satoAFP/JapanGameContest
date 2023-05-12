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

    //コルーチン呼び出し用
    public void SceneMoveName(string name)
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine(CSceneMoveName(name));
    }

    //名前から参照してシーン移動
    private IEnumerator CSceneMoveName(string name)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }

    //コルーチン呼び出し用
    public void SceneMoveNext()
    {
        managerAccessor.Instance.dataMagager.sceneMoveStart = true;
        StartCoroutine("CSceneMoveNext");
    }

    //次のステージにシーン移動
    private IEnumerator CSceneMoveNext()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            if (SceneManager.GetActiveScene().name == "Stage" + (i + 1))
            {
                if (SceneManager.GetActiveScene().name != "Stage" + managerAccessor.Instance.dataMagager.stageNum)
                {
                    SceneManager.LoadScene("Stage" + (i + 2));
                }
                else
                {
                    SceneManager.LoadScene("StageSelect");
                }
            }
        }
    }

    //リロード処理
    public void SceneMoveRetry()
    {
        StartCoroutine("CSceneMoveRetry");
    }

    private IEnumerator CSceneMoveRetry()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //シーンの名前取得関数
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


}
