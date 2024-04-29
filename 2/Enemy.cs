using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//-----
//制作：矢野
//追記：髙橋、小野
//概要：敵の動きの処理
///----

public class Enemy : MonoBehaviour
{
    // 追記：小野
    [Header("会話イベント")]
    [SerializeField] private TalkEventManager talkEvent = null;

    [SerializeField]
    [Header("EnemyBaseスクリプト取得")]
    private EnemyBase Ebase;


    [SerializeField]
    [Header("敵の弾")]
    private GameObject Bullet;

    [SerializeField]
    [Header("弾を発射する場所")]
    private GameObject tarett;

   
    private int EnemyMAxHp = 200;

   
    private int Hp = 200;

    private int EnemyMaxMP = 120;

    private int MP = 120;

    //仮プレイヤーの取得
    [Header("プレイヤーの取得")]
    [SerializeField] private Player player;

    //仮プレイヤーの取得
    [Header("プレイヤーの取得")]
    [SerializeField] private GameObject playerObject;


    [Header("狙い撃ちの際の高さ")]
    [SerializeField] private float AimHight;

    //プレイヤーの位置を入れる変数
    Vector3 playerPos;

    Vector3 PlayerPos;
    //敵のスピード
    private float Speed;
    //敵のダッシュのスピード
    private float DashSpeed;


    [Header("敵の次の行動を考える時間")]
    public float IdelTime = 3.0f;

    //時間を使って処理を遅らせるときに使う変数
    private float time = 0.0f;


    [Header("攻撃を行っている時間")]
    public float AttackTime = 3.0f;

  

   
    //RigidBodyを取得
    private Rigidbody rb;

    [SerializeField, Header("敵のジャンプ力")]
    private float JunpPower;

    [SerializeField, Header("ジャンプ近接のジャンプ力")]
    private float JumpAttackPower;

    //ジャンプ中か
    private bool IsJunp = false;

    //壁にぶつかっているか
    private bool MoveStop = false;
    //ジャンプ攻撃をしているか
    private bool JumpAttack = false;
    //回転し終わっているか
    private bool isRotation = true;
    //遠距離攻撃でチャージするか
    private bool isCharge = false;
    //遠距離攻撃で狙うか
    private bool isAim = false;
    private bool isMinuse = false;
    //当たり判定オブジェクト
    [Header("右攻撃当たり判定オブジェクト")]
    [SerializeField] private GameObject RightattackObject;
    private Collider attackRightCollider;
    private EnemyAttack ERightattack;
    [Header("左攻撃当たり判定オブジェクト")]
    [SerializeField] private GameObject LeftatackObject;
    private Collider attackLeftCollider;
    private EnemyAttack ELeftattack;
    [Header("ジャンプ攻撃当たり判定オブジェクト")]
    [SerializeField] private GameObject JumptackObject;
    private Collider JumpattackCollider;
    private EnemyAttack Jumptattack;

    // 自身のTransform
    [SerializeField] private Transform _self;

    // ターゲットのTransform
    [SerializeField] private Transform _target;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 _forward = Vector3.forward;

    private bool isLeft = true;

    [SerializeField, Header("敵のAnimatr")]
    private Animator animator;
    [SerializeField, Header("CameraShackスクリプト取得")]
    private CameraShake camerashake;
    AnimatorStateInfo animeInfo;
    private bool ChajeSE = false;
    private bool ShotSE = false;

    [SerializeField, Header("ダウンまでの回数")]
    private int DownCount;
    private int Count = 0;
   
    private bool NotRotation = false;

    [SerializeField, Header("殴りしかしない")]
    private bool CloseAttaack;

    [SerializeField, Header("歩きしかしない")]
    private bool Move;

    [Header("弾を撃ち続ける(trueなら)")]
    [SerializeField] private bool isFaier = false;

    [Header("Chage弾を撃ち続ける(trueなら)")]
    [SerializeField] private bool Chage = false;

    [Header("連続攻撃しかしない(trueなら)")]
    [SerializeField] private bool succession = false;

    [Header("チャージ近接しかしない(trueなら)")]
    [SerializeField] private bool ChageAttack = false;

    [Header("ジャンプ攻撃しかしない(trueなら)")]
    [SerializeField] private bool jumpAttack = false;

    [Header("行動不可(trueなら)")]
    [SerializeField] private bool NoStart = false;
    [Header("連射(trueなら)")]
    [SerializeField] private bool isBlazing = false;

    [Header("上蹴り(trueなら)")]
    [SerializeField]private bool Flipkick = false;

    [Header("3段階攻撃(trueなら)")]
    [SerializeField] private bool ThreeAttack = false;

    private bool BlazingShot = false;

    private bool fixation = false;
    private bool Left = false;
    private bool OwnGetPlayerPos = false;
    private bool islowered = false;
    private bool AnimeStop = false;

    private Vector3 eulerAngles;

    private static readonly int PROPERTY_COLOR = Shader.PropertyToID("_Color");
    [SerializeField, Header("モデルのRenderer")]
    private Renderer _renderer;
    private Material _material;
    private Sequence _seq;

   

    private bool GetPostiton = false;
    [SerializeField, Header("エフェクト")]
    private GameObject AttackEffect;
    private bool NoEffect = false;
    private bool getrnd = false;
    private bool changedirection = false;
    

    //敵のステート
    public enum EnemyState
    {
        //次の行動を選ぶ
        Idel,
        //近づく
        Move,
        //近接攻撃
        CloseAttack,
        //チャージ近接攻撃
        ChageAttack,
        //ダッシュ近距離攻撃
        DashCloseAttack,
        //ジャンプして近接攻撃をする
        JunpAttack,
        //遠距離攻撃
        LongAttack,
        //右に下がって遠距離攻撃
        BackLeftAttack,
        //左に下がって遠距離攻撃
        BackRightAttack,
        //チャージ遠距離攻撃
        ChargeLongAttack,
        //右に下がってチャージ遠距離攻撃
        ChargeBackLeftAttack,
        //左に下がってチャージ遠距離攻撃
        ChargeBackRightAttack,
        //弾連射
        Blazing,
        //狙い撃ち
        AimShoot,
        //連続攻撃
        Succession,
        //倒れる
        fall,
        //バックジャンプ
        BackJump,
        //上蹴り
        FlipKick,
        //3連続攻撃
        ThreeAttack,
    }
    [Header("敵の現在のステート")]
    public EnemyState enemyState;
    private int rnd;
    private bool isShot;
    private bool isFall = true;
    private bool isBack;
    [SerializeField, Header("弾")]
    GameObject bullet;
    [SerializeField, Header("弾スクリプト")]
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
        // 追記：小野
        // 会話イベントがあるとき、終わったらこの後の処理を行う
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
            //次の攻撃選択
            case EnemyState.Idel:
                {
                    //少し時間を空ける  
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
                                    //次の行動をランダムで決める
                                    //HPが半分になった時
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
                                    //HPが半分よりも小さい時
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
            //プレイヤーに近づく
            case EnemyState.Move:
                {
                    animator.SetBool("Walk", true);
                    //プレイヤーが敵より左側に居たら左側に移動
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

                    //プレイヤーが敵より右側に居たら右側に移動

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
            //遠距離攻撃
            case EnemyState.LongAttack:
                {
                    //プレイヤーが敵より左側に居たら
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
                                        Debug.Log("球を撃つ");
                                        Shot();
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音
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

                    //プレイヤーが敵より右側に居たら
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
                                        //Debug.Log("球を撃つ");
                                        Shot();
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音
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
                                //敵との距離が近い時
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

            //右に下がって遠距離攻撃
            case EnemyState.BackLeftAttack:
                {
                    time += Time.deltaTime;
                    //自分の位置を取得していなかったら取得する
                    if (time >= 1.0f)
                    {
                        animator.SetBool("Shot", true);
                        //敵との距離が近かった場合離れて遠距離攻撃をする
                        isLeft = true;
                        if (!isShot)
                        {
                            Shot();
                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音
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
                            //壁に触れていなければ移動
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Step", true);
                        }
                    }

                }

                break;
            //左に下がって遠距離攻撃
            case EnemyState.BackRightAttack:
                {
                    time += Time.deltaTime;
                    //自分の位置を取得していなかったら取得する

                    if (time >= 1.0f)
                    {
                        isLeft = false;
                        //敵との距離が近かった場合離れて遠距離攻撃をする

                        animator.SetBool("Shot", true);

                        time = 0.0f;
                        if (!isShot)
                        {
                            Shot();
                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音
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
                            //壁に触れていなければ移動
                            transform.position += Vector3.right * DashSpeed * Time.deltaTime;
                            animator.SetBool("Step", true);
                        }
                    }
                }

                break;
            //チャージして遠距離攻撃
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
                    //プレイヤーが敵より左側に居たら
                    if (playerPos.x < transform.position.x)
                    {
                        //if (Pos > 8.0f || isCharge)
                        {
                            animator.SetBool("Shot", true);
                            isCharge = true;

                            //拳を引くところまでアニメーションを動かす。
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.7f)
                                {
                                    //一度だけアニメーションを止める
                                    if (!AnimeStop)
                                    {
                                        animator.speed = 0.0f;
                                        AnimeStop = true;
                                    }
                                    //弾をチャージし始める、一発分だけ
                                    if (isShot == false)
                                    {
                                        Shot();
                                    }
                                    else
                                    {
                                        if (!ChajeSE)
                                        {
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK); // 弾遠距離発射時溜め音
                                            ChajeSE = true;
                                        }
                                    }
                                    //MPを減らしていなかったら
                                    if (!isMinuse)
                                    {
                                        //弾を撃ったタイミング
                                        if (time >= 3.2f)
                                        {
                                            if (!ShotSE)
                                            {
                                                SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音
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
                    //プレイヤーが敵より右側に居たら
                    if (playerPos.x > transform.position.x)
                    {
                        //if (Pos > 8.0f || isCharge)
                        {
                            animator.SetBool("Shot", true);
                            isCharge = true;

                            //拳を引くところまでアニメーションを動かす。
                            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Shot"))
                            {
                                if (animeInfo.normalizedTime > 0.7f)
                                {
                                    //一度だけアニメーションを止める
                                    if (!AnimeStop)
                                    {
                                        animator.speed = 0.0f;
                                        AnimeStop = true;
                                    }
                                    //弾をチャージし始める、一発分だけ
                                    if (isShot == false)
                                    {
                                        Shot();
                                    }
                                    else
                                    {
                                        if (!ChajeSE)
                                        {
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK); // 弾遠距離発射時溜め音再生
                                            ChajeSE = true;
                                        }
                                    }
                                    //MPを減らしていなかったら
                                    if (!isMinuse)
                                    {
                                        //弾を撃ったタイミング
                                        if (time >= 3.2f)
                                        {
                                            if (!ShotSE)
                                            {
                                                SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音再生
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

                //もしかしたら必要になるかもしれない
                {
                    ////右に下がってチャージ遠距離攻撃
                    //case EnemyState.ChargeBackLeftAttack:
                    //    {
                    //        time += Time.deltaTime;
                    //        //自分の位置を取得していなかったら取得する

                    //        if (time >= 1.5f)
                    //        {
                    //            MoveStop = true;
                    //            isCharge = true;
                    //            //敵との距離が近かった場合離れて遠距離攻撃をする                    

                    //            if (isShot == false)
                    //            {
                    //                Shot();
                    //            }
                    //            else
                    //            {
                    //                if (ChajeSE)
                    //                {
                    //                    //SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_TAMELONGATK,
                    //                    //       SoundManager.Instance.GetSEVolume()); // 遠距離溜め再生
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
                    //                //壁に触れていなければ移動
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
                    ////左に下がって遠距離攻撃
                    //case EnemyState.ChargeBackRightAttack:
                    //    {
                    //        time += Time.deltaTime;
                    //        //自分の位置を取得していなかったら取得する

                    //        if (time >= 1.5f)
                    //        {
                    //            MoveStop = true;
                    //            isCharge = true;
                    //            //敵との距離が近かった場合離れて遠距離攻撃をする

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
                    //                    //       SoundManager.Instance.GetSEVolume()); // 遠距離溜め再生
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
                    //                //壁に触れていなければ移動
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

            //プレイヤーを狙って遠距離攻撃
            case EnemyState.AimShoot:
                {
                    //上にジャンプする。
                    if (IsJunp == false)
                    {
                        Janp();
                    }
                    //指定の高さで止まる。    
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
                                //一度だけアニメーションを止める
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
                                        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音再生
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
            //近接攻撃
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
                        //近接攻撃を行う時に距離が近い場合
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
                        //近接攻撃を行う時に距離が遠い場合
                        SetState(EnemyState.DashCloseAttack);
                    }

                }

                break;
            //プレイヤーに近づいて近接攻撃
            case EnemyState.DashCloseAttack:
                {
                    //プレイヤーが敵より左側にいる
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

                    //プレイヤーが敵より右側にいる
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
                            Debug.Log("ダッシュ通常攻撃での減少");
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
            //ジャンプを行って近接攻撃
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
                        //    //プレイヤーが敵より左側にいる時
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
            //攻撃を受けてダウン
            case EnemyState.fall:
                {
                    animator.speed = 1.0f;
                    if (isFall)
                    {
                        if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Fall"))
                        {
                            if (animeInfo.normalizedTime > 1.0f)
                            {
                                //hpが0なら起きれなくなる
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

                            //起きている途中hpが0になったら
                            if (Hp <= 0)
                            {
                                animator.SetBool("Fall", true);
                                SetState(EnemyState.fall);
                            }
                        }
                    }

                }

                break;
            //連続近接攻撃
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
                                //壁に触れていなければ移動
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
                                //壁に触れていなければ移動
                                transform.position -= Vector3.right * DashSpeed * Time.deltaTime;
                                animator.SetBool("Step", true);
                            }
                            else
                            {
                                islowered = true;
                            }
                        }
                    }


                    //アニメーション切り替え
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
                            Debug.Log("通常攻撃での減少");
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
                            Debug.Log("左にジャンプで下がる");
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

                    //プレイヤーが敵より左側に居たら
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
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音再生
                                            time = 0.0f;
                                            count++;
                                            Debug.Log(count + "弾の数");
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

                    //プレイヤーが敵より右側に居たら
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
                                            SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_NORMALATKFIRE); // 通常攻撃弾発射音再生
                                            time = 0.0f;
                                            count++;
                                            Debug.Log(count + "弾の数");
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

        //プレイヤーの向きに回転
        if (playerObject)
        {
            var dir = _target.position - _self.position;
            // ターゲットの方向への回転
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);

            // 回転補正
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            _self.rotation = lookAtRotation * offsetRotation;
            eulerAngles = transform.eulerAngles; // ローカル変数に格納
            if (enemyState != EnemyState.AimShoot && enemyState != EnemyState.JunpAttack && !NotRotation) 
            {
                eulerAngles.z = 0; // ローカル変数に格納した値を上書き
            }
            if (enemyState == EnemyState.ChargeLongAttack ||
                enemyState == EnemyState.ChargeBackLeftAttack ||
                enemyState == EnemyState.ChargeBackRightAttack ||
                fixation == true)
            {
                if (isLeft)
                {
                    eulerAngles.y = 0; // ローカル変数に格納した値を上書き
                }
                else
                {
                    eulerAngles.y = 180.0f;
                }

            }
            transform.eulerAngles = eulerAngles; // ローカル変数を代入
        }

        //デバッグとしてDキーを押すとジャンプできる
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

    //敵のステートを切り替える
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
        SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_ENEMY_HITNORMALATK); // 通常攻撃音再生



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
        Debug.Log("敵の体力: " + Hp);
    }

    //- 敵HPの取得
    public int GetEnemyMaxHp()
    {
        return EnemyMAxHp;
    }
    //- 現在の敵HPの取得
    public int GetEnemyHp()
    {
        return Hp;
    }
    //- 現在の敵Mpの取得
    public int GetEnemyMp()
    {
        return MP;
    }
    //- Mpの最大値の取得
    public int GetEnemyMaxMp()
    {
        return EnemyMaxMP;
    }

    //敵のジャンプ処理
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

    //敵の弾を出現させる処理
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

    //オブジェクトとの当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        //地面に触れているか
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJunp = false;
        }

        //壁に触れているか
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
        //壁から離れたか
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

        Debug.Log("赤くなる");
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOTween.To(() => Color.red, c => _material.SetColor(PROPERTY_COLOR, c), color, 0.1f));
        _seq.Append(DOTween.To(() => color, c => _material.SetColor(PROPERTY_COLOR, c), Color.white, 0.1f));
        _seq.Play();

    }
}
