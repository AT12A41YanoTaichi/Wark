using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//-----
//����F���
//�ǋL�F�����A����
//�T�v�F�G�̓����̏���
///----

public class Enemy : MonoBehaviour
{
    // �ǋL�F����
    [Header("��b�C�x���g")]
    [SerializeField] private TalkEventManager talkEvent = null;

    [SerializeField]
    [Header("EnemyBase�X�N���v�g�擾")]
    private EnemyBase Ebase;


    [SerializeField]
    [Header("�G�̒e")]
    private GameObject Bullet;

    [SerializeField]
    [Header("�e�𔭎˂���ꏊ")]
    private GameObject tarett;

   
    private int EnemyMAxHp = 200;

   
    private int Hp = 200;

    private int EnemyMaxMP = 120;

    private int MP = 120;

    //���v���C���[�̎擾
    [Header("�v���C���[�̎擾")]
    [SerializeField] private Player player;

    //���v���C���[�̎擾
    [Header("�v���C���[�̎擾")]
    [SerializeField] private GameObject playerObject;


    [Header("�_�������̍ۂ̍���")]
    [SerializeField] private float AimHight;

    //�v���C���[�̈ʒu������ϐ�
    Vector3 playerPos;

    Vector3 PlayerPos;
    //�G�̃X�s�[�h
    private float Speed;
    //�G�̃_�b�V���̃X�s�[�h
    private float DashSpeed;


    [Header("�G�̎��̍s�����l���鎞��")]
    public float IdelTime = 3.0f;

    //���Ԃ��g���ď�����x�点��Ƃ��Ɏg���ϐ�
    private float time = 0.0f;


    [Header("�U�����s���Ă��鎞��")]
    public float AttackTime = 3.0f;

  

   
    //RigidBody���擾
    private Rigidbody rb;

    [SerializeField, Header("�G�̃W�����v��")]
    private float JunpPower;

    [SerializeField, Header("�W�����v�ߐڂ̃W�����v��")]
    private float JumpAttackPower;

    //�W�����v����
    private bool IsJunp = false;

    //�ǂɂԂ����Ă��邩
    private bool MoveStop = false;
    //�W�����v�U�������Ă��邩
    private bool JumpAttack = false;
    //��]���I����Ă��邩
    private bool isRotation = true;
    //�������U���Ń`���[�W���邩
    private bool isCharge = false;
    //�������U���ő_����
    private bool isAim = false;
    private bool isMinuse = false;
    //�����蔻��I�u�W�F�N�g
    [Header("�E�U�������蔻��I�u�W�F�N�g")]
    [SerializeField] private GameObject RightattackObject;
    private Collider attackRightCollider;
    private EnemyAttack ERightattack;
    [Header("���U�������蔻��I�u�W�F�N�g")]
    [SerializeField] private GameObject LeftatackObject;
    private Collider attackLeftCollider;
    private EnemyAttack ELeftattack;
    [Header("�W�����v�U�������蔻��I�u�W�F�N�g")]
    [SerializeField] private GameObject JumptackObject;
    private Collider JumpattackCollider;
    private EnemyAttack Jumptattack;

    // ���g��Transform
    [SerializeField] private Transform _self;

    // �^�[�Q�b�g��Transform
    [SerializeField] private Transform _target;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 _forward = Vector3.forward;

    private bool isLeft = true;

    [SerializeField, Header("�G��Animatr")]
    private Animator animator;
    [SerializeField, Header("CameraShack�X�N���v�g�擾")]
    private CameraShake camerashake;
    AnimatorStateInfo animeInfo;
    private bool ChajeSE = false;
    private bool ShotSE = false;

    [SerializeField, Header("�_�E���܂ł̉�")]
    private int DownCount;
    private int Count = 0;
   
    private bool NotRotation = false;

    [SerializeField, Header("���肵�����Ȃ�")]
    private bool CloseAttaack;

    [SerializeField, Header("�����������Ȃ�")]
    private bool Move;

    [Header("�e������������(true�Ȃ�)")]
    [SerializeField] private bool isFaier = false;

    [Header("Chage�e������������(true�Ȃ�)")]
    [SerializeField] private bool Chage = false;

    [Header("�A���U���������Ȃ�(true�Ȃ�)")]
    [SerializeField] private bool succession = false;

    [Header("�`���[�W�ߐڂ������Ȃ�(true�Ȃ�)")]
    [SerializeField] private bool ChageAttack = false;

    [Header("�W�����v�U���������Ȃ�(true�Ȃ�)")]
    [SerializeField] private bool jumpAttack = false;

    [Header("�s���s��(true�Ȃ�)")]
    [SerializeField] private bool NoStart = false;
    [Header("�A��(true�Ȃ�)")]
    [SerializeField] private bool isBlazing = false;

    [Header("��R��(true�Ȃ�)")]
    [SerializeField]private bool Flipkick = false;

    [Header("3�i�K�U��(true�Ȃ�)")]
    [SerializeField] private bool ThreeAttack = false;

    private bool BlazingShot = false;

    private bool fixation = false;
    private bool Left = false;
    private bool OwnGetPlayerPos = false;
    private bool islowered = false;
    private bool AnimeStop = false;

    private Vector3 eulerAngles;

    private static readonly int PROPERTY_COLOR = Shader.PropertyToID("_Color");
    [SerializeField, Header("���f����Renderer")]
    private Renderer _renderer;
    private Material _material;
    private Sequence _seq;

   

    private bool GetPostiton = false;
    [SerializeField, Header("�G�t�F�N�g")]
    private GameObject AttackEffect;
    private bool NoEffect = false;
    private bool getrnd = false;
    private bool changedirection = false;
    

    //�G�̃X�e�[�g
    public enum EnemyState
    {
        //���̍s����I��
        Idel,
        //�߂Â�
        Move,
        //�ߐڍU��
        CloseAttack,
        //�`���[�W�ߐڍU��
        ChageAttack,
        //�_�b�V���ߋ����U��
        DashCloseAttack,
        //�W�����v���ċߐڍU��������
        JunpAttack,
        //�������U��
        LongAttack,
        //�E�ɉ������ĉ������U��
        BackLeftAttack,
        //���ɉ������ĉ������U��
        BackRightAttack,
        //�`���[�W�������U��
        ChargeLongAttack,
        //�E�ɉ������ă`���[�W�������U��
        ChargeBackLeftAttack,
        //���ɉ������ă`���[�W�������U��
        ChargeBackRightAttack,
        //�e�A��
        Blazing,
        //�_������
        AimShoot,
        //�A���U��
        Succession,
        //�|���
        fall,
        //�o�b�N�W�����v
        BackJump,
        //��R��
        FlipKick,
        //3�A���U��
        ThreeAttack,
    }
    [Header("�G�̌��݂̃X�e�[�g")]
    public EnemyState enemyState;
    private int rnd;
    private bool isShot;
    private bool isFall = true;
    private bool isBack;
    [SerializeField, Header("�e")]
    GameObject bullet;
    [SerializeField, Header("�e�X�N���v�g")]
    Bullet B;
    private int count = 0;
    private bool Attack = false;
    private CameraZoom zoom;
    // Start is called before the first frame update
    void Start()
    {
        zoom = GameObject.Find("Main Camera").GetComponent<CameraZoom>();
        _material = _renderer.material;
        isShot = false;
        isAim = false;
        Speed = 5.0f;
        DashSpeed = 8.0f;

        rb = GetComponent<Rigidbody>();
        ERightattack = RightattackObject.GetComponent<EnemyAttack>();
        attackRightCollider = RightattackObject.GetComponent<Collider>();

        ELeftattack = LeftatackObject.GetComponent<EnemyAttack>();
        attackLeftCollider = LeftatackObject.GetComponent<Collider>();

        Jumptattack = JumptackObject.GetComponent<EnemyAttack>();
        JumpattackCollider = JumptackObject.GetComponent<Collider>();

        OffColliderAttack();

        OffColliderRightAttack();


        EnemyMAxHp = Ebase.GetEnemyMaxHp();
        EnemyMaxMP = Ebase.GetEnemyMaxMp();
        Hp = Ebase.GetEnemyHp();
        MP = Ebase.GetEnemyMp();


        if (isFaier == false)
        {
            rnd = Random.Range(1, 6);
            switch (rnd)
            {
                case 1:
                    enemyState = EnemyState.CloseAttack;
                    break;
                case 2:
                    enemyState = EnemyState.LongAttack;
                    break;
                case 3:
                    enemyState = EnemyState.Move;
                    break;
                case 4:
                    enemyState = EnemyState.JunpAttack;
                    break;
                case 5:
                    enemyState = EnemyState.AimShoot;
                    break;
            }
        }
        else
        {
            enemyState = EnemyState.LongAttack;
        }

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        // �ǋL�F����
        // ��b�C�x���g������Ƃ��A�I������炱�̌�̏������s��
        if (talkEvent != null)
        {
            if (!talkEvent.IsEnd())
            {
                return;
            }
        }

        if(player.GetHp() <= 0)
        {
            return;
        }

        animeInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (playerObject)
        {
            playerPos.x = playerObject.transform.position.x;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        Hp = Ebase.GetEnemyHp();
        MP = Ebase.GetEnemyMp();
        float Pos = Vector3.Distance(transform.position, playerPos);
        Debug.Log(enemyState);
        switch (enemyState)
        {
            //���̍U���I��
            case EnemyState.Idel:
                {
                    //�������Ԃ��󂯂�  
                    if (time > IdelTime && !ERightattack.NowStunTime())
                    {
                        if (MP > 0)
                        {
                            if (!NoStart)
                            {
                                if (isFaier)
                                {
                                    enemyState = EnemyState.LongAttack;
                                    isBack = false;
                                }
                                else if (CloseAttaack)
                                {
                                    enemyState = EnemyState.CloseAttack;
                                }
                                else if (Move)
                                {
                                    enemyState = EnemyState.Move;
                                }
                                else if (Chage)
                                {
                                    enemyState = EnemyState.ChargeLongAttack;
                                }
                                else if (succession)
                                {
                                    enemyState = EnemyState.Succession;
                                }
                                else if (ChageAttack)
                                {
                                    enemyState = EnemyState.ChageAttack;
                                }
                                else if (jumpAttack)
                                {
                                    enemyState = EnemyState.JunpAttack;
                                }
                                else if (isBlazing)
                                {
                                    enemyState = EnemyState.Blazing;
                                }
                                else if (Flipkick)
                                {
                                    enemyState = EnemyState.FlipKick;
                                }
                                else if(ThreeAttack)
                                {
                                    enemyState = EnemyState.ThreeAttack;
                                }
                                else
                                {
                                    //���̍s���������_���Ō��߂�
                                    //HP�������ɂȂ�����
                                    if (Hp > 100)
                                    {
                                        {
                                            if (Pos < 2.4f)
                                            {
                                                rnd = Random.Range(1, 4);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.ThreeAttack;
                                                        isBack = false;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.FlipKick;
                                                        break;
                                                }
                                            }
                                            else if (Pos > 2.4f && Pos < 8.0f)
                                            {
                                                rnd = Random.Range(1, 4);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        isBack = false;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.ChageAttack;
                                                        break;
                                                }
                                            }
                                            else if (Pos > 8.0f && Pos < 10.0f)
                                            {

                                                rnd = Random.Range(1, 6);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.Move;
                                                        break;
                                                    case 4:
                                                        enemyState = EnemyState.ChargeLongAttack;
                                                        break;
                                                    case 5:
                                                        enemyState = EnemyState.ChageAttack;
                                                        break;
                                                }

                                            }
                                            else if (Pos > 10.0f)
                                            {
                                                rnd = Random.Range(1, 8);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.Move;
                                                        break;
                                                    case 4:
                                                        enemyState = EnemyState.ChargeLongAttack;
                                                        break;
                                                    case 5:
                                                        enemyState = EnemyState.JunpAttack;
                                                        break;
                                                    case 6:
                                                        enemyState = EnemyState.AimShoot;
                                                        break;
                                                    case 7:
                                                        enemyState = EnemyState.ChageAttack;
                                                        break;
                                                }
                                            }

                                        }
                                    }
                                    //HP������������������
                                    else
                                    {
                                        {
                                            if (Pos < 5.0f)
                                            {
                                                rnd = Random.Range(1, 3);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.Succession;
                                                        break;
                                                }
                                            }
                                            else if (Pos > 5.0f && Pos < 8.0f)
                                            {
                                                rnd = Random.Range(1, 6);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        isBack = false;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.Succession;
                                                        break;

                                                    case 4:
                                                        enemyState = EnemyState.ChageAttack;
                                                        break;

                                                    case 5:
                                                        enemyState = EnemyState.Blazing;
                                                        break;
                                                }
                                            }
                                            else if (Pos > 8.0f && Pos < 10.0f)
                                            {

                                                rnd = Random.Range(1, 8);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.Move;
                                                        break;
                                                    case 4:
                                                        enemyState = EnemyState.ChargeLongAttack;
                                                        break;
                                                    case 5:
                                                        enemyState = EnemyState.Succession;
                                                        isBack = false;
                                                        break;

                                                    case 6:
                                                        enemyState = EnemyState.ChageAttack;
                                                        break;

                                                    case 7:
                                                        enemyState = EnemyState.Blazing;
                                                        break;

                                                }

                                            }
                                            else if (Pos > 10.0f)
                                            {
                                                rnd = Random.Range(1, 10);
                                                switch (rnd)
                                                {
                                                    case 1:
                                                        enemyState = EnemyState.CloseAttack;
                                                        break;

                                                    case 2:
                                                        enemyState = EnemyState.LongAttack;
                                                        break;

                                                    case 3:
                                                        enemyState = EnemyState.Move;
                                                        break;
                                                    case 4:
                                                        enemyState = EnemyState.ChargeLongAttack;
                                                        break;
                                                    case 5:
                                                        enemyState = EnemyState.JunpAttack;
                                                        break;
                                                    case 6:
                                                        enemyState = EnemyState.AimShoot;
                                                        break;
                                                    case 7:
                                                        enemyState = EnemyState.Succession;
                                                        isBack = false;
                                                        break;
                                                    case 8:
                                                        enemyState = EnemyState.ChageAttack;
                                                        isBack = false;
                                                        break;

                                                    case 9:
                                                        enemyState = EnemyState.Blazing;
                                                        break;

                                                }
                                            }
                                        }
                                    }

                                }

                                time = 0.0f;
                            }

                        }

                    }

                    time += Time.deltaTime;

                }

                break;
            //�v���C���[�ɋ߂Â�
            case EnemyState.Move:
                {
                    animator.SetBool("Walk", true);
                    //�v���C���[���G��荶���ɋ����獶���Ɉړ�
                    if (playerPos.x < transform.position.x)
                    {

                        if (Pos < 5.0f)
                        {
                            animator.SetBool("Walk", false);
                            SetState(EnemyState.Idel);
                        }
                        else
                        {
                            transform.position -= Vector3.right * Speed * Time.deltaTime;
                        }
                    }

                    //�v���C���[���G���E���ɋ�����E���Ɉړ�

                    if (playerPos.x > transform.position.x)
                    {
                        if (Pos < 5.0f)
                        {
                            animator.SetBool("Walk", false);
                            SetState(EnemyState.Idel);
                        }
                        else
                        {
                            transform.position += Vector3.right * Speed * Time.deltaTime;
                        }
                    }

                }

                break;
            //�������U��
            case EnemyState.LongAttack:
                {
                    //�v���C���[���G��荶���ɋ�����
                    if (playerPos.x < transform.position.x)
                    {
                        if (Pos > 6.0f || !isBack)
                        {
                            animator.SetBool("Shot", true);

                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.7f)
                                {
                                    if (!isShot)
                                    {
                                        Debug.Log("��������");
                                        Shot();
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ�
                                    }

                                    if (!isMinuse)
                                    {
                                        MP -= 10;
                                        isMinuse = true;
                                    }
                                }
                                if (animeInfo.normalizedTime > 1.0f)
                                {
                                    isMinuse = false;
                                    animator.SetBool("Shot", false);
                                    isShot = false;
                                    isBack = true;
                                    SetState(EnemyState.Idel);
                                }
                            }
                        }
                        else
                        {
                            if (!isShot && isBack)
                            {
                                SetState(EnemyState.BackLeftAttack);
                            }
                            else
                            {
                                isMinuse = false;
                                animator.SetBool("Shot", false);
                                isShot = false;
                                SetState(EnemyState.Idel);
                            }

                        }
                    }

                    //�v���C���[���G���E���ɋ�����
                    if (playerPos.x > transform.position.x)
                    {
                        if (Pos > 6.0f || !isBack)
                        {
                            animator.SetBool("Shot", true);
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.8f)
                                {
                                    if (!isShot)
                                    {
                                        //Debug.Log("��������");
                                        Shot();
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ�
                                    }

                                    if (!isMinuse)
                                    {
                                        MP -= 10;
                                        isMinuse = true;
                                    }
                                }
                            }
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 1.0f)
                                {
                                    isMinuse = false;
                                    animator.SetBool("Shot", false);
                                    isShot = false;
                                    isBack = true;
                                    SetState(EnemyState.Idel);
                                }

                            }
                        }
                        else
                        {
                            if (!isShot && isBack)
                            {
                                //�G�Ƃ̋������߂���
                                SetState(EnemyState.BackRightAttack);
                            }
                            else
                            {
                                isMinuse = false;
                                animator.SetBool("Shot", false);
                                isShot = false;
                                SetState(EnemyState.Idel);
                            }
                        }
                    }
                }

                break;

            //�E�ɉ������ĉ������U��
            case EnemyState.BackLeftAttack:
                {
                    time += Time.deltaTime;
                    //�����̈ʒu���擾���Ă��Ȃ�������擾����
                    if (time >= 1.0f)
                    {
                        animator.SetBool("Shot", true);
                        //�G�Ƃ̋������߂������ꍇ����ĉ������U��������
                        isLeft = true;
                        if (!isShot)
                        {
                            Shot();
                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ�
                        }
                        if (!isMinuse)
                        {
                            MP -= 10;
                            isMinuse = true;
                        }

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("Shot", false);
                                animator.SetBool("Step", false);
                                isMinuse = false;
                                isShot = false;
                                MoveStop = false;
                                SetState(EnemyState.Idel);
                                time = 0.0f;
                            }
                        }


                    }
                    else
                    {
                        if (MoveStop == false)
                        {
                            //�ǂɐG��Ă��Ȃ���Έړ�
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Step", true);
                        }
                    }

                }

                break;
            //���ɉ������ĉ������U��
            case EnemyState.BackRightAttack:
                {
                    time += Time.deltaTime;
                    //�����̈ʒu���擾���Ă��Ȃ�������擾����

                    if (time >= 1.0f)
                    {
                        isLeft = false;
                        //�G�Ƃ̋������߂������ꍇ����ĉ������U��������

                        animator.SetBool("Shot", true);

                        time = 0.0f;
                        if (!isShot)
                        {
                            Shot();
                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ�
                            isShot = true;
                        }
                        if (!isMinuse)
                        {
                            MP -= 10;
                            isMinuse = true;
                        }

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("Shot", false);
                                animator.SetBool("Step", false);
                                isMinuse = false;
                                isShot = false;
                                MoveStop = false;
                                SetState(EnemyState.Idel);
                                time = 0.0f;
                            }
                        }


                    }
                    else
                    {
                        if (MoveStop == false)
                        {
                            //�ǂɐG��Ă��Ȃ���Έړ�
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Step", true);
                        }
                    }
                }

                break;
            //�`���[�W���ĉ������U��
            case EnemyState.ChargeLongAttack:
                {
                    if (!OwnGetPlayerPos)
                    {
                        if (playerPos.x < transform.position.x)
                        {
                            isLeft = true;
                        }
                        else
                        {
                            isLeft = false;
                        }
                        OwnGetPlayerPos = true;
                    }


                    time += Time.deltaTime;
                    //�v���C���[���G��荶���ɋ�����
                    if (playerPos.x < transform.position.x)
                    {
                        //if (Pos > 8.0f || isCharge)
                        {
                            animator.SetBool("Shot", true);
                            isCharge = true;

                            //���������Ƃ���܂ŃA�j���[�V�����𓮂����B
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.7f)
                                {
                                    //��x�����A�j���[�V�������~�߂�
                                    if (!AnimeStop)
                                    {
                                        animator.speed = 0.0f;
                                        AnimeStop = true;
                                    }
                                    //�e���`���[�W���n�߂�A�ꔭ������
                                    if (isShot == false)
                                    {
                                        Shot();
                                    }
                                    else
                                    {
                                        if (!ChajeSE)
                                        {
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK); // �e���������ˎ����߉�
                                            ChajeSE = true;
                                        }
                                    }
                                    //MP�����炵�Ă��Ȃ�������
                                    if (!isMinuse)
                                    {
                                        //�e���������^�C�~���O
                                        if (time >= 3.2f)
                                        {
                                            if (!ShotSE)
                                            {
                                                SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ�
                                                ShotSE = true;
                                            }
                                            MP -= 20;
                                            isMinuse = true;
                                            animator.speed = 1.0f;
                                        }
                                    }
                                }
                                if (animeInfo.normalizedTime > 1.0f)
                                {
                                    isMinuse = false;
                                    AnimeStop = false;
                                    ChajeSE = false;
                                    ShotSE = false;
                                    isShot = false;
                                    OwnGetPlayerPos = false;
                                    time = 0.0f;
                                    isCharge = false;
                                    animator.SetBool("Shot", false);
                                    SetState(EnemyState.Idel);
                                }
                            }
                        }
                    }
                    //�v���C���[���G���E���ɋ�����
                    if (playerPos.x > transform.position.x)
                    {
                        //if (Pos > 8.0f || isCharge)
                        {
                            animator.SetBool("Shot", true);
                            isCharge = true;

                            //���������Ƃ���܂ŃA�j���[�V�����𓮂����B
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.7f)
                                {
                                    //��x�����A�j���[�V�������~�߂�
                                    if (!AnimeStop)
                                    {
                                        animator.speed = 0.0f;
                                        AnimeStop = true;
                                    }
                                    //�e���`���[�W���n�߂�A�ꔭ������
                                    if (isShot == false)
                                    {
                                        Shot();
                                    }
                                    else
                                    {
                                        if (!ChajeSE)
                                        {
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK); // �e���������ˎ����߉��Đ�
                                            ChajeSE = true;
                                        }
                                    }
                                    //MP�����炵�Ă��Ȃ�������
                                    if (!isMinuse)
                                    {
                                        //�e���������^�C�~���O
                                        if (time >= 3.2f)
                                        {
                                            if (!ShotSE)
                                            {
                                                SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ��Đ�
                                                ShotSE = true;
                                            }
                                            MP -= 20;
                                            isMinuse = true;
                                            ChajeSE = false;
                                            animator.speed = 1.0f;
                                        }
                                    }
                                }
                                if (animeInfo.normalizedTime > 1.0f)
                                {
                                    isMinuse = false;
                                    AnimeStop = false;
                                    ChajeSE = true;
                                    isShot = false;
                                    ShotSE = false;
                                    isShot = false;
                                    OwnGetPlayerPos = false;
                                    time = 0.0f;
                                    isCharge = false;
                                    animator.SetBool("Shot", false);
                                    SetState(EnemyState.Idel);
                                }
                            }

                        }

                    }
                }

                break;

                //������������K�v�ɂȂ邩������Ȃ�
                {
                    ////�E�ɉ������ă`���[�W�������U��
                    //case EnemyState.ChargeBackLeftAttack:
                    //    {
                    //        time += Time.deltaTime;
                    //        //�����̈ʒu���擾���Ă��Ȃ�������擾����

                    //        if (time >= 1.5f)
                    //        {
                    //            MoveStop = true;
                    //            isCharge = true;
                    //            //�G�Ƃ̋������߂������ꍇ����ĉ������U��������                    

                    //            if (isShot == false)
                    //            {
                    //                Shot();
                    //            }
                    //            else
                    //            {
                    //                if (ChajeSE)
                    //                {
                    //                    //SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK,
                    //                    //       SoundManager.Instance.GetSEVolume()); // ���������ߍĐ�
                    //                }
                    //            }
                    //            animator.SetBool("Shot", true);
                    //            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                    //            {
                    //                if (animeInfo.normalizedTime > 0.3f)
                    //                {
                    //                    animator.speed = 0.0f;
                    //                    AnimeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    //                }
                    //            }
                    //            if (!isMinuse)
                    //            {
                    //                if (time >= 3.0f)
                    //                {
                    //                    isMinuse = true;
                    //                    ChajeSE = false;
                    //                    MP -= 20;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                animator.speed = 1.0f;
                    //                animator.Play("Shot", 0, AnimeTime);

                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (MoveStop == false)
                    //            {
                    //                //�ǂɐG��Ă��Ȃ���Έړ�
                    //                transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                    //                animator.SetBool("Step", true);
                    //            }
                    //        }

                    //        if (time > 4.0f)
                    //        {
                    //            animator.SetBool("Step", false);
                    //            animator.SetBool("Shot", false);
                    //            isMinuse = false;
                    //            OwnGetPlayerPos = false;
                    //            ChajeSE = true;
                    //            MoveStop = false;
                    //            isShot = false;
                    //            time = 0.0f;
                    //            isCharge = false;
                    //            SetState(EnemyState.Idel);
                    //        }

                    //    }

                    //    break;
                    ////���ɉ������ĉ������U��
                    //case EnemyState.ChargeBackRightAttack:
                    //    {
                    //        time += Time.deltaTime;
                    //        //�����̈ʒu���擾���Ă��Ȃ�������擾����

                    //        if (time >= 1.5f)
                    //        {
                    //            MoveStop = true;
                    //            isCharge = true;
                    //            //�G�Ƃ̋������߂������ꍇ����ĉ������U��������

                    //            animator.SetBool("Shot", true);

                    //            if (isShot == false)
                    //            {
                    //                Shot();
                    //            }
                    //            else
                    //            {
                    //                if (ChajeSE)
                    //                {
                    //                    //SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK,
                    //                    //       SoundManager.Instance.GetSEVolume()); // ���������ߍĐ�
                    //                }

                    //            }
                    //            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                    //            {
                    //                if (animeInfo.normalizedTime > 0.2f)
                    //                {
                    //                    animator.speed = 0.0f;
                    //                    AnimeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    //                }
                    //            }

                    //            if (!isMinuse)
                    //            {
                    //                if (time >= 3.0f)
                    //                {
                    //                    MP -= 20;
                    //                    isMinuse = true;
                    //                    ChajeSE = false;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                animator.speed = 1.0f;
                    //                animator.Play("Shot", 0, AnimeTime);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (MoveStop == false)
                    //            {
                    //                //�ǂɐG��Ă��Ȃ���Έړ�
                    //                transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                    //                animator.SetBool("Step", true);
                    //            }
                    //        }

                    //        if (time > 4.0f)
                    //        {
                    //            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                    //            {
                    //                if (animeInfo.normalizedTime > 0.2f)
                    //                {
                    //                    isMinuse = false;
                    //                    ChajeSE = true;
                    //                    OwnGetPlayerPos = false;
                    //                    animator.SetBool("Step", false);
                    //                    animator.SetBool("Shot", false);
                    //                    isShot = false;
                    //                    time = 0.0f;
                    //                    isCharge = false;
                    //                    SetState(EnemyState.Idel);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    break;
                }

            //�v���C���[��_���ĉ������U��
            case EnemyState.AimShoot:
                {
                    //��ɃW�����v����B
                    if (IsJunp == false)
                    {
                        Janp();
                    }
                    //�w��̍����Ŏ~�܂�B    
                    if (transform.position.y > AimHight)
                    {
                        animator.SetBool("Shot", true);
                        isAim = true;
                        rb.useGravity = false;
                        rb.velocity = Vector3.zero;
                        time += Time.deltaTime;

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                        {
                            if (animeInfo.normalizedTime > 0.7f)
                            {
                                //��x�����A�j���[�V�������~�߂�
                                if (!AnimeStop)
                                {
                                    animator.speed = 0.0f;
                                    AnimeStop = true;
                                }
                                if (time > 2.2f)
                                {
                                    if (isShot == false)
                                    {
                                        Shot();
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ��Đ�
                                        animator.speed = 1.0f;
                                        MP -= 20;
                                    }
                                }
                            }
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                time = 0.0f;
                                isAim = false;
                                isShot = false;
                                AnimeStop = false;
                                rb.useGravity = true;
                                animator.SetBool("Shot", false);
                                animator.SetBool("Jump", false);
                                SetState(EnemyState.Idel);
                            }


                        }
                    }
                }
                break;
            //�ߐڍU��
            case EnemyState.CloseAttack:
                {
                    if (!getrnd)
                    {
                        rnd = Random.Range(1, 4);
                        getrnd = true;
                    }
                    if (playerPos.x < transform.position.x)
                    {
                        if (isRotation)
                        {
                            isLeft = true;
                            isRotation = false;
                        }


                    }
                    else if (playerPos.x > transform.position.x)
                    {
                        if (isRotation)
                        {
                            isLeft = false;
                            isRotation = false;
                        }
                    }
                    if (Pos <= 2.4f)
                    {
                        //�ߐڍU�����s�����ɋ������߂��ꍇ
                        animator.SetBool("Punch", true);
                        fixation = true;
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Punch"))
                        {
                            if (animeInfo.normalizedTime > 0.76f && animeInfo.normalizedTime < 0.9f)
                            {
                                OnColliderAttack();
                                ELeftattack.StartAttack(51, 30, false,true);
                            }
                            else
                            {
                                OffColliderAttack();
                            }
                        }

                        if (!isMinuse)
                        {
                            MP -= 10;
                            isMinuse = true;
                        }
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Punch"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                isMinuse = false;
                                fixation = false;
                                isRotation = true;
                                if (rnd == 1)
                                {
                                    SetState(EnemyState.Idel);
                                    getrnd = false;
                                    animator.SetBool("Punch", false);
                                }
                                else
                                {
                                    SetState(EnemyState.BackJump);
                                }
                            }

                        }

                    }
                    else if (Pos > 2.4f)
                    {
                        //�ߐڍU�����s�����ɋ����������ꍇ
                        SetState(EnemyState.DashCloseAttack);
                    }

                }

                break;
            //�v���C���[�ɋ߂Â��ċߐڍU��
            case EnemyState.DashCloseAttack:
                {
                    //�v���C���[���G��荶���ɂ���
                    if (playerPos.x < transform.position.x)
                    {
                        if (isRotation)
                        {
                            isLeft = true;
                        }
                        isRotation = false;
                        if (!fixation)
                        {
                            transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Run", true);
                        }
                    }

                    //�v���C���[���G���E���ɂ���
                    if (playerPos.x > transform.position.x)
                    {
                        if (isRotation)
                        {
                            isLeft = false;
                        }
                        isRotation = false;
                        if (!fixation)
                        {
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Run", true);
                        }
                    }

                    if (Pos < 2.4f || isMinuse)
                    {
                        animator.SetBool("Punch", true);
                        fixation = true;
                        if (animeInfo.normalizedTime > 0.8f && animeInfo.normalizedTime < 0.9f)
                        {
                            OnColliderAttack();
                            ELeftattack.StartAttack(51, 30, false, true);
                        }
                        else
                        {
                            OffColliderAttack();
                        }
                        if (!isMinuse)
                        {
                            Debug.Log("�_�b�V���ʏ�U���ł̌���");
                            MP -= 10;
                            isMinuse = true;
                        }

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Punch"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                isMinuse = false;
                                fixation = false;
                                isRotation = true;
                                getrnd = false;
                                animator.SetBool("Punch", false);
                                animator.SetBool("Run", false);
                                SetState(EnemyState.Idel);
                            }


                        }

                    }

                }

                break;
            //�W�����v���s���ċߐڍU��
            case EnemyState.JunpAttack:
                {
                    if (!GetPostiton)
                    {
                        animator.SetBool("JumpAttack", true);
                        PlayerPos = playerObject.transform.position;
                        PlayerPos.y = 10.0f;
                        if (playerPos.x < transform.position.x)
                        {
                            PlayerPos.x += 2.0f;
                            fixation = true;
                            isLeft = true;
                        }
                        else
                        {
                            fixation = true;
                            isLeft = false;
                            PlayerPos.x -= 2.0f;
                        }

                        rb.useGravity = false;
                        GetPostiton = true;
                    }

                    if (transform.position.y < PlayerPos.y && !rb.useGravity)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, PlayerPos, 20.0f * Time.deltaTime);
                        IsJunp = true;
                    }
                    else
                    {
                        if (!rb.useGravity)
                        {
                            rb.velocity = Vector3.down * JumpAttackPower;
                        }
                        rb.useGravity = true;
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.EnemyAirAttack"))
                        {
                            NotRotation = true;
                            OnColliderJumpAttack();
                            Jumptattack.StartAttack(10, 30, true, true);

                            if (!IsJunp && !NoEffect)
                            {
                                Instantiate(AttackEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z), Quaternion.Euler(-90, 0, 90));
                                NoEffect = true;
                            }

                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                NoEffect = false;
                                GetPostiton = false;
                                fixation = false;
                                NotRotation = false;
                                OffColliderJumpAttack();
                                animator.SetBool("JumpAttack", false);
                                SetState(EnemyState.Idel);

                            }
                        }

                    }


                    {
                        //if (!JumpAttack)
                        //{
                        //    if (IsJunp == false)
                        //    {
                        //        Janp();
                        //        JumpAttack = true;
                        //    }
                        //}


                        //if (IsJunp == false)
                        //{
                        //    animator.SetBool("Run", true);
                        //}

                        //if (!isAttack)
                        //{
                        //    //�v���C���[���G��荶���ɂ��鎞
                        //    if (playerPos.x < transform.position.x)
                        //    {
                        //        transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                        //    }

                        //    if (playerPos.x > transform.position.x)
                        //    {
                        //        transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                        //    }
                        //}


                        //if (JumpAttack)
                        //{
                        //    if (playerObject.transform.position.y - transform.position.y < 2.0f)
                        //    {
                        //        isAttack = true;
                        //        animator.SetBool("Punch", true);
                        //        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Punch"))
                        //        {
                        //            if (animeInfo.normalizedTime > 0.8f && animeInfo.normalizedTime < 0.9f)
                        //            {
                        //                OnColliderAttack();
                        //                ELeftattack.StartAttack(51, 30);
                        //            }
                        //            else
                        //            {
                        //                OffColliderAttack();
                        //            }
                        //            if (animeInfo.normalizedTime > 1.0f)
                        //            {
                        //                isAttack = false;
                        //                JumpAttack = false;
                        //                isRotation = true;
                        //                MP -= 10;
                        //                SetState(EnemyState.Idel);
                        //                OffColliderAttack();
                        //                animator.SetBool("Punch", false);
                        //                animator.SetBool("Run", false);
                        //                animator.SetBool("Jump", false);

                        //            }
                        //        }
                        //    }

                        //}
                    }
                }

                break;
            //�U�����󂯂ă_�E��
            case EnemyState.fall:
                {
                    animator.speed = 1.0f;
                    if (isFall)
                    {
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Fall"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                //hp��0�Ȃ�N����Ȃ��Ȃ�
                                if (Hp >= 0)
                                {
                                    isFall = false;
                                    animator.SetBool("StandUp", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.StandUp"))
                        {
                            isFall = true;
                            animator.SetBool("StandUp", false);
                            animator.SetBool("Fall", false);
                            animator.SetBool("Run", false);
                            animator.SetBool("Punch", false);
                            animator.SetBool("Jump", false);
                            animator.SetBool("Shot", false);
                            animator.SetBool("Step", false);
                            animator.SetBool("Walk", false);
                            animator.SetBool("BackJump", false);
                            Reset();
                            SetState(EnemyState.Idel);

                            //�N���Ă���r��hp��0�ɂȂ�����
                            if (Hp <= 0)
                            {
                                animator.SetBool("Fall", true);
                                SetState(EnemyState.fall);
                            }
                        }
                    }

                }

                break;
            //�A���ߐڍU��
            case EnemyState.Succession:
                {
                    if (!OwnGetPlayerPos)
                    {
                        if (playerPos.x < transform.position.x)
                        {
                            Left = true;
                        }
                        else
                        {
                            Left = false;
                        }
                        OwnGetPlayerPos = true;
                    }

                    if (islowered)
                    {
                        if (Left)
                        {
                            fixation = true;
                            if (isRotation)
                            {
                                isLeft = true;
                            }
                            isRotation = false;

                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.1"))
                            {
                                if (animeInfo.normalizedTime > 0.6f && animeInfo.normalizedTime < 0.8f)
                                {
                                    transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                            else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.5"))
                            {
                                if (animeInfo.normalizedTime > 0.0f && animeInfo.normalizedTime < 0.6f)
                                {
                                    transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                            else
                            {
                                if (animeInfo.normalizedTime > 0.1f && animeInfo.normalizedTime < 1.0f)
                                {
                                    transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                        }
                        else
                        {
                            fixation = true;
                            if (isRotation)
                            {
                                isLeft = false;
                            }
                            isRotation = false;
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.1"))
                            {
                                if (animeInfo.normalizedTime > 0.6f && animeInfo.normalizedTime < 0.8f)
                                {
                                    transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                            else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.5"))
                            {
                                if (animeInfo.normalizedTime > 0.0f && animeInfo.normalizedTime < 0.6f)
                                {
                                    transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                            else
                            {
                                if (animeInfo.normalizedTime > 0.1f && animeInfo.normalizedTime < 1.0f)
                                {
                                    transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!isBack)
                        {
                            islowered = true;
                        }
                        if (Left)
                        {
                            if (MoveStop == false)
                            {
                                //�ǂɐG��Ă��Ȃ���Έړ�
                                transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                                animator.SetBool("Step", true);
                            }
                            else
                            {
                                islowered = true;
                            }
                        }
                        else
                        {
                            if (MoveStop == false)
                            {
                                //�ǂɐG��Ă��Ȃ���Έړ�
                                transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                animator.SetBool("Step", true);
                            }
                            else
                            {
                                islowered = true;
                            }
                        }
                    }


                    //�A�j���[�V�����؂�ւ�
                    if (islowered)
                    {
                        OnColliderRightAttack();
                        OnColliderAttack();

                        animator.SetBool("Succession", true);
                        ELeftattack.StartAttack(3, 15, false, true);
                        ERightattack.StartAttack(3, 15, false, true);

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.1"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("2", true);
                                ELeftattack.StartAttack(3, 15, false, true);
                                ERightattack.StartAttack(3, 15, false, true);
                            }
                        }
                        else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.2"))
                        {

                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("3", true);
                                ELeftattack.StartAttack(3, 15, false, true);
                                ERightattack.StartAttack(3, 15, false, true);
                            }
                        }
                        else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.3"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("4", true);
                                ELeftattack.StartAttack(3, 15, false, true);
                                ERightattack.StartAttack(3, 10, false, true);
                            }
                        }
                        else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.4"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("5", true);
                                ELeftattack.StartAttack(3, 15, false, true);
                                ERightattack.StartAttack(3, 50, false, true);
                            }
                        }
                        else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.5"))
                        {
                            if (!isMinuse)
                            {
                                MP -= 40;
                                isMinuse = true;
                            }
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                animator.SetBool("Succession", false);
                                isRotation = true;
                                fixation = false;
                                islowered = false;
                                isMinuse = false;
                                OwnGetPlayerPos = false;
                                isBack = true;
                                animator.SetBool("2", false);
                                animator.SetBool("3", false);
                                animator.SetBool("4", false);
                                animator.SetBool("5", false);
                                animator.SetBool("Step", false);
                                OffColliderRightAttack();
                                OffColliderAttack();
                                Left = false;
                                SetState(EnemyState.Idel);
                            }
                        }
                    }

                }
                break;

            case EnemyState.ChageAttack:
                {
                    if (Pos <= 2.4f || fixation)
                    {
                        fixation = true;
                        animator.SetBool("KyouAttack", true);
                        if (!isMinuse)
                        {
                            Debug.Log("�ʏ�U���ł̌���");
                            MP -= 30;
                            isMinuse = true;
                        }
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.KyouAttack"))
                        {
                            if (animeInfo.normalizedTime > 0.6f && animeInfo.normalizedTime < 0.9)
                            {
                                OnColliderRightAttack();
                                ERightattack.StartAttack(51, 50, false, true);
                            }
                            else
                            {
                                OffColliderRightAttack();
                            }

                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                SetState(EnemyState.Idel);
                                isRotation = true;
                                fixation = false;
                                isMinuse = false;
                                animator.SetBool("KyouAttack", false);
                                animator.SetBool("Run", false);
                            }
                        }
                    }
                    else
                    {
                        if (playerPos.x < transform.position.x)
                        {
                            if (isRotation)
                            {
                                isLeft = true;
                            }
                            isRotation = false;
                            if (!fixation)
                            {
                                transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                animator.SetBool("Run", true);
                            }
                        }

                        if (playerPos.x > transform.position.x)
                        {
                            if (isRotation)
                            {
                                isLeft = false;
                            }
                            isRotation = false;
                            if (!fixation)
                            {
                                transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                                animator.SetBool("Run", true);
                            }
                        }

                    }

                }
                break;

            case EnemyState.BackJump:
                {
                    if (!JumpAttack)
                    {
                        if (IsJunp == false)
                        {
                            Janp();
                            JumpAttack = true;
                        }
                    }
                    if (playerPos.x < transform.position.x)
                    {
                        if (MoveStop == false && IsJunp)
                        {
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                        }
                        else
                        {
                            if (!IsJunp)
                            {
                                animator.SetBool("BackJump", false);
                                animator.SetBool("Punch", false);
                                getrnd = false;
                                JumpAttack = false;
                                SetState(EnemyState.Idel);
                            }

                        }
                    }
                    else
                    {
                        if (MoveStop == false && IsJunp)
                        {
                            Debug.Log("���ɃW�����v�ŉ�����");
                            transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                        }
                        else
                        {
                            if (!IsJunp)
                            {
                                animator.SetBool("BackJump", false);
                                animator.SetBool("Run", false);
                                animator.SetBool("Punch", false);
                                getrnd = false;
                                JumpAttack = false;
                                SetState(EnemyState.Idel);
                            }
                        }
                    }

                }
                break;

            case EnemyState.Blazing:
                {
                    BlazingShot = true;

                    //�v���C���[���G��荶���ɋ�����
                    if (playerPos.x < transform.position.x)
                    {
                        animator.SetBool("Shot", true);

                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                        {
                            if (animeInfo.normalizedTime > 0.8f)
                            {

                                if (!AnimeStop)
                                {
                                    animator.speed = 0.0f;
                                    time = 1.0f;
                                    AnimeStop = true;
                                }
                                if (!isShot)
                                {
                                    time += Time.deltaTime;

                                    if (time >= 0.5f)
                                    {
                                        if (count <= 2)
                                        {
                                            Shot();
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ��Đ�
                                            time = 0.0f;
                                            count++;
                                            Debug.Log(count + "�e�̐�");
                                            if (!isMinuse)
                                            {
                                                MP -= 10;
                                            }
                                        }
                                        else
                                        {
                                            isMinuse = true;
                                            isShot = true;
                                        }
                                    }
                                }
                                else
                                {
                                    animator.speed = 1.0f;
                                    if (animeInfo.normalizedTime > 1.0f)
                                    {
                                        isMinuse = false;
                                        animator.SetBool("Shot", false);
                                        BlazingShot = false;
                                        AnimeStop = false;
                                        count = 0;
                                        isShot = false;
                                        isBack = true;
                                        SetState(EnemyState.Idel);
                                    }
                                }

                            }
                        }
                    }

                    //�v���C���[���G���E���ɋ�����
                    if (playerPos.x > transform.position.x)
                    {
                        animator.SetBool("Shot", true);
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                        {
                            if (animeInfo.normalizedTime > 0.8f)
                            {

                                if (!AnimeStop)
                                {
                                    animator.speed = 0.0f;
                                    time = 1.0f;
                                    AnimeStop = true;
                                }
                                if (!isShot)
                                {
                                    time += Time.deltaTime;
                                    if (time >= 0.5f)
                                    {
                                        if (count <= 2)
                                        {
                                            Shot();
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // �ʏ�U���e���ˉ��Đ�
                                            time = 0.0f;
                                            count++;
                                            Debug.Log(count + "�e�̐�");
                                            if (!isMinuse)
                                            {
                                                MP -= 10;
                                            }
                                        }
                                        else
                                        {
                                            isMinuse = true;
                                            isShot = true;
                                        }
                                    }
                                }
                                else
                                {
                                    animator.speed = 1.0f;
                                    if (animeInfo.normalizedTime > 1.0f)
                                    {
                                        isMinuse = false;
                                        animator.SetBool("Shot", false);
                                        BlazingShot = false;
                                        AnimeStop = false;
                                        count = 0;
                                        isShot = false;
                                        isBack = true;
                                        SetState(EnemyState.Idel);
                                    }
                                }


                            }
                        }
                    }


                }
                break;

            case EnemyState.FlipKick:
                {
                    animator.SetBool("FlipKick", true);

                    if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Flip"))
                    {
                        if (animeInfo.normalizedTime > 0.1f && animeInfo.normalizedTime < 0.5f)
                        {
                            OnColliderJumpAttack();
                            Jumptattack.StartAttack(10, 30, false, true);
                        }
                        else
                        {
                            OffColliderJumpAttack();
                        }



                        if (animeInfo.normalizedTime > 1.0f)
                        {

                            animator.SetBool("FlipKick", false);
                            SetState(EnemyState.Idel);
                        }
                    }

                }
                break;

            case EnemyState.ThreeAttack:
                {
                    animator.SetBool("ThreeAttack", true);
                    animator.speed = 1.5f;
                    if (animeInfo.nameHash == Animator.StringToHash("Base Layer.ThreeRight"))
                    {
                        if(!fixation)
                        {
                            if (playerPos.x < transform.position.x)
                            {
                                isLeft = true;
                            }
                            else
                            {
                                isLeft = false;
                            }
                        }
                        fixation = true;


                        if (animeInfo.normalizedTime > 0.4f && animeInfo.normalizedTime < 0.6f)
                        {
                            OnColliderRightAttack();
                            ERightattack.StartAttack(10, 10, false, true);
                            if (isLeft)
                            {
                                transform.position -= Vector3.right * 2.0f * Time.deltaTime;
                            }
                            else
                            {
                                transform.position += Vector3.right * 2.0f * Time.deltaTime;
                            }
                        }
                        else
                        {
                            OffColliderRightAttack();
                        }
                        if (animeInfo.normalizedTime > 0.9f)
                        {
                            fixation = false;
                        }

                    }
                    else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.ThreeLeft"))
                    {
                        if (!fixation)
                        {
                            if (playerPos.x < transform.position.x)
                            {
                                isLeft = true;
                            }
                            else
                            {
                                isLeft = false;
                            }

                        }
                        fixation = true;

                        if (animeInfo.normalizedTime > 0.4f && animeInfo.normalizedTime < 0.6f)
                        {
                            OnColliderAttack();
                            ELeftattack.StartAttack(10, 10, false, true);
                            if (isLeft)
                            {
                                transform.position -= Vector3.right * 2.0f * Time.deltaTime;
                            }
                            else
                            {
                                transform.position += Vector3.right * 2.0f * Time.deltaTime;
                            }
                        }
                        else
                        {
                            OffColliderAttack();
                        }
                        if(animeInfo.normalizedTime > 1.0f)
                        {
                            fixation = false;
                            animator.SetBool("ThreeAttack", false);
                            animator.speed = 1.0f;
                            SetState(EnemyState.FlipKick);
                        }
                    }
                }

                break;

        }

        //�v���C���[�̌����ɉ�]
        if (playerObject)
        {
            var dir = _target.position - _self.position;
            // �^�[�Q�b�g�̕����ւ̉�]
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);

            // ��]�␳
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            _self.rotation = lookAtRotation * offsetRotation;
            eulerAngles = transform.eulerAngles; // ���[�J���ϐ��Ɋi�[
            if (enemyState != EnemyState.AimShoot && enemyState != EnemyState.JunpAttack && !NotRotation) 
            {
                eulerAngles.z = 0; // ���[�J���ϐ��Ɋi�[�����l���㏑��
            }
            if (enemyState == EnemyState.ChargeLongAttack ||
                enemyState == EnemyState.ChargeBackLeftAttack ||
                enemyState == EnemyState.ChargeBackRightAttack ||
                fixation == true)
            {
                if (isLeft)
                {
                    eulerAngles.y = 0; // ���[�J���ϐ��Ɋi�[�����l���㏑��
                }
                else
                {
                    eulerAngles.y = 180.0f;
                }

            }
            transform.eulerAngles = eulerAngles; // ���[�J���ϐ�����
        }

        //�f�o�b�O�Ƃ���D�L�[�������ƃW�����v�ł���
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (IsJunp == false)
            {
                Janp();
            }
        }

        

        if (Hp <= 0) 
        {
            animator.SetBool("Fall", true);
            zoom.Camerazoom(transform.position);
            animator.speed = 0.1f;
        }

    }

    //�G�̃X�e�[�g��؂�ւ���
    public void SetState(EnemyState state)
    {
        enemyState = state;
    }

    public EnemyState GetState()
    {
        return enemyState;
    }


    public void MinusHP(int Attack)
    {
        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_HITNORMALATK); // �ʏ�U�����Đ�



        HitFadeBlink(Color.red);
               
        if (enemyState != EnemyState.fall)
        {
            Count += 1;
        }
        if (Count >= DownCount)
        {
            OffColliderAttack();

            OffColliderRightAttack();
            if (enemyState != EnemyState.ChageAttack && enemyState != EnemyState.Succession &&
                enemyState != EnemyState.FlipKick && enemyState != EnemyState.ThreeAttack) 
            {
                animator.SetBool("Fall", true);
                SetState(EnemyState.fall);
            }
            if (!isMinuse)
            {
                B.IsFall(true);
            }
            Count = 0;
        }
        
        if (MP >= Attack)
        {
            MP -= Attack;
        }
        else
        {
            MP = 0;
            Attack -= MP;
            Hp -= Attack;
        }
        Debug.Log("�G�̗̑�: " + Hp);
    }

    //- �GHP�̎擾
    public int GetEnemyMaxHp()
    {
        return EnemyMAxHp;
    }
    //- ���݂̓GHP�̎擾
    public int GetEnemyHp()
    {
        return Hp;
    }
    //- ���݂̓GMp�̎擾
    public int GetEnemyMp()
    {
        return MP;
    }
    //- Mp�̍ő�l�̎擾
    public int GetEnemyMaxMp()
    {
        return EnemyMaxMP;
    }

    //�G�̃W�����v����
    private void Janp()
    {
        if (enemyState != EnemyState.JunpAttack)
        {
            rb.velocity = Vector3.up * JunpPower;
        }
        else
        {
            rb.velocity = Vector3.up * JumpAttackPower;
        }
        
        if(enemyState != EnemyState.BackJump)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("BackJump", true);
        }

        IsJunp = true;
    }

    //�G�̒e���o�������鏈��
    private void Shot()
    {
        Vector3 bulletPos = tarett.transform.position;
        bullet = Instantiate(Bullet, bulletPos, transform.rotation);
        B = bullet.GetComponent<Bullet>();
        if (isCharge)
        {
            B.Creeate(-7.0f, true, 2.0f, false, 0.0f);
            isShot = true;
        }
        else if (isAim)
        {
            B.Creeate(-7.0f, false, 0.0f, true, 0.0f);
            isShot = true;
        }
        else if(BlazingShot)
        {
          
           B.Creeate(-7.0f, false, 0.0f, true, 0.0f);
            
        }
        else
        {
            B.Creeate(-7.0f, false, 0.0f, false, 0.0f);
            isShot = true;
        }
    }

    //�I�u�W�F�N�g�Ƃ̓����蔻��
    private void OnCollisionEnter(Collision collision)
    {
        //�n�ʂɐG��Ă��邩
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJunp = false;
        }

        //�ǂɐG��Ă��邩
        if (collision.gameObject.CompareTag("Wall"))
        {
            MoveStop = true;
        }

    }
    private void OffColliderAttack()
    {
        attackLeftCollider.enabled = false;
        LeftatackObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnColliderAttack()
    {
        attackLeftCollider.enabled = true;
        LeftatackObject.GetComponent<MeshRenderer>().enabled = true;
    }
    private void OffColliderRightAttack()
    {
        attackRightCollider.enabled = false;
        RightattackObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnColliderRightAttack()
    {
        attackRightCollider.enabled = true;
       RightattackObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OffColliderJumpAttack()
    {
        JumpattackCollider.enabled = false;
       JumptackObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnColliderJumpAttack()
    {
        JumpattackCollider.enabled = true;
       JumptackObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public bool GetisLeft()
    {
        return isLeft;
    }
    private void OnCollisionExit(Collision collision)
    {
        //�ǂ��痣�ꂽ��
        if (collision.gameObject.CompareTag("Wall"))
        {
            MoveStop = false;
        }
    }
    private void Reset()
    {
        MoveStop = false;
        isMinuse = false;
        fixation = false;
        isShot = false;
        isBack = true;
        isCharge = false;
        isAim = false;
        rb.useGravity = true;
        JumpAttack = false;
        isRotation = true;
        OwnGetPlayerPos = false;
        ChajeSE = false;
        ShotSE = false;
        BlazingShot = false;
        AnimeStop = false;
        getrnd = false;
    }

    public void HitFadeBlink(Color color)
    {

        Debug.Log("�Ԃ��Ȃ�");
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOTween.To(() => Color.red, c => _material.SetColor(PROPERTY_COLOR, c), color, 0.1f));
        _seq.Append(DOTween.To(() => color, c => _material.SetColor(PROPERTY_COLOR, c), Color.white, 0.1f));
        _seq.Play();

    }
}
