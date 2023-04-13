using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotLine : MonoBehaviour
{
    [SerializeField, Header("�͂��悤�̃h�b�g")] private GameObject dotObj;

    [SerializeField, Header("�h�b�g�`�掞�̊Ԋu")] private float wide;


    //�h�b�g�i�[�p
    private List<GameObject> clone = new List<GameObject>();
    //�N���b�N�����Ƃ��̏����ʒu
    private Vector2 startPos;
    //�l�p�̏c���̒���
    private Vector2 square;
    //�h�b�g�̐��i�[�p
    private int dotNum;


    //�ŏ��̈�񂵂��ʂ�Ȃ������p
    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //�I���J�n����1�t���[���ڂ̃}�E�X�̍��W�擾
            if (Input.GetMouseButton(0))
            {
                if (first)
                {
                    startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    first = false;
                }
            }
            else
                first = true;

            //�͈͑I������
            if (Input.GetMouseButton(0))
            {
                //DataManager�擾
                DataManager dataManager = managerAccessor.Instance.dataMagager;
                //�����ʒu���
                Vector2 usePos = startPos;
                //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
                int checkPos = 0;

                //�l�p�̃T�C�Y�v�Z
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //�h�b�g��ł����v�Z
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //�����ʒu�����炷
                square += startPos;

                //�h�b�g�̏�����
                for (int i = 0; i < clone.Count; i++)
                    Destroy(clone[i]);
                clone.Clear();

                //startPos���猩�č����ɂ�����
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 1;
                //startPos���猩�č���ɂ�����
                else if (dataManager.MouseWorldChange().x < startPos.x)
                    checkPos = 2;
                //startPos���猩�ĉE���ɂ�����
                else if (dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 3;

                //�h�b�g��`��
                for (int i = 0; i < dotNum; i++)
                {
                    //��
                    if (usePos.x < square.x && usePos.y == startPos.y)
                    {
                        //�h�b�g��ł��W�����炷
                        usePos.x += wide;

                        //�����l�p����͂ݏo�����ꍇ�A�o���������̕`��ʒu�ɏC������
                        if (usePos.x > square.x)
                        {
                            usePos.y += usePos.x - square.x;
                            usePos.x = square.x;
                        }
                    }
                    //�E
                    else if (usePos.y < square.y && usePos.x == square.x)
                    {
                        usePos.y += wide;

                        if (usePos.y > square.y)
                        {
                            usePos.x -= usePos.y - square.y;
                            usePos.y = square.y;
                        }
                    }
                    //��
                    else if (usePos.x > startPos.x && usePos.y == square.y)
                    {
                        usePos.x -= wide;

                        if (usePos.x < startPos.x)
                        {
                            usePos.y -= startPos.x - usePos.x;
                            usePos.x = startPos.x;
                        }
                    }
                    //��
                    else if (usePos.y > startPos.y && usePos.x == startPos.x)
                    {
                        usePos.y -= wide;

                        if (usePos.y < startPos.y)
                        {
                            usePos.x -= startPos.y - usePos.y;
                            usePos.y = startPos.y;
                        }
                    }

                    //�C���l����p
                    Vector2 inUsePos = usePos;

                    //startPos���猩�č����ɂ�����
                    if (checkPos == 1)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.y = -usePos.y;
                        inUsePos += startPos * 2;
                    }
                    //startPos���猩�č���ɂ�����
                    else if (checkPos == 2)
                    {
                        inUsePos.x = -usePos.x;
                        inUsePos.x += startPos.x * 2;
                    }
                    //startPos���猩�ĉE���ɂ�����
                    else if (checkPos == 3)
                    {
                        inUsePos.y = -usePos.y;
                        inUsePos.y += startPos.y * 2;
                    }

                    //�h�b�g�̕`��
                    clone.Add(Instantiate(dotObj));
                    clone[i].transform.position = inUsePos;
                }

            }
        }
    }
}
