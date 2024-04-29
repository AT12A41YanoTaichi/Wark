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
        Idel,       //�l����
        Alent,      //�v���C���[�̕��������邾���A��Ɍx����Ԉȏ�̎��ɍs��
        Frieze,     //�������Ȃ�
        Teisatu,    //��@
        Nears,      //�߂Â�
        Leave,      //�����
        Attack,     //�ߐڍU��
        Shot,       //�������U��
        Fall,       //�|���
    }
    public EnemyState State;

    Vector3 dir;
    //�ړI�n
    Vector3 destination;
    public NavMeshAgent agent;
    //private  Transform targetTransform;
    private GameObject Player;
    private int AttackCount = 0;
    [Header("EnemyBase�擾")]
    [SerializeField] EnemyBase Ebase;
    [Header("Animator�擾")]
    [SerializeField] Animator animator;
    AnimatorStateInfo animeInfo;
    private bool agentStop;
    Quaternion SetRotatin;
    private Vector3 StatPos;
    [Header("�U�������蔻��I�u�W�F�N�g")]
    [SerializeField] private GameObject AttackObject;
    private Collider AttackCollider;
    private EnemyAttack enemyAttack;
    [Header("�U���J�n�p�̃t���O")]
    [SerializeField] public bool StopState;
    private float time;
    [Header("���̏ꏊ��I�Ԃ܂ł̒�~�̎���")]
    [SerializeField] private float NextPointTime;
    [Header("�U����d���̎���")]
    [SerializeField] private float FriezeTime;

    public PlayableDirector playableDirector;
    [Header("�ˌ��I���p�̃t���O")]
    [SerializeField] private bool Shotfnish = false;
    [Header("�r�[���e��Start�ʒu���擾")]
    [SerializeField] private GameObject BulletStart;
    [Header("�r�[���e��end�ʒu���擾")]
    [SerializeField] private GameObject BulletEnd;
    private int laser_count;
    [Header("FallSmoke�擾")]
    [SerializeField] private ParticleSystem FallSmoke;

    [SerializeField] [Range(0, 1)] private float dropRate = 0.1f;//�A�C�e���o���m��
    [SerializeField] private Item itemPrefab;
    [SerializeField] private int number = 1;//�A�C�e���̏o����

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
            //���̍s����I������
            case EnemyState.Idel:
                

                break;

            //�v���C���[�̕�������������
            case EnemyState.Alent:

                dir = (Player.transform.position - transform.position).normalized;
                //y���͕ύX���Ȃ�
                dir.y = 0;
                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

                break;

            //�����_���ȏꏊ�Ɉړ�����
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
                        Debug.Log("���̏ꏊ��T��");
                        SetState(EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                        time = 0.0f;
                    }
                    
                }

                break;

            //�U���̍d��
            case EnemyState.Frieze:

                time += Time.deltaTime;

                if (FriezeTime <= time)
                {
                    StopState = true;
                    time = 0.0f;
                }

                break;

            //�v���C���[��ǂ�������
            case EnemyState.Nears:
                agent.SetDestination(GetDestination());
                //�ړI�n�̕������擾����
                dir = (Player.transform.position - transform.position).normalized;
                //y���͕ύX���Ȃ�
                dir.y = 0;

                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

                break;

            //�v���C���[���痣���
            case EnemyState.Leave:

                dir = (transform.position - Player.transform.position).normalized;
                //y���͕ύX���Ȃ�
                dir.y = 0;

                SetDestination(transform.position + dir * 3.0f);
                agent.SetDestination(GetDestination());

                break;

            //�v���C���[�ɋߐڍU�����s��
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

            //�v���C���[�ɉ������U�����s��
            case EnemyState.Shot:

                dir = (Player.transform.position - transform.position).normalized;
                //y���͕ύX���Ȃ�
                dir.y = 0;
                SetRotatin = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);
                if (Shotfnish == true)
                {
                    Shotfnish = false;
                    AttackFnishe();
                }

                break;

            //HP���[���ɂȂ�������|���
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
            //���̍s����I������
            case EnemyState.Idel:
               
                //agent.isStopped = true;

                break;

            //�v���C���[�̕�������������
            case EnemyState.Alent:
                agent.isStopped = true;

                break;

            //�����_���ȏꏊ�Ɉړ�����
            case EnemyState.Teisatu:
               
                agent.isStopped = false;
                //�~�^�����̃����_���ȍ��W���擾����
                var randDestination = Random.insideUnitCircle * 20;
                SetDestination(StatPos + new Vector3(randDestination.x, 0, randDestination.y));
                
                break;

            //�U���̍d��
            case EnemyState.Frieze:
                //agent.speed = 0;
                agent.isStopped = true;
                break;

            //�v���C���[��ǂ�������
            case EnemyState.Nears:
                agent.isStopped = false;
                agent.speed = 5;
                SetDestination(Target);
                break;

            //�v���C���[���痣���
            case EnemyState.Leave:
                agent.isStopped = false;
                break;

            //�v���C���[�ɋߐڍU�����s��
            case EnemyState.Attack:
                StopState = false;
                agent.isStopped = false;
                break;

            //�v���C���[�ɉ������U�����s��
            case EnemyState.Shot:
                StopState = false;
                agent.isStopped = false;
                playableDirector.Play();

                break;

            //HP���[���ɂȂ�������|���
            case EnemyState.Fall:
               // agent.speed = 0.0f;
                StopState = false;
                agent.isStopped = true;
                break;
        }

    }
    //�����蔻��Off
    private void OffColliderAttack()
    {
        AttackCollider.enabled = false;
        AttackObject.GetComponent<MeshRenderer>().enabled = false;
    }

    //�����蔻��On
    private void OnColliderAttack()
    {
        AttackCollider.enabled = true;
        AttackObject.GetComponent<MeshRenderer>().enabled = true;
    }

    //���̖ړI�n��ݒ�
    public void SetDestination(Vector3 positin)
    {
        destination = positin;
    }

    //�A�b�v�f�[�g���łɂ��̒l��Ԃ�
    public Vector3 GetDestination()
    {
        return destination;
    }

    //�U���I������
    public bool StopAttack()
    {
        return StopState;
    }

    [Header("�e�v���n�u�̎擾")]
    [SerializeField] private GameObject BulletPrefab;
    [Header("���ˈʒu�̎擾")]
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

    //�v���C���[���b�N�I���A�C�R���\��
    public void OnAicon()
    {
        Player.GetComponent<PlayerBase>().OnAimAicon();
    }
    //�v���C���[���b�N�I���A�C�R����\��
    public void OffAicon()
    {
        Player.GetComponent<PlayerBase>().OffAimAicon();
    }
    //�ˌ��I��
    public void ShotFinsh()
    {
        Shotfnish = true;
    }

    //�A�C�e���h���b�v
    private void DropIfNeeded()
    {
        //�A�C�e���h���b�v���I��������J��Ԃ��Ȃ�
        if(_isDropInvoked)
        {
            return;
        }

        _isDropInvoked = true;

        //�h���b�v�����Ⴉ������h���b�v���Ȃ�
        if(Random.Range(0,1f) >= dropRate)
        {
            return;
        }

        //�w�肵�������h���b�v����
        for (var i = 0; i < number; i++) 
        {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.Initialize();
        }

    }


    //�m������
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;
        Debug.Log(fProbabilityRate + "�����_�����l");
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
