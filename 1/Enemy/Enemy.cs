using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idel,       //考える
        Alent,      //プレイヤーの方向を見るだけ、主に警戒状態以上の時に行う
        Frieze,     //何もしない
        Teisatu,    //偵察
        Nears,      //近づく
        Leave,      //離れる
        Attack,     //近接攻撃
        Shot,       //遠距離攻撃
        Fall,       //倒れる
    }
    public EnemyState State;

    Vector3 dir;
    //目的地
    Vector3 destination;
    public NavMeshAgent agent;
    //private  Transform targetTransform;
    private GameObject Player;
    private int AttackCount = 0;
    [Header("EnemyBase取得")]
    [SerializeField] EnemyBase Ebase;
    [Header("Animator取得")]
    [SerializeField] Animator animator;
    AnimatorStateInfo animeInfo;
    private bool agentStop;
    Quaternion SetRotatin;
    private Vector3 StatPos;
    [Header("攻撃当たり判定オブジェクト")]
    [SerializeField] private GameObject AttackObject;
    private Collider AttackCollider;
    private EnemyAttack enemyAttack;
    [Header("攻撃開始用のフラグ")]
    [SerializeField] public bool StopState;
    private float time;
    [Header("次の場所を選ぶまでの停止の時間")]
    [SerializeField] private float NextPointTime;
    [Header("攻撃後硬直の時間")]
    [SerializeField] private float FriezeTime;

    public PlayableDirector playableDirector;
    [Header("射撃終了用のフラグ")]
    [SerializeField] private bool Shotfnish = false;
    [Header("ビーム弾のStart位置を取得")]
    [SerializeField] private GameObject BulletStart;
    [Header("ビーム弾のend位置を取得")]
    [SerializeField] private GameObject BulletEnd;
    private int laser_count;
    [Header("FallSmoke取得")]
    [SerializeField] private ParticleSystem FallSmoke;

    [SerializeField] [Range(0, 1)] private float dropRate = 0.1f;//アイテム出現確率
    [SerializeField] private Item itemPrefab;
    [SerializeField] private int number = 1;//アイテムの出現数

    private bool _isDropInvoked;

   

    // Start is called before the first frame update
    void Start()
    {
        StatPos = transform.position;
        agent.speed = 5;
        enemyAttack = AttackObject.GetComponent<EnemyAttack>();
        AttackCollider = AttackObject.GetComponent<Collider>();
        OffColliderAttack();
        State = Enemy.EnemyState.Teisatu;
        var randDestination = Random.insideUnitCircle * 10;
        SetDestination(StatPos + new Vector3(randDestination.x, 0, randDestination.y));
        StopState = true;
        playableDirector = GetComponent<PlayableDirector>();
        Player = GameObject.Find("Paladin WProp J Nordstrom (1)");

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
       
        if (!Player)
        {
            Player = GameObject.Find("Paladin WProp J Nordstrom (1)");
        }

        animeInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (State)
        {
            //次の行動を選択する
            case EnemyState.Idel:
                

                break;

            //プレイヤーの方向を向くだけ
            case EnemyState.Alent:

                dir = (Player.transform.position - transform.position).normalized;
                //y軸は変更しない
                dir.y = 0;
                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

                break;

            //ランダムな場所に移動する
            case EnemyState.Teisatu:

                var nextPoint = agent.steeringTarget;
                Vector3 targetDir = nextPoint - transform.position;

                SetRotatin = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, SetRotatin ,200f * Time.deltaTime);
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (angle <= 30.0f) 
                {
                    agent.speed = 5; 
                    agent.SetDestination(GetDestination());
                }
                else
                {
                   // agent.speed = 0;
                }

                if (agent.remainingDistance <= 1.0f)
                {
                    time += Time.deltaTime;
                    if (NextPointTime < time) 
                    {
                        Debug.Log("次の場所を探索");
                        SetState(EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                        time = 0.0f;
                    }
                    
                }

                break;

            //攻撃の硬直
            case EnemyState.Frieze:

                time += Time.deltaTime;

                if (FriezeTime <= time)
                {
                    StopState = true;
                    time = 0.0f;
                }

                break;

            //プレイヤーを追いかける
            case EnemyState.Nears:
                agent.SetDestination(GetDestination());
                //目的地の方向を取得する
                dir = (Player.transform.position - transform.position).normalized;
                //y軸は変更しない
                dir.y = 0;

                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

                break;

            //プレイヤーから離れる
            case EnemyState.Leave:

                dir = (transform.position - Player.transform.position).normalized;
                //y軸は変更しない
                dir.y = 0;

                SetDestination(transform.position + dir * 3.0f);
                agent.SetDestination(GetDestination());

                break;

            //プレイヤーに近接攻撃を行う
            case EnemyState.Attack:
                //StopState = false;
                if (!agentStop)
                {
                    //agent.speed = 0;
                    agentStop = true;
                }
                
                if (agent.velocity.magnitude < 1.0f) 
                {
                    animator.SetBool("Attack", true);
                }

                if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Attack"))
                {
                    if (animeInfo.normalizedTime > 0.4f && animeInfo.normalizedTime < 0.8f)
                    {
                        animator.speed = 1.5f;
                        agent.speed = 15;
                        OnColliderAttack();
                    }
                    if (animeInfo.normalizedTime > 1.0f)
                    {
                        animator.speed = 1.0f;
                        agent.speed = 5; 
                        AttackFnishe();
                        OffColliderAttack();
                        agentStop = false;
                        animator.SetBool("Attack", false);
                    }
                }

                break;

            //プレイヤーに遠距離攻撃を行う
            case EnemyState.Shot:

                dir = (Player.transform.position - transform.position).normalized;
                //y軸は変更しない
                dir.y = 0;
                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);
                if (Shotfnish == true)
                {
                    Shotfnish = false;
                    AttackFnishe();
                }

                break;

            //HPがゼロになったから倒れる
            case EnemyState.Fall:
                animator.SetBool("Fall", true);
                if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Fall"))
                {
                    FallSmoke.Play();
               
                    if (animeInfo.normalizedTime > 1.0f)
                    {
                        Destroy(gameObject);
                        Destroy(FallSmoke);
                        DropIfNeeded();
                    }
                }
                break;
        }
        if (Ebase.GetHP() <= 0) 
        {
            State = EnemyState.Fall;
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            SetState(EnemyState.Shot, new Vector3(0.0f, 0.0f, 0.0f));
        }

    }

    public void AttackFnishe()
    {
        switch (Ebase.condition())
        {
            case 1:

                if (AttackCount <= 0)
                {
                    if (Probability(80))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else
                {
                    State = EnemyState.Frieze;
                    AttackCount = 0;
                }
               

                break;

            case 2:

                if (AttackCount <= 0)
                {
                    if (Probability(50))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if(AttackCount <= 1)
                {
                    if (Probability(70))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if(AttackCount <= 2)
                {
                    if (Probability(90))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else
                {
                    State = EnemyState.Frieze;
                    AttackCount = 0;
                }
                  

                break;

            case 3:

                if (AttackCount <= 0)
                {
                    if (Probability(20))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if (AttackCount <= 1)
                {
                    if (Probability(40))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if(AttackCount <= 2)
                {
                    if (Probability(50))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if (AttackCount <= 3)
                {
                    if (Probability(70))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else if (AttackCount <= 4)
                {
                    if (Probability(90))
                    {
                        State = EnemyState.Frieze;
                        AttackCount = 0;
                    }
                    else
                    {
                        StopState = true;
                        AttackCount++;
                    }
                }
                else
                {
                    State = EnemyState.Frieze;
                    AttackCount = 0;
                }

                break;
        }

    }

    public EnemyState GetState()
    {
        return State;
    }

    public void SetState(EnemyState state, Vector3 Target)
    {
        State = state;

        switch (State)
        {
            //次の行動を選択する
            case EnemyState.Idel:
               
                //agent.isStopped = true;

                break;

            //プレイヤーの方向を向くだけ
            case EnemyState.Alent:
                agent.isStopped = true;

                break;

            //ランダムな場所に移動する
            case EnemyState.Teisatu:
               
                agent.isStopped = false;
                //円型内部のランダムな座標を取得する
                var randDestination = Random.insideUnitCircle * 20;
                SetDestination(StatPos + new Vector3(randDestination.x, 0, randDestination.y));
                
                break;

            //攻撃の硬直
            case EnemyState.Frieze:
                //agent.speed = 0;
                agent.isStopped = true;
                break;

            //プレイヤーを追いかける
            case EnemyState.Nears:
                agent.isStopped = false;
                agent.speed = 5;
                SetDestination(Target);
                break;

            //プレイヤーから離れる
            case EnemyState.Leave:
                agent.isStopped = false;
                break;

            //プレイヤーに近接攻撃を行う
            case EnemyState.Attack:
                StopState = false;
                agent.isStopped = false;
                break;

            //プレイヤーに遠距離攻撃を行う
            case EnemyState.Shot:
                StopState = false;
                agent.isStopped = false;
                playableDirector.Play();

                break;

            //HPがゼロになったから倒れる
            case EnemyState.Fall:
               // agent.speed = 0.0f;
                StopState = false;
                agent.isStopped = true;
                break;
        }

    }
    //当たり判定Off
    private void OffColliderAttack()
    {
        AttackCollider.enabled = false;
        AttackObject.GetComponent<MeshRenderer>().enabled = false;
    }

    //当たり判定On
    private void OnColliderAttack()
    {
        AttackCollider.enabled = true;
        AttackObject.GetComponent<MeshRenderer>().enabled = true;
    }

    //次の目的地を設定
    public void SetDestination(Vector3 positin)
    {
        destination = positin;
    }

    //アップデート内でにその値を返す
    public Vector3 GetDestination()
    {
        return destination;
    }

    //攻撃終了判定
    public bool StopAttack()
    {
        return StopState;
    }

    [Header("弾プレハブの取得")]
    [SerializeField] private GameObject BulletPrefab;
    [Header("発射位置の取得")]
    [SerializeField] private GameObject point;
    public void LaserBullet()
    {
        var dir = (Player.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(BulletPrefab, point.transform.position,transform.rotation);
        bullet.transform.rotation = Quaternion.LookRotation(dir,Vector3.up);
        bullet.GetComponent<Rigidbody>().AddForce(dir * 60.0f, ForceMode.Impulse);
    }

    private void LaserBeam()
    {
        var dir = (transform.forward).normalized;
        switch (laser_count)
        {
            case 1:
                BulletEnd.transform.position = Vector3.Lerp(BulletEnd.transform.position, transform.position + dir * 40.0f + new Vector3(0.0f, Player.transform.position.y, 0.0f), Time.deltaTime);
                break;

            case 2:
                BulletStart.transform.position = Vector3.Lerp(BulletStart.transform.position, BulletEnd.transform.position - dir * 0.5f, 0.05f);
                break;
        }
    }

    public void LaserTrigger()
    {
        switch (laser_count)
        {
            case 0:
                laser_count++;

                break;

            case 1:
                laser_count++;
                break;

            case 2:
                BulletEnd.transform.position = Vector3.zero;
                BulletStart.transform.position = Vector3.zero;
                laser_count = 0;

                break;
        }

    }

    //プレイヤーロックオンアイコン表示
    public void OnAicon()
    {
        Player.GetComponent<PlayerBase>().OnAimAicon();
    }
    //プレイヤーロックオンアイコン非表示
    public void OffAicon()
    {
        Player.GetComponent<PlayerBase>().OffAimAicon();
    }
    //射撃終了
    public void ShotFinsh()
    {
        Shotfnish = true;
    }

    //アイテムドロップ
    private void DropIfNeeded()
    {
        //アイテムドロップが終了したら繰り返さない
        if(_isDropInvoked)
        {
            return;
        }

        _isDropInvoked = true;

        //ドロップ率より低かったらドロップしない
        if(Random.Range(0,1f) >= dropRate)
        {
            return;
        }

        //指定した個数分ドロップする
        for (var i = 0; i < number; i++) 
        {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.Initialize();
        }

    }


    //確率判定
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;
        Debug.Log(fProbabilityRate + "ランダム数値");
        if (fPercent ==100.0f&&fProbabilityRate == fPercent)
        {
            return true;
        }
        else if(fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
