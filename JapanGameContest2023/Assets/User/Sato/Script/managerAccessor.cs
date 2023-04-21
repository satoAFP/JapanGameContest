using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class managerAccessor
{
    //シングルトンパターン
    private static managerAccessor instance = null;
    public static managerAccessor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new managerAccessor();
            }
            return instance;
        }
    }

    //マネージャーの参照
    public DataManager dataMagager;
    public ObjDataManager objDataManager;
    public SceneMoveManager sceneMoveManager;
}
