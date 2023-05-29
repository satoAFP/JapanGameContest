using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    [SerializeField, Header("�G���[�E�B���h�E")] private GameObject errorWindow;

    [SerializeField, Header("�G���[�E�B���h�E�\����SE")] private AudioClip errorWindowSE;

    [SerializeField, Header("�G���[�E�B���h�E�ŏ����o�����W")] private Vector3 windowPopPos;

    [SerializeField, Header("�G���[�E�B���h�E�o���Ԋu")] private int windowInterval;

    [SerializeField, Header("�G���[�E�B���h�E���ʂɏo���^�C�~���O")] private int windowPopTime;

    [SerializeField, Header("�G���[�E�B���h�E���ʂɏo�����̊Ԋu")] private int windowPopTimeInterval;

    [SerializeField, Header("�G���[�E�B���h�E�����炷����")] private Vector3 shiftPos;

    [SerializeField, Header("�G���[�E�B���h�E����ʂ���o���Ƃ����炷����")] private Vector3 outShiftPos;

    [SerializeField, Header("�G���[�E�B���h�E���؂�Ԃ�Y���W")] private float restartPosY;

    private float countTime = 0.0f;                     //deltaTime�̐��l����p
    private int windowPopCount = 0;                     //�G���[�E�B���h�E���o���^�C�~���O
    private DataManager dataManager;                    //dataManager�擾�p
    private GameObject clone;                           //�G���[�E�B���h�E�����p
    private AudioSource audio;                          //SE�Đ��p
    private int frameCount = 0;                         //�t���[���v���p

    //1�x�������s���鏈���p
    private bool first = true;

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

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

        //�L���������ʂƃG���[�E�B���h�E���o�Ȃ�
        if (!managerAccessor.Instance.dataMagager.playerlost)
        {
            //�������Ԃ̔����̎��ԂɂȂ�ƃG���[�E�B���h�E���o���n�߂�
            if (dataManager.stageTime / 2 <= (int)countTime)
            {
                //�G���[�E�B���h�E���o���^�C�~���O���Ǘ�
                if (windowPopCount <= (int)countTime)
                {
                    //���߂�ꂽ���ԂɂȂ�܂�1�b���ɕ\��
                    if (!(dataManager.stageTime - windowPopTime <= (int)countTime))
                    {
                        windowPopCount += windowInterval;
                        DuplicationErrorWindow(new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-4.0f, 4.0f)));
                    }
                    //���߂�ꂽ���ԂɂȂ��windowPopTimeInterval�t���[����1���\��
                    else
                    {
                        if (frameCount % windowPopTimeInterval == 0)
                        {
                            DuplicationErrorWindow(windowPopPos);

                            //�\��������W�����炷
                            windowPopPos += shiftPos;

                            //�܂�Ԃ�����
                            if (clone.transform.localPosition.y >= restartPosY)
                            {
                                windowPopPos -= outShiftPos;
                            }
                        }
                    }
                }
            }
            //�������ԂɂȂ�Ǝ��S����
            if (dataManager.stageTime <= (int)countTime)
            {
                managerAccessor.Instance.dataMagager.playerlost = true;
                managerAccessor.Instance.dataMagager.timeDeth = true;
            }

            //�t���[�����Z
            frameCount++;
        }
    }

    /// <summary>
    /// �G���[�E�B���h�E�����p�֐�
    /// </summary>
    /// <param name="pos">����������W</param>
    private void DuplicationErrorWindow(Vector3 pos)
    {
        clone = Instantiate(errorWindow, gameObject.transform);
        clone.transform.localPosition = pos;
        audio.PlayOneShot(errorWindowSE);
    }
}
