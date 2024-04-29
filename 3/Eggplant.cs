using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NavMeshAgent使うときに必要
using UnityEngine.AI;
//オブジェクトにNavMeshAgentコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class Eggplant : MonoBehaviour
{
    public enum NasuState
    {
        Teisatu,
        Attack
    }

    public NasuState nasuState;

    public Transform central;
    public NavMeshAgent agent;

    //ランダムで決める数値の最大値
    [SerializeField] float radius = 10;
    private float Pointtime = 0f;
    private float wantTime = 2f;


    private float time = 0f;
    [SerializeField] bool IsCharge = false;
    GameObject player;

    //プレイヤーの発見時の位置
    Vector3 playerPos;
    //プレイヤーの現在の位置
    Vector3 playerPosnow;

    public Transform targetTransform;

    public float speed = 8f;

    public bool IsHit = false;
    Vector3 pos;

    public Animator animator; //Animatorコンポーネント 

    //目的地
    Vector3 destination;
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

    public bool Damagecheck = false;

    public CountNum _countNum;

    public bool Yarareta;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //目標地点に近づいても速度を落とさなくなる
        agent.autoBraking = false;
        nasuState = NasuState.Teisatu;
        player = GameObject.Find("Tank");
        IsCharge = false;
        //animator = GetComponent<Animator>();
        GotoNextPoint();
        animator.SetBool("sakuteki", true);
        HP = maxHp;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;

    }


    public void GotoNextPoint()
    {
        //NavMeshAgentのストップを解除
        agent.isStopped = false;

        //ランダムでposx、zを決めるradiusは範囲の最大値
        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        pos = central.position;
        pos.x += posX;
        pos.z += posZ;


        agent.SetDestination(pos);


    }

    public void StopPoint()
    {
        agent.isStopped = true;
        Pointtime += Time.deltaTime;
        if (Pointtime > wantTime)
        {
            animator.SetBool("sakuteki", false);
            GotoNextPoint();
            Pointtime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        GetState();
        if (nasuState == NasuState.Teisatu)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.8f)
            {
                // エージェントが現目標地点に近づいてきたら、
                //次の目標地点を選択します
                StopPoint();
                animator.SetBool("sakuteki", true);
            }

        }

        if (nasuState == NasuState.Attack)
        {
            agent.isStopped = false;
            //animator.SetBool("Atack", true);
            if (targetTransform == null)
            {
                animator.SetBool("Atack", false);
                nasuState = NasuState.Teisatu;
            }

            animator.SetBool("Atack", true);
            SetDestination(targetTransform.position);
            agent.SetDestination(GetDestination());

            //目的地の方向を取得する
            var dir = (GetDestination() - transform.position).normalized;
            //y軸は変更しない
            dir.y = 0;

            Quaternion SetRotatin = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, SetRotatin, agent.angularSpeed * 0.1f * Time.deltaTime);
        }


        if(HP <= 0)
        {
            animator.SetBool("Die", true);
            agent.speed = 0.0f;
            agent.isStopped = true;
        }

    }

    public void SetState(NasuState tempState, Transform targetObject = null)
    {
        nasuState = tempState;

        if (nasuState == NasuState.Teisatu)
        {
            animator.SetBool("sakuteki", false);
        }

        if (nasuState == NasuState.Attack)
        {
            targetTransform = targetObject;

            SetDestination(targetTransform.position);
        }
    }

    public NasuState GetState()
    {
        return nasuState;
    }

    public void SetDestination(Vector3 positin)
    {
        destination = positin;
    }

    //アップデート内でにその値を返す
    public Vector3 GetDestination()
    {
        return destination;
    }



    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {
            HP--;
            SetHp(HP);
            SetState(NasuState.Attack, player.transform);
        }
        if (HP <= 0)
        {
            //animator.SetBool("Die", true);
            Destroy(gameObject, 3.0f);
            Debug.Log("死ぬ～");

            HideStatusUI();

            if(Yarareta == false)
            {
                _countNum.NasuDesNum++;
                Yarareta = true;
            }
            
        }

    }

    public void BulletCollision()
    {
        //HP--;
        //Damagecheck = true;

        //if (HP <= 0)
        //{
        //    Destroy(gameObject, 3.0f);
        //    Debug.Log("死ぬ～");



        //}
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "EffectBullet")
        {
            HP--;
            SetHp(HP);
            SetState(NasuState.Attack, player.transform);
        }
        if (HP <= 0)
        {
            Destroy(gameObject,3.0f);
            Debug.Log("死ぬ～");

            HideStatusUI();
            if (Yarareta == false)
            {
                _countNum.NasuDesNum++;
                Yarareta = true;
            }
        }
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
}