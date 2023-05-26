using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    [SerializeField, Header("�G���[�E�B���h�E")] private GameObject errorWindow;

    [SerializeField, Header("�G���[�E�B���h�E���ʂɏo���^�C�~���O")] private int windowPopTime;

    private float countTime = 0.0f;                     //deltaTime�̐��l����p
    private int windowPopCount = 0;                     //�G���[�E�B���h�E���o���^�C�~���O
    private Vector3 windowPopPos = new Vector3(0, 0, 0);//�G���[�E�B���h�E���o�����W
    private DataManager dataManager;                    //dataManager�擾�p

    //1�x�������s���鏈���p
    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //dataManager�擾
        dataManager = managerAccessor.Instance.dataMagager;

        if (first)
        {
            //���Ԑ����̔����̎��Ԃ��擾
            windowPopCount = dataManager.stageTime / 2;
            first = false;
        }

        //���ԉ��Z
        countTime += Time.deltaTime;
        
        //�������Ԃ̔����̎��ԂɂȂ�ƃG���[�E�B���h�E���o���n�߂�
        if (dataManager.stageTime / 2 <= (int)countTime) 
        {
            //�G���[�E�B���h�E���o���^�C�~���O���Ǘ�
            if (windowPopCount <= (int)countTime)
            {
                //���߂�ꂽ���ԂɂȂ�܂�1�b���ɕ\��
                if (!(dataManager.stageTime - windowPopTime <= (int)countTime))
                {
                    windowPopCount++;
                    DuplicationErrorWindow(windowPopPos);
                }
                //���߂�ꂽ���ԂɂȂ��1�t���[����1���\��
                else
                {
                    DuplicationErrorWindow(windowPopPos);
                }
                //�\��������W�����炷
                windowPopPos += new Vector3(0.2f, 0.2f, 0);
            }
        }
        //�������ԂɂȂ�Ǝ��S����
        if (dataManager.stageTime <= (int)countTime) 
        {
            managerAccessor.Instance.dataMagager.playerlost = true;
            managerAccessor.Instance.dataMagager.timeDeth = true;
        }
    }

    /// <summary>
    /// �G���[�E�B���h�E�����p�֐�
    /// </summary>
    /// <param name="pos">����������W</param>
    private void DuplicationErrorWindow(Vector3 pos)
    {
        GameObject clone = Instantiate(errorWindow, gameObject.transform);
        clone.transform.localPosition = pos;
    }
}
