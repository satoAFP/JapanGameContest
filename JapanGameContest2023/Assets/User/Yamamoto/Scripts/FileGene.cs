using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    // クリックされた位置
    private Vector3 clickPosition;

    [SerializeField, Header("生成する移動指標オブジェクト")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//移動指標オブジェクトを入れる（削除命令に使う）

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // クリックされた位置を取得
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // z座標を0に設定（2Dゲームなので）
    }
}
