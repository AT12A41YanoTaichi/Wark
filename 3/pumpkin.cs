using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class pumpkin : MonoBehaviour
{
    [SerializeField]
    private int maxHp = 15;
    //　敵のHP
    [SerializeField]
    private int HP;

    public NavMeshAgent nav;
    public float trackingRange;
    public float StopRange;
    public GameObject player;
    Vector3 playerPos;
    float distance;
    public Animator animator; //Animatorコンポーネント 

    //　HP表示用UI
    [SerializeField]
    private GameObject HPUI;
    //　HP表示用スライダー
    private Slider hpSlider;

    public CountNum _countNum;

    public enum PumpikinState
    {
        Stop,
        Attack,
    }

    public PumpikinState pumState;



    // ディゾルブ ================================
    public GameObject[] obj;
    Material[] mats;
    public bool Yarareta;
    public float DissoNum;
    //============================================

    // Start is called before the first frame update
    void Start()
    {
        pumState = PumpikinState.Stop;
        nav.isStopped = true;
        animator.SetBool("aruki", false);
        HP = maxHp;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;

        for (int j = 0; j < obj.Length; j++)
        {

            mats = obj[j].GetComponent<SkinnedMeshRenderer>().materials;
            for (int i = 0; i < obj[j].GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
            {
                mats[0].SetFloat("_disso_strength", DissoNum);
            }
            obj[j].GetComponent<SkinnedMeshRenderer>().materials = mats;
        }



    }


    // Update is called once per frame
    void Update()
    {
        if (Yarareta == true)
        {
            DissoNum += 0.005f;
            if (DissoNum >= 1.0f)
            {
                Destroy(gameObject);
            }
            // ディゾルブ ================================
            for (int j = 0; j < obj.Length; j++)
            {

                mats = obj[j].GetComponent<SkinnedMeshRenderer>().materials;
                for (int i = 0; i < obj[j].GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
                {
                    mats[0].SetFloat("_disso_strength", DissoNum);
                    //mats[i] = mat;
                }
                obj[j].GetComponent<SkinnedMeshRenderer>().materials = mats;

            }

            //============================================
        }




        playerPos = player.transform.position;
        distance = Vector3.Distance(this.transform.position, playerPos);

        if (HP > 0) 
        {
            switch (pumState)
            {
                case PumpikinState.Stop:
                    animator.SetBool("aruki", false);
                    nav.isStopped = true;
                    if (distance < trackingRange)
                    {
                        pumState = PumpikinState.Attack;
                        nav.isStopped = false;
                    }

                    break;

                case PumpikinState.Attack:
                    animator.SetBool("aruki", true);
                    if (distance < StopRange)
                    {
                        nav.isStopped = true;
                        animator.SetBool("aruki", false);
                    }
                    nav.SetDestination(playerPos);
                    Quaternion.LookRotation(playerPos - this.transform.position);
                    if (distance > trackingRange)
                    {
                        nav.isStopped = false;
                        pumState = PumpikinState.Stop;
                    }
                    break;
            }
        }

       
    }

    public void BulletCollision()
    {
        HP--;
        SetHp(HP);
        if (HP <= 0)
        {
            Destroy(gameObject);
            HideStatusUI();
            //Instantiate(Expo, transform.position, transform.rotation);


        }
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {
            HP--;
            SetHp(HP);
        }
        if (HP <= 0)
        {
            //Destroy(gameObject);
            
            Debug.Log("死ぬ～");
            HideStatusUI();
            //Instantiate(Expo, transform.position, transform.rotation);

            if(Yarareta == false)
            {
                _countNum.KabotyaDesNum++;
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
        }
        if (HP <= 0)
        {
            //Destroy(gameObject);
            
            Debug.Log("死ぬ～");

            HideStatusUI();
            if(Yarareta == false)
            {
                _countNum.KabotyaDesNum++;
                Yarareta = true;
            }
           
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, StopRange);
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
