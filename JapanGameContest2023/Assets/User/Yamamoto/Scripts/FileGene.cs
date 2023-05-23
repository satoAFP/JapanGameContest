using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGene : MonoBehaviour
{

    public int playercount = 0;//プレイヤーの数を数える（ファイル削除命令用）

    public bool posupdate = false;//プレイヤーにクリックした座標に移動させる命令を送るフラグ

    [SerializeField, Header("生成する移動指標オブジェクト")]
    private GameObject prefab;

    [System.NonSerialized]
    public GameObject CreateObj;//移動指標オブジェクトを入れる（削除命令に使う）

    [SerializeField] private AudioClip appse;//プレイヤー登場SE（ここで書く理由は、プレイヤー登場SEを重なって鳴らさないようにするため）

    private AudioSource audioSource;

    private bool oneshot = false;//一回だけ音を鳴らす

    // Start is called before the first frame update
    void Start()
    {
        playercount = GameObject.FindGameObjectsWithTag("Player").Length;
        audioSource = GetComponent<AudioSource>();//スクリプト取得
    }

    // Update is called once per frame
    void Update()
    {
       
        if(managerAccessor.Instance.sceneMoveManager.GetSceneName() == "Stage1")
        {
            //ステージ1ではないとき
        }
        else if(!oneshot)
        {
            audioSource.PlayOneShot(appse);//プレイヤー登場SE鳴らす
            oneshot = true;
        }

        if (managerAccessor.Instance.dataMagager.playMode)//操作モードの時
        {
            //ゲームオーバーになっていなければ処理実行
            if (!managerAccessor.Instance.dataMagager.fallDeth ||
           !managerAccessor.Instance.dataMagager.infectionDeth ||
           !managerAccessor.Instance.dataMagager.overDeth)
            {
                //Decoyファイルにふれたときおとりファイルを消去
                if (managerAccessor.Instance.dataMagager.onDecoyFile)
                {
                    Destroy(CreateObj);
                }

                if (!managerAccessor.Instance.dataMagager.noTapArea)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // クリックされた位置を取得
                        managerAccessor.Instance.dataMagager.clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        managerAccessor.Instance.dataMagager.clickPosition.z = 0; // z座標を0に設定（2Dゲームなので）


                        posupdate = true;//Player側にクリックした座標に移動する命令を出す

                        if (CreateObj == null) //初めて移動指標オブジェクトを作るとき
                        {

                            CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//移動指標オブジェクト作成
                        }
                        else
                        {
                            //すでに移動指標オブジェクト作成が作られている場合
                            Destroy(CreateObj);//前回作ったものを削除
                            CreateObj = Instantiate(prefab, managerAccessor.Instance.dataMagager.clickPosition, Quaternion.identity);//新たに移動指標オブジェクト作成
                        }




                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        posupdate = false;
                    }
                }



                if (playercount == 0)//移動しているプレイヤーが0になるとCreateObj削除
                {
                    Destroy(CreateObj);
                    managerAccessor.Instance.dataMagager.isMoving = false;//プレイヤー全体の移動処理を終了
                    playercount = GameObject.FindGameObjectsWithTag("Player").Length;//プレイヤーの数を再カウント
                }
            }

        }

        　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
    }
}
