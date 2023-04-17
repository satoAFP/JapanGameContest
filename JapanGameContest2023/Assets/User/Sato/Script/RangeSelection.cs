using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    public enum ChangeSizePosName
    {
        DOWN,
        RIGHT,
        UP,
        LEFT,
        RIGHT_DOWN,
        RIGHT_UP,
        LEFT_UP,
        LEFT_DOWN,
        NONE,
    }


    [SerializeField, Header("�h�b�g�`�掞�̊Ԋu")] private float wide;

    [SerializeField, Header("�T�C�Y�ύX�ł��镝")] private Vector2 changeSizeWidth;

    //�ŏ��̈�񂵂��ʂ�Ȃ������p
    private bool first = true;
    private bool first2 = true;
    private bool first3 = false;
    private bool first4 = true;

    private Vector3 clickStartPos;                      //�N���b�N�J�n���̏����ʒu
    private GameObject clone;                           //�I��͈͕\���p�I�u�W�F�N�g
    private bool selectionMode = false;                 //�I�u�W�F�N�g��I�����Ă��邩�ǂ���
    private bool editMode = false;                      //�I�u�W�F�N�g��I�����Ă��邩�ǂ���
    private Vector3 beforePos = new Vector3(0, 0, 0);   //��t���[���O�̃}�E�X�̈ʒu
    private Vector2 judgeStartPos;                      //�͂߂�͈�(����)
    private Vector2 judgeEndPos;                        //�͂߂�͈�(�E��)
    private bool onEdge = false;                        //�h�b�g�̘g�̉��ɏ���Ă���Ƃ�
    private int onPos = (int)ChangeSizePosName.NONE;    //������Ă��鉏�̏ꏊ(�񋓎Q��)
    private Vector2 onStartPos;                         //�g��k�����̏����ʒu
    private Vector2 backUpSquare/* = new Vector2(0, 0)*/;   //�h�b�g�̎l�p�̃o�b�N�A�b�v�f�[�^

    private List<GameObject> cloneDot = new List<GameObject>();//�h�b�g�i�[�p
    private Vector2 startPos;                           //�N���b�N�����Ƃ��̏����ʒu
    private Vector2 usePos;                             //�����ʒu���
    [SerializeField]private Vector2 square;                             //�l�p�̏c���̒���
    private int checkPos = 0;                           //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
    [SerializeField] private int dotNum;                                 //�h�b�g�̐��i�[�p
    private Vector2 inUsePos;                           //usePos�̏C���l����p


    // Update is called once per frame
    void FixedUpdate()
    {

        //�L�����𑀍쒆�͑I���ł��Ȃ�
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //�I������Ă�I�u�W�F�N�g���i�[�����
            List<GameObject> Objs = managerAccessor.Instance.dataMagager.selectObjsData;

            //�I�u�W�F�N�g���I������Ă��鎞
            if (!selectionMode)
            {
                //�}�E�X���W�����[���h���W�ɕϊ�
                Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

                CheckRangeSize();

                if (onEdge)
                {
                    //�I�u�W�F�N�g�I����Ԃɂ���
                    editMode = true;
                }
                //�h�b�g�̘g���̏ꍇ��������悤�ɂȂ�
                else if (judgeStartPos.x < nowMousePos.x && judgeEndPos.x > nowMousePos.x &&
                    judgeStartPos.y < nowMousePos.y && judgeEndPos.y > nowMousePos.y) 
                {
                    //�I�u�W�F�N�g�I����Ԃɂ���
                    editMode = true;
                }
                else
                {
                    //�}�E�X���N���b�N����Ă����Ԃ̏ꍇ�A�I����Ԃ��������Ȃ�
                    if (!Input.GetMouseButton(0))
                        editMode = false;
                }
            }

            //�I�u�W�F�N�g���I������Ă��鎞
            if (editMode)
            {
                if (onEdge)
                {
                    ChangeRangeSize();
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        //1�t���[���O�Ƃ̌덷���Z�o
                        Vector3 movePower = managerAccessor.Instance.dataMagager.MouseWorldChange() - beforePos;

                        //�I������Ă���I�u�W�F�N�g�ɉ��Z
                        for (int i = 0; i < Objs.Count; i++)
                            Objs[i].transform.localPosition += movePower;

                        //�h�b�g�̘g�ɂ��ړ��ʉ��Z
                        for (int i = 0; i < cloneDot.Count; i++)
                            cloneDot[i].transform.localPosition += movePower;

                        //�h�b�g�̘g���ړ��ʉ��Z
                        judgeStartPos.x += movePower.x;
                        judgeStartPos.y += movePower.y;
                        judgeEndPos.x += movePower.x;
                        judgeEndPos.y += movePower.y;

                    }
                }
            }
            //�I�u�W�F�N�g���I������Ă��Ȃ���
            else
            {
                //�I�u�W�F�N�g�I��͈͕\��
                SelectObj();
                //�h�b�g�̘g�\��
                Dotline();
            }
        }

        //�}�E�X�̍��W���L��
        beforePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
    }


    //�I�u�W�F�N�g�I��͈͕\���֐�
    private void SelectObj()
    {
        GameObject selectUI = managerAccessor.Instance.dataMagager.rightClickUIClone;
        if (!(selectUI != null &&
            selectUI.GetComponent<RectTransform>().position.x + (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) > Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.x - (selectUI.GetComponent<RectTransform>().sizeDelta.x / 2) < Input.mousePosition.x &&
            selectUI.GetComponent<RectTransform>().position.y + (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) > Input.mousePosition.y &&
            selectUI.GetComponent<RectTransform>().position.y - (selectUI.GetComponent<RectTransform>().sizeDelta.y / 2) < Input.mousePosition.y))
        {
            //�������őI��͈͕\��
            if (Input.GetMouseButton(0))
            {
                if (first)
                {
                    //�I������Ă���I�u�W�F�N�g�f�[�^�̍폜
                    managerAccessor.Instance.dataMagager.selectObjsData.Clear();

                    //�I���J�n���̏����ʒu�L��
                    clickStartPos = Input.mousePosition;

                    //�͈͑I��p�I�u�W�F�N�g�̏������W�ݒ�
                    clone = Instantiate(managerAccessor.Instance.objDataManager.selectionObj);
                    clone.transform.localPosition = managerAccessor.Instance.dataMagager.MouseWorldChange();

                    //��������Ԃ���������܂ł͂���Ȃ��悤�ɂ���
                    first = false;
                }

                //�I��
                selectionMode = true;
                //�}�E�X�̈ړ��ʂőI��͈͎Z�o
                Vector3 inputData = Input.mousePosition - clickStartPos;
                //�ړ��ʒ���
                inputData.x /= 107;
                inputData.y /= 107;
                inputData.z /= 107;
                //�I��͈͓���
                clone.transform.localScale = inputData;
            }
            else
            {
                //�폜
                Destroy(clone);
                first = true;

                managerAccessor.Instance.dataMagager.copyReset = true;

                //�I������Ă���Ƃ��̃I�u�W�F�N�g�ԍ����Z�b�g
                managerAccessor.Instance.dataMagager.objNum = 0;

                //�I���O
                selectionMode = false;
            }
        }
    }


    //�h�b�g�ʒu�v�Z�p�֐�
    private void Dotline()
    {
        if (!managerAccessor.Instance.dataMagager.playMode)
        {
            //�I���J�n����1�t���[���ڂ̃}�E�X�̍��W�擾
            if (Input.GetMouseButton(0))
            {
                if (first2)
                {
                    startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    first2 = false;
                }
            }
            else
                first2 = true;

            //�͈͑I������
            if (Input.GetMouseButton(0))
            {
                //DataManager�擾
                DataManager dataManager = managerAccessor.Instance.dataMagager;
                //�����ʒu���
                usePos = startPos;
                //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
                checkPos = 0;

                //�l�p�̃T�C�Y�v�Z
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //�h�b�g��ł����v�Z
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //�����ʒu�����炷
                square += startPos;

                //�h�b�g�̏�����
                for (int i = 0; i < cloneDot.Count; i++)
                    Destroy(cloneDot[i]);
                cloneDot.Clear();

                //startPos���猩�č����ɂ�����
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 1;
                //startPos���猩�č���ɂ�����
                else if (dataManager.MouseWorldChange().x < startPos.x)
                    checkPos = 2;
                //startPos���猩�ĉE���ɂ�����
                else if (dataManager.MouseWorldChange().y < startPos.y)
                    checkPos = 3;

                //�h�b�g�̕`��
                DotDraw(startPos, usePos, square, dotNum);


                first3 = true;
            }
            else
            {
                //�N���b�N�I�����̃}�E�X�̍��W�擾
                if(first3)
                {
                    //�E��
                    if (checkPos == 0)
                    {
                        judgeStartPos = startPos;
                        judgeEndPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    }
                    //����
                    else if (checkPos == 1)
                    {
                        judgeStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                        judgeEndPos = startPos;
                    }
                    //����
                    else if (checkPos == 2)
                    {
                        judgeStartPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeStartPos.y = startPos.y;
                        judgeEndPos.x = startPos.x;
                        judgeEndPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                    }
                    //�E��
                    else if (checkPos == 3)
                    {
                        judgeStartPos.x = startPos.x;
                        judgeStartPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                        judgeEndPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeEndPos.y = startPos.y;
                    }
                    first3 = false;
                }
            }
        }
    }


    //���̃T�C�Y�ύX�p�֐�
    private void ChangeRangeSize()
    {
        

        //�I���J�n����1�t���[���ڂ̃}�E�X�̍��W�擾
        if (Input.GetMouseButton(0))
        {
            if (first4)
            {
                onStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                backUpSquare = square;
                first4 = false;
            }
        }
        else
            first4 = true;

        //�͈͑I������
        if (Input.GetMouseButton(0))
        {
            //DataManager�擾
            DataManager dataManager = managerAccessor.Instance.dataMagager;
            //�����ʒu���
            //usePos = startPos;
            //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
            //checkPos = 0;

            //�l�p�̃T�C�Y�v�Z
            //square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
            //square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

            checkPos = 0;

            if (onPos == (int)ChangeSizePosName.DOWN) 
            {
                startPos.y = dataManager.MouseWorldChange().y - onStartPos.y;
                square.x = backUpSquare.x;
                square.y = backUpSquare.y + Mathf.Abs(dataManager.MouseWorldChange().y - onStartPos.y);
            }

            Vector2 setStartPos = startPos;
            setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;

            //�h�b�g��ł����v�Z
            dotNum = (int)((square.x * 2 + square.y * 2) / wide);
            //�����ʒu�����炷
            square += setStartPos;

            //�h�b�g�̏�����
            for (int i = 0; i < cloneDot.Count; i++)
                Destroy(cloneDot[i]);
            cloneDot.Clear();

            

            //�h�b�g�̕`��
            DotDraw(setStartPos, usePos, square, dotNum);


            //DotDraw�֐��̈����ݒ肵��I


            first3 = true;
        }
    }


    //�h�b�g�`��p�֐�
    private void DotDraw(Vector2 StartPos, Vector2 UsePos, Vector2 Square, int DotNum)
    {
        //�h�b�g��`��
        for (int i = 0; i < DotNum; i++)
        {
            //��
            if (UsePos.x < Square.x && UsePos.y == StartPos.y)
            {
                //�h�b�g��ł��W�����炷
                UsePos.x += wide;

                //�����l�p����͂ݏo�����ꍇ�A�o���������̕`��ʒu�ɏC������
                if (UsePos.x > Square.x)
                {
                    UsePos.y += UsePos.x - Square.x;
                    UsePos.x = Square.x;
                }
            }
            //�E
            else if (UsePos.y < Square.y && UsePos.x == Square.x)
            {
                UsePos.y += wide;

                if (UsePos.y > Square.y)
                {
                    UsePos.x -= UsePos.y - Square.y;
                    UsePos.y = Square.y;
                }
            }
            //��
            else if (UsePos.x > StartPos.x && UsePos.y == Square.y)
            {
                UsePos.x -= wide;

                if (UsePos.x < StartPos.x)
                {
                    UsePos.y -= StartPos.x - UsePos.x;
                    UsePos.x = StartPos.x;
                }
            }
            //��
            else if (UsePos.y > StartPos.y && UsePos.x == StartPos.x)
            {
                UsePos.y -= wide;

                if (UsePos.y < StartPos.y)
                {
                    UsePos.x -= StartPos.y - UsePos.y;
                    UsePos.y = StartPos.y;
                }
            }

            //�C���l����p
            inUsePos = UsePos;

            //startPos���猩�č����ɂ�����
            if (checkPos == 1)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.y = -UsePos.y;
                inUsePos += StartPos * 2;
            }
            //startPos���猩�č���ɂ�����
            else if (checkPos == 2)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.x += StartPos.x * 2;
            }
            //startPos���猩�ĉE���ɂ�����
            else if (checkPos == 3)
            {
                inUsePos.y = -UsePos.y;
                inUsePos.y += StartPos.y * 2;
            }

            //�h�b�g�̕`��
            cloneDot.Add(Instantiate(managerAccessor.Instance.objDataManager.dotObj));
            cloneDot[i].transform.position = inUsePos;
        }
    }











    //���ǂ̌����̉���G���Ă��邩�`�F�b�N����֐�
    private void CheckRangeSize()
    {
        Vector2 mousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

        onPos = (int)ChangeSizePosName.NONE;
        bool[] rangeChecks = new bool[4];

        for (int i = 0; i < rangeChecks.Length; i++)
            rangeChecks[i] = false;

        //������
        if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
            judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeStartPos.y + changeSizeWidth.y > mousePos.y) 
        {
            onPos = (int)ChangeSizePosName.DOWN;
            rangeChecks[(int)ChangeSizePosName.DOWN] = true;
        }
        //�E����
        if (judgeEndPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
            judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
        {
            onPos = (int)ChangeSizePosName.RIGHT;
            rangeChecks[(int)ChangeSizePosName.RIGHT] = true;
        }
        //�㔻��
        if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
            judgeEndPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
        {
            onPos = (int)ChangeSizePosName.UP;
            rangeChecks[(int)ChangeSizePosName.UP] = true;
        }
        //������
        if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeStartPos.x + changeSizeWidth.x > mousePos.x &&
            judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
        {
            onPos = (int)ChangeSizePosName.LEFT;
            rangeChecks[(int)ChangeSizePosName.LEFT] = true;
        }

        //�E������
        if(rangeChecks[(int)ChangeSizePosName.DOWN]&& rangeChecks[(int)ChangeSizePosName.RIGHT])
        {
            onPos = (int)ChangeSizePosName.RIGHT_DOWN;
        }
        //�E�㔻��
        if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.RIGHT])
        {
            onPos = (int)ChangeSizePosName.RIGHT_UP;
        }
        //���㔻��
        if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.LEFT])
        {
            onPos = (int)ChangeSizePosName.LEFT_UP;
        }
        //��������
        if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.LEFT])
        {
            onPos = (int)ChangeSizePosName.LEFT_DOWN;
        }

        //�h�b�g�̘g�̏�ɃJ�[�\�������邩�ǂ���
        if (rangeChecks[(int)ChangeSizePosName.DOWN] || rangeChecks[(int)ChangeSizePosName.RIGHT] ||
            rangeChecks[(int)ChangeSizePosName.UP] || rangeChecks[(int)ChangeSizePosName.LEFT])
            onEdge = true;
        else
            onEdge = false;



    }

}
