using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    //�T�C�Y�ύX�������Ă��鉏�̈ʒu�̖��O
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

    public enum MouseDirection
    {
        RIGHT_UP,
        LEFT_DOWN,
        LEFT_UP,
        RIGHT_DOWN,
        NONE,
    }

    [SerializeField, Header("�ҏW���[�h�ؑ�")] private bool isEdit;

    [SerializeField, Header("�h�b�g�`�掞�̊Ԋu")] private float wide;

    [SerializeField, Header("�T�C�Y�ύX�ł��镝")] private Vector2 changeSizeWidth;

    //�ŏ��̈�񂵂��ʂ�Ȃ������p
    private bool first = true;
    private bool first2 = true;
    private bool first3 = false;
    private bool first4 = true;
    private bool first5 = true;

    private List<GameObject> Objs=new List<GameObject>();//�I������Ă���I�u�W�F�N�g�i�[�p
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
    private Vector2 backUpSquare;                       //�h�b�g�̎l�p�̃o�b�N�A�b�v�f�[�^
    private List<Vector3> memsize = new List<Vector3>();//�I������Ă���I�u�W�F�N�g�̃T�C�Y�f�[�^
    private List<Vector3> mempos = new List<Vector3>(); //�I������Ă���I�u�W�F�N�g�̈ʒu�f�[�^
    private Vector2 memjudgeStartPos;                   //�I������Ă���I�u�W�F�N�g�̒͂߂�͈�(����)
    private Vector2 memjudgeEndPos;                     //�I������Ă���I�u�W�F�N�g�̒͂߂�͈�(�E��)


    private Vector2 startPos;                           //�N���b�N�����Ƃ��̏����ʒu
    private Vector2 usePos;                             //�����ʒu���
    private Vector2 square;                             //�l�p�̏c���̒���
    private int checkPos = 0;                           //�N���b�N��AstartPos�����_�ɏc�����ꂼ��̈ʒu�`�F�b�N�p
    private int dotNum;                                 //�h�b�g�̐��i�[�p
    private Vector2 inUsePos;                           //usePos�̏C���l����p

    //DataManager�ł��g�p
    [System.NonSerialized] public List<GameObject> cloneDot = new List<GameObject>();//�h�b�g�i�[�p


    // Update is called once per frame
    void FixedUpdate()
    {
        //�L�����𑀍쒆�͑I���ł��Ȃ�
        if (!managerAccessor.Instance.dataMagager.playMode || isEdit) 
        {
            //�I������Ă�I�u�W�F�N�g���i�[�����
            Objs = managerAccessor.Instance.dataMagager.selectObjsData;

            //�I�u�W�F�N�g���I������Ă��鎞
            if (!selectionMode)
            {
                //�}�E�X���W�����[���h���W�ɕϊ�
                Vector3 nowMousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();

                CheckRangeSize();


                if (Objs.Count == 0) 
                {
                    editMode = false;
                }
                else if (onEdge)
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
        else
        {
            //playmode�̎��͋����I�ɘg��������
            if(cloneDot.Count!=0)
            {
                //�h�b�g�̏�����
                for (int i = 0; i < cloneDot.Count; i++)
                {
                    Destroy(cloneDot[i]);
                }
            }
            cloneDot.Clear();
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

                Debug.Log("aaa");
                //�I��
                selectionMode = true;
                //�}�E�X�̈ړ��ʂőI��͈͎Z�o
                Vector3 inputData = Input.mousePosition - clickStartPos;
                //�ړ��ʒ���
                inputData.x /= 108;
                inputData.y /= 108;
                inputData.z /= 108;
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
                checkPos = (int)MouseDirection.RIGHT_UP;

                //�l�p�̃T�C�Y�v�Z
                square.x = Mathf.Abs(dataManager.MouseWorldChange().x - startPos.x);
                square.y = Mathf.Abs(dataManager.MouseWorldChange().y - startPos.y);

                //�h�b�g��ł����v�Z
                dotNum = (int)((square.x * 2 + square.y * 2) / wide);
                //�����ʒu�����炷
                square += startPos;

                //�h�b�g�̏�����
                for (int i = 0; i < cloneDot.Count; i++)
                {
                    Destroy(cloneDot[i]);
                }
                cloneDot.Clear();

                //startPos���猩�č����ɂ�����
                if (dataManager.MouseWorldChange().x < startPos.x && dataManager.MouseWorldChange().y < startPos.y)
                {
                    checkPos = (int)MouseDirection.LEFT_DOWN;
                }
                //startPos���猩�č���ɂ�����
                else if (dataManager.MouseWorldChange().x < startPos.x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
                //startPos���猩�ĉE���ɂ�����
                else if (dataManager.MouseWorldChange().y < startPos.y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }

                //�h�b�g�̕`��
                DotDraw(startPos, usePos, square, dotNum);


                first3 = true;
            }
            else
            {
                //�N���b�N�I�����̃}�E�X�̍��W�擾
                if (first3)
                {
                    //�E��
                    if (checkPos == (int)MouseDirection.RIGHT_UP)
                    {
                        judgeStartPos = startPos;
                        judgeEndPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                    }
                    //����
                    else if (checkPos == (int)MouseDirection.LEFT_DOWN)
                    {
                        judgeStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                        judgeEndPos = startPos;
                    }
                    //����
                    else if (checkPos == (int)MouseDirection.LEFT_UP)
                    {
                        judgeStartPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeStartPos.y = startPos.y;
                        judgeEndPos.x = startPos.x;
                        judgeEndPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                    }
                    //�E��
                    else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
                    {
                        judgeStartPos.x = startPos.x;
                        judgeStartPos.y = managerAccessor.Instance.dataMagager.MouseWorldChange().y;
                        judgeEndPos.x = managerAccessor.Instance.dataMagager.MouseWorldChange().x;
                        judgeEndPos.y = startPos.y;
                    }
                    first3 = false;
                }

                //�I�u�W�F�N�g�������I������Ă��Ȃ������Ƃ�
                if (Objs.Count == 0) 
                {
                    //�h�b�g�̏�����
                    for (int i = 0; i < cloneDot.Count; i++)
                    {
                        Destroy(cloneDot[i]);
                    }
                    cloneDot.Clear();

                    //��ʊO�֔�΂�
                    judgeStartPos = new Vector3(999, 999, 999);
                    judgeEndPos = new Vector3(999, 999, 999);
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
            //�I���J�n���L�����Ă��������ϐ�
            if (first4)
            {
                onStartPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                startPos = judgeStartPos;
                backUpSquare.x = Mathf.Abs(judgeEndPos.x - judgeStartPos.x);
                backUpSquare.y = Mathf.Abs(judgeEndPos.y - judgeStartPos.y);
                memjudgeStartPos = judgeStartPos;
                memjudgeEndPos = judgeEndPos;
                memsize.Clear();
                mempos.Clear();
                for (int i = 0; i < Objs.Count; i++)
                {
                    memsize.Add(Objs[i].transform.localScale);
                    mempos.Add(Objs[i].transform.localPosition);
                }
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

            //�����ʒu���痣�ꂽ����
            Vector2 setStartPos = judgeStartPos;

            //�`��̏����ʒu�������ɐݒ�
            checkPos = (int)MouseDirection.RIGHT_UP;


            //�l�p�̃T�C�Y�ύX�����ꂼ��̐��l�ύX
            if (onPos == (int)ChangeSizePosName.DOWN) 
            {
                //�l�p�̃T�C�Y�ύX
                square.x = backUpSquare.x;
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                //�����ʒu�̐ݒ�
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                //�Q�[���S�̂Ŕ��������l�p�̍��W�X�V
                judgeStartPos.y = setStartPos.y;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(0, -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);
                if (judgeEndPos.y < dataManager.MouseWorldChange().y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }
            }
            if (onPos == (int)ChangeSizePosName.RIGHT)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                judgeEndPos.x = setStartPos.x + square.x;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, 0, 0), onPos);

                if (judgeStartPos.x > dataManager.MouseWorldChange().x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
            }
            if (onPos == (int)ChangeSizePosName.UP)
            {
                square.x = backUpSquare.x;
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.y = setStartPos.y + square.y;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(0, dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                if (judgeStartPos.y > dataManager.MouseWorldChange().y)
                {
                    checkPos = (int)MouseDirection.RIGHT_DOWN;
                }
            }
            if (onPos == (int)ChangeSizePosName.LEFT)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y;
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), 0, 0), onPos);

                if (judgeEndPos.x < dataManager.MouseWorldChange().x)
                {
                    checkPos = (int)MouseDirection.LEFT_UP;
                }
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_DOWN)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeEndPos.x = setStartPos.x + square.x;
                judgeStartPos.y = setStartPos.y;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);

                checkPos = GetCheckPos(new Vector2(judgeStartPos.x, judgeEndPos.y), dataManager.MouseWorldChange());
            }
            if (onPos == (int)ChangeSizePosName.RIGHT_UP)
            {
                square.x = backUpSquare.x + (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                judgeEndPos.x = setStartPos.x + square.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(dataManager.MouseWorldChange().x - onStartPos.x, dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                checkPos = GetCheckPos(new Vector2(judgeStartPos.x, -judgeStartPos.y), new Vector2(dataManager.MouseWorldChange().x, -dataManager.MouseWorldChange().y));
            }
            if (onPos == (int)ChangeSizePosName.LEFT_UP)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y + (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                judgeStartPos.x = setStartPos.x;
                judgeEndPos.y = setStartPos.y + square.y;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), dataManager.MouseWorldChange().y - onStartPos.y, 0), onPos);

                checkPos = GetCheckPos(new Vector2(-judgeEndPos.x, -judgeStartPos.y), -dataManager.MouseWorldChange());
            }
            if (onPos == (int)ChangeSizePosName.LEFT_DOWN)
            {
                square.x = backUpSquare.x - (dataManager.MouseWorldChange().x - onStartPos.x);
                square.y = backUpSquare.y - (dataManager.MouseWorldChange().y - onStartPos.y);
                setStartPos.x = startPos.x + dataManager.MouseWorldChange().x - onStartPos.x;
                setStartPos.y = startPos.y + dataManager.MouseWorldChange().y - onStartPos.y;
                judgeStartPos = setStartPos;
                //�T�C�Y�ύX���̈ړ��ʐݒ�
                BlockRatioChange(new Vector3(-(dataManager.MouseWorldChange().x - onStartPos.x), -(dataManager.MouseWorldChange().y - onStartPos.y), 0), onPos);

                checkPos = GetCheckPos(new Vector2(-judgeEndPos.x, judgeEndPos.y), new Vector2(-dataManager.MouseWorldChange().x, dataManager.MouseWorldChange().y));
            }

            square.x = Mathf.Abs(square.x);
            square.y = Mathf.Abs(square.y);

            //�T�C�Y�ύX�K�p����
            BlockSizeChange(new Vector3(square.x / backUpSquare.x, square.y / backUpSquare.y, 0));

            //�`��ʒu�ݒ�p���W�̃X�^�[�g�ʒu������
            usePos = setStartPos;


            //�h�b�g��ł����v�Z
            dotNum = (int)((Mathf.Abs(square.x * 2) + Mathf.Abs(square.y * 2)) / wide);
            //�����ʒu�����炷
            square += setStartPos;



            //�h�b�g�̏�����
            for (int i = 0; i < cloneDot.Count; i++)
                Destroy(cloneDot[i]);
            cloneDot.Clear();

            

            //�h�b�g�̕`��
            DotDraw(setStartPos, usePos, square, dotNum);

            first5 = true;
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
            if (checkPos == (int)MouseDirection.LEFT_DOWN)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.y = -UsePos.y;
                inUsePos += StartPos * 2;
            }
            //startPos���猩�č���ɂ�����
            else if (checkPos == (int)MouseDirection.LEFT_UP)
            {
                inUsePos.x = -UsePos.x;
                inUsePos.x += StartPos.x * 2;
            }
            //startPos���猩�ĉE���ɂ�����
            else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
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
        //�}�E�X�̃��[���h���W
        Vector2 mousePos = managerAccessor.Instance.dataMagager.MouseWorldChange();
        //�ߓx�̔���Ƃ�p
        bool[] rangeChecks = new bool[4];

        //�N���b�N����Ă���Ƃ��́A������Ă���ꏊ��ύX���Ȃ�
        if (!Input.GetMouseButton(0))
        {
            //�����ꂽ�ꏊ�̏�����
            onPos = (int)ChangeSizePosName.NONE;
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                rangeChecks[i] = false;
            }

            //������
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeStartPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.DOWN;
                rangeChecks[(int)ChangeSizePosName.DOWN] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.DOWN;
            }
            //�E����
            if (judgeEndPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.RIGHT;
                rangeChecks[(int)ChangeSizePosName.RIGHT] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT;
            }
            //�㔻��
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeEndPos.x + changeSizeWidth.x > mousePos.x &&
                judgeEndPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.UP;
                rangeChecks[(int)ChangeSizePosName.UP] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.UP;
            }
            //������
            if (judgeStartPos.x - changeSizeWidth.x < mousePos.x && judgeStartPos.x + changeSizeWidth.x > mousePos.x &&
                judgeStartPos.y - changeSizeWidth.y < mousePos.y && judgeEndPos.y + changeSizeWidth.y > mousePos.y)
            {
                onPos = (int)ChangeSizePosName.LEFT;
                rangeChecks[(int)ChangeSizePosName.LEFT] = true;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT;
            }

            //�E������
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_DOWN;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT_DOWN;
            }
            //�E�㔻��
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.RIGHT])
            {
                onPos = (int)ChangeSizePosName.RIGHT_UP;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.RIGHT_UP;
            }
            //���㔻��
            if (rangeChecks[(int)ChangeSizePosName.UP] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_UP;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT_UP;
            }
            //��������
            if (rangeChecks[(int)ChangeSizePosName.DOWN] && rangeChecks[(int)ChangeSizePosName.LEFT])
            {
                onPos = (int)ChangeSizePosName.LEFT_DOWN;
                managerAccessor.Instance.dataMagager.whereEdge = (int)ChangeSizePosName.LEFT_DOWN;
            }
        }

        

        //�h�b�g�̘g�̏�ɃJ�[�\�������邩�ǂ���
        if (rangeChecks[(int)ChangeSizePosName.DOWN] || rangeChecks[(int)ChangeSizePosName.RIGHT] ||
            rangeChecks[(int)ChangeSizePosName.UP] || rangeChecks[(int)ChangeSizePosName.LEFT])
        {
            onEdge = true;
            managerAccessor.Instance.dataMagager.onEdge = true;
        }
        else
        {
            //�}�E�X��������Ă���Ƃ��͕ύX���Ȃ�
            if (!Input.GetMouseButton(0))
            {
                onEdge = false;
                managerAccessor.Instance.dataMagager.onEdge = false;

                //�Ђ�����Ԃ������l�p�̍��W��ύX
                if (first5)
                {
                    //���ꂼ��̃o�b�N�A�b�v�f�[�^
                    Vector2 _judgeStartPos = judgeStartPos;
                    Vector2 _judgeEndPos = judgeEndPos;


                    //����
                    if (checkPos == (int)MouseDirection.LEFT_DOWN)
                    {
                        judgeStartPos = _judgeEndPos;
                        judgeEndPos = _judgeStartPos;
                    }
                    //����
                    else if (checkPos == (int)MouseDirection.LEFT_UP)
                    {
                        judgeStartPos.x = _judgeEndPos.x;
                        judgeEndPos.x = _judgeStartPos.x;
                    }
                    //�E��
                    else if (checkPos == (int)MouseDirection.RIGHT_DOWN)
                    {
                        judgeStartPos.y = _judgeEndPos.y;
                        judgeEndPos.y = _judgeStartPos.y;
                    }

                    first5 = false;
                }
            }

        }



    }


    //�u���b�N�̃T�C�Y�ύX�K�p�֐�
    //����1�@�u���b�N�̃T�C�Y�ύX��
    private void BlockSizeChange(Vector3 MovePower)
    {
        //�I�u�W�F�N�g���I������Ă���Ƃ�
        if (memsize.Count != 0)
        {
            //�I������Ă���I�u�W�F�N�g�ɉ��Z
            for (int i = 0; i < Objs.Count; i++)
            {
                //�T�C�Y��ς���䗦���͗p
                Objs[i].transform.localScale = Mul(memsize[i], MovePower);
            }
            
        }
    }


    //�T�C�Y�ύX���ʒu��䗦�ɍ��킹��֐�
    //����1�@�����ʒu���猩���}�E�X�̈ړ���
    //����2�@�ǂ̏ꏊ�������Ă��邩
    private void BlockRatioChange(Vector3 MovePower, int OnPos)
    {
        //�I�u�W�F�N�g���I������Ă���Ƃ�
        if (mempos.Count != 0)
        {
            //�I������Ă���I�u�W�F�N�g�ɉ��Z
            for (int i = 0; i < Objs.Count; i++)
            {
                //�͂��Ă��钆�ł̉����猩���䗦
                Vector3 ratio = new Vector3(0, 0, 0);
                //���W���͗p
                Vector3 input = new Vector3(0, 0, 0);

                //���ꂼ��G���Ă邢��ꏊ
                switch (OnPos)
                {
                    case (int)ChangeSizePosName.DOWN:
                        //�䗦�̌v�Z
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        //�䗦������
                        Objs[i].transform.localPosition = mempos[i] - Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.RIGHT:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        Objs[i].transform.localPosition = mempos[i] + Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.UP:
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        Objs[i].transform.localPosition = mempos[i] + Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.LEFT:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        Objs[i].transform.localPosition = mempos[i] - Mul(MovePower, ratio);
                        break;
                    case (int)ChangeSizePosName.RIGHT_DOWN:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        input.x = mempos[i].x + Mul(MovePower, ratio).x;
                        input.y = mempos[i].y - Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.RIGHT_UP:
                        ratio.x = (memjudgeStartPos.x - mempos[i].x) / (memjudgeStartPos.x - memjudgeEndPos.x);
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        input.x = mempos[i].x + Mul(MovePower, ratio).x;
                        input.y = mempos[i].y + Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.LEFT_UP:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        ratio.y = (memjudgeStartPos.y - mempos[i].y) / (memjudgeStartPos.y - memjudgeEndPos.y);
                        input.x = mempos[i].x - Mul(MovePower, ratio).x;
                        input.y = mempos[i].y + Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                    case (int)ChangeSizePosName.LEFT_DOWN:
                        ratio.x = (memjudgeEndPos.x - mempos[i].x) / (memjudgeEndPos.x - memjudgeStartPos.x);
                        ratio.y = (memjudgeEndPos.y - mempos[i].y) / (memjudgeEndPos.y - memjudgeStartPos.y);
                        input.x = mempos[i].x - Mul(MovePower, ratio).x;
                        input.y = mempos[i].y - Mul(MovePower, ratio).y;
                        Objs[i].transform.localPosition = input;
                        break;
                }
            }
        }
    }


    //���_���猩�ă}�E�X�������ɂ��邩�`�F�b�N����֐�
    //����1�@���_
    //����2�@�}�E�X�̍��W
    //�߂�l�@���݂̃}�E�X�̕���
    private int GetCheckPos(Vector2 origin,Vector2 mouse)
    {
        int checkpos = 0;

        //���_���猩�ĉE��ɂ���Ƃ�
        if (origin.x < mouse.x && origin.y < mouse.y) 
        {
            checkpos = (int)MouseDirection.RIGHT_DOWN;
        }
        //���_���猩�č����ɂ���Ƃ�
        else if (origin.x > mouse.x && origin.y > mouse.y)
        {
            checkpos = (int)MouseDirection.LEFT_UP;
        }
        //���_���猩�č���ɂ���Ƃ�
        else if (origin.x > mouse.x && origin.y < mouse.y)
        {
            checkpos = (int)MouseDirection.LEFT_DOWN;
        }
        //���_���猩�ĉE���ɂ���Ƃ�
        else if (origin.x < mouse.x && origin.y > mouse.y)
        {
            checkpos = (int)MouseDirection.RIGHT_UP;
        }

        return checkpos;
    }


    //vector3���m�̊|���Z�֐�
    private Vector3 Mul(Vector3 a,Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

}
