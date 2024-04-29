using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.UI;



[RequireComponent(typeof(NavMeshAgent))]

public class EnemyTankAI : MonoBehaviour
{

    public enum EnemyAIState
    {
        Idel,       //考える
        Teisatu,    //偵察  
        gether,     //集まる
        Attack      //攻撃
    }
    public EnemyAIState State;
    public AudioClip soundHit;
    AudioSource audioSource;


    [SerializeField]
    private int maxHp = 5;
    //　敵のHP
    [SerializeField]
    private int HP;

    //　HP表示用UI
    [SerializeField]
    private GameObject HPUI;
    //　HP表示用スライダー
    private Slider hpSlider;

    [SerializeField] public GameObject discovery;

    public Transform central;
    public NavMeshAgent agent;
    public GameObject Expo;

    public GoalBowl Bowl;

    [SerializeField] float radius = 10;


    [SerializeField] float wantTime = 5f;

    [SerializeField] float time = 0f;

    GameObject turret;

    Vector3 playerPos;

    //目的地
    Vector3 destination;


    public Transform targetTransform;

    GameObject player;
    //float distance;


    public float TurretSpeed;

    private bool isgether = false;

    Vector3 pos;
    bool animecheck = false;


    public GameObject Turret;
    public Animator animator; //Animatorコンポーネント 

    public bool DamageCheck;


    public CountNum _countNum;

    public bool Yarareta;

    void Start()
    {
        agent.autoBraking = false;
        State = EnemyAIState.Teisatu;

        player = GameObject.Find("Tank");

        SetState(EnemyAIState.Teisatu);
        audioSource = GetComponent<AudioSource>();
        DamageCheck = false;

        HP = maxHp;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
        discovery.SetActive(false);
    }



    void Update()
    {
        if (Bowl.SetStateGether() == true)
        {
            State = EnemyAIState.gether;
        }

        if (State == EnemyAIState.Teisatu)
        {

            //目的地の方向を取得する
            var dir = (GetDestination() - transform.position).normalized;
            //y軸は変更しない
            dir.y = 0;

            Quaternion SetRotatin = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

            if (!agent.pathPending && agent.remainingDistance < 1.5f)
            {
                StopPoint();

            }

        }

        if (State == EnemyAIState.Attack)
        {
            Invoke("falseDiscovery", 2.0f);
            if (targetTransform == null)
            {
                SetState(EnemyAIState.Teisatu);
            }


            SetDestination(targetTransform.position);
            agent.SetDestination(GetDestination());

            //目的地の方向を取得する
            var dir = (GetDestination() - transform.position).normalized;
            //y軸は変更しない
            dir.y = 0;

            Quaternion SetRotatin = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);
        }

        if (State == EnemyAIState.gether)
        {
            Invoke("falseDiscovery", 2.0f);
            if (isgether == false)
            {
                SetDestination(player.transform.position);
                agent.SetDestination(GetDestination());
                isgether = true;
            }
            //目的地の方向を取得する
            var dir = (GetDestination() - transform.position).normalized;
            //y軸は変更しない
            dir.y = 0;
            Quaternion SetRotatin = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                SetState(EnemyAIState.Teisatu);
                GotoNextPoint();
                Bowl.ReturnIsGether(false);
                isgether = false;
            }


        }


        if (HP <= 0)
        {

            animator.SetBool("Die", true);
            agent.speed = 0.0f;
            agent.isStopped = true;
            if (animecheck == false)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180.0f, transform.rotation.z);
                animecheck = true;
            }
        }


        GetState();





    }


    public void GotoNextPoint()
    {

        agent.isStopped = false;


        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        pos = central.position;
        pos.x += posX;
        pos.z += posZ;

        agent.destination = pos;

        SetDestination(pos);

    }





    void StopPoint()
    {


        time += Time.deltaTime;
        animator.SetBool("sakuteki", true);
        if (time > wantTime)
        {
            animator.SetBool("sakuteki", false);
            GotoNextPoint();
            time = 0;
        }
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {
            HP--;
            SetHp(HP);
            audioSource.PlayOneShot(soundHit);
            DamageCheck = true;
            SetState(EnemyAIState.Attack, player.transform);

        }
        if (HP <= 0)
        {
            Destroy(gameObject, 3.0f);
            Debug.Log("死ぬ～");
            HideStatusUI();
            Instantiate(Expo, transform.position, transform.rotation);

            if (Yarareta == false)
            {
                _countNum.EbiDesNum++;
                Yarareta = true;
            }

        }

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "EffectBullet")
        {
            HP--;
            SetHp(HP);
            animator.SetBool("damezi", true);
            animator.SetBool("damezi", false);
            SetState(EnemyAIState.Attack, player.transform);

        }
        if (HP <= 0)
        {
            Destroy(gameObject, 3.0f);
            Debug.Log("死ぬ～");

            HideStatusUI();
            if (Yarareta == false)
            {
                _countNum.EbiDesNum++;
                Yarareta = true;
            }
        }
    }



    public void BulletCollision()
    {
        //HP--;
        //audioSource.PlayOneShot(soundHit);
        //SetState(EnemyAIState.Attack, player.transform);
        //DamageCheck = true;
    }


    //引数(ステートどれにするのか、追いかける場所はどこなのか)
    public void SetState(EnemyAIState tempState, Transform targetObject = null)
    {
        State = tempState;
        if (tempState == EnemyAIState.Idel)
        {
            agent.isStopped = true;
        }

        else if (tempState == EnemyAIState.Attack)
        {

            animator.SetBool("kougeki", true);
            Bowl.ReturnIsGether(false);
            isgether = false;
            targetTransform = targetObject;
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
        }

        else if (tempState == EnemyAIState.gether)
        {

            targetTransform = player.transform;
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;

        }

    }

    //別のプログラムで敵のステートを変えるために使う
    public EnemyAIState GetState()
    {
        return State;
    }

    //次に向かう場所の場所を指定する
    public void SetDestination(Vector3 positin)
    {
        destination = positin;
    }

    //アップデート内でにその値を返す
    public Vector3 GetDestination()
    {
        return destination;
    }
    public void SetHp(int hp)
    {
        this.HP = hp;

        //　HP表示用UIのアップデート
        UpdateHPValue();
    }

    public int GetHp()
    {
        return HP;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    //　死んだらHPUIを非表示にする
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }

    private void falseDiscovery()
    {
        discovery.SetActive(false);
    }
}