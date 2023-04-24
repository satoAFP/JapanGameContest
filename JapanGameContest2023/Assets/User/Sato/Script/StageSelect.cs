using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W�z�u���̐e�I�u�W�F�N�g")] private GameObject stageParent;

    [SerializeField, Header("�_�u���N���b�N����Ԋu����")] private int clickFrameRate;

    private List<GameObject> stages = new List<GameObject>();   //�X�e�[�W�L���p
    private int frameCount = 0;                                 //�_�u���N���b�N�̊Ԋu���J�E���g
    private bool oneClick = false;                            �@//���ڃN���b�N���ꂽ����
    private bool doubleClick = false;                           //���ڃN���b�N���ꂽ����

    //�ŏ��̈�񂾂�����
    private bool first1 = true;

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�̐���
        for (int i = 0; i < managerAccessor.Instance.dataMagager.stageNum; i++) 
        {
            stages.Add(Instantiate(managerAccessor.Instance.objDataManager.stageSelectObj));
            stages[i].transform.parent = stageParent.transform;
            stages[i].transform.GetChild(0).GetComponent<Text>().text = "STAGE" + (i + 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�_�u���N���b�N
        if (Input.GetMouseButton(0))
        {
            //�������͔������Ȃ�
            if (first1)
            {
                //���N���b�N���ꂽ��
                if(oneClick)
                {
                    doubleClick = true;
                }
                oneClick = true;
                first1 = false;
            }
        }
        else
        {
            first1 = true;
        }

        //�_�u���N���b�N�̔��������܂ł̎��Ԍv������
        if(oneClick)
        {
            if (clickFrameRate == frameCount) 
            {
                oneClick = false;
                frameCount = 0;
            }

            frameCount++;
        }

        //�_�u���N���b�N�ɐ��������Ƃ�
        if (doubleClick)
        {
            for (int i = 0; i < stages.Count; i++)
            {

                RectTransform stageNum = stages[i].GetComponent<RectTransform>();
                //�}�E�X�����W���ɂ���Ƃ�
                if (stageNum.position.x - stageNum.sizeDelta.x + 60 < Input.mousePosition.x &&
                    stageNum.position.x + stageNum.sizeDelta.x - 60 > Input.mousePosition.x &&
                    stageNum.position.y - stageNum.sizeDelta.y + 40 < Input.mousePosition.y &&
                    stageNum.position.y + stageNum.sizeDelta.y - 60 > Input.mousePosition.y)
                {
                    //�X�e�[�W�ړ�
                    managerAccessor.Instance.sceneMoveManager.SceneMoveName("Stage" + (i + 1));
                }
            }
        }
    }
}
