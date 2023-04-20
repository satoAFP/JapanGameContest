using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class managerAccessor
{
    //�V���O���g���p�^�[��
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

    //�}�l�[�W���[�̎Q��
    public DataManager dataMagager;
    public ObjDataManager objDataManager;
    public SceneMoveManager sceneMoveManager;
}
