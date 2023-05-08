using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageChangeArea : MonoBehaviour
{
    [SerializeField, Header("�؂�ւ������I�u�W�F�N�g")] private List<GameObject> stage;


    private void Start()
    {
        //�X�e�[�W����1�ڂ̃^�u�̂ݕ\��
        for (int i = 1; i < stage.Count; i++) 
        {
            stage[i].SetActive(false);
            transform.GetChild(i).gameObject.GetComponent<TabButton>().selectPanel.SetActive(false);
        }
    }


    public void ChangeTab(int num)
    {
        //�ړ����͕ύX�ł��Ȃ�
        if (!managerAccessor.Instance.dataMagager.isMoving)
        {
            //��U�^�u���\��
            for (int i = 0; i < stage.Count; i++)
            {
                stage[i].SetActive(false);
                transform.GetChild(i).gameObject.GetComponent<TabButton>().selectPanel.SetActive(false);
            }

            //�����ꂽ�^�u�̕\��
            stage[num - 1].SetActive(true);
            transform.GetChild(num - 1).gameObject.GetComponent<TabButton>().selectPanel.SetActive(true);
        }
    }


}
