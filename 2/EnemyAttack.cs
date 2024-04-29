using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----
//制作：矢野
//追記：
//概要：敵の近接攻撃の当たり判定の処理
///----
public class EnemyAttack : MonoBehaviour
{
    //攻撃の硬直
    [Header("技の全体硬直")]
    [SerializeField] private int StunTime = 30;

    //敵のHP
    [SerializeField, Header("Playerの取得")]
    private Player player;
    [Header("ヒットエフェクト")]
    [SerializeField] GameObject prefab;
    private int AttackDameje;


    private int stuntimer;
    private bool Nowstuntime;
    [SerializeField, Header("敵の攻撃最初の位置")]
    private Vector3 Fasttransform;
    private bool Hit;
    private bool EndAttack;
    private bool Effect = false;
    [SerializeField, Header("攻撃Effect")]
    private GameObject AttackEffect;
    [SerializeField, Header("EnemyScript取得")]
    private Enemy enemy;
    private bool isJampAttack;
    private bool Boss;

    ShowDamageNumber showDamageNumber;
    // Start is called before the first frame update
    void Start()
    {
        stuntimer = 0;
        Nowstuntime = false;
        EndAttack = false;
        Hit = false;
        showDamageNumber = GetComponent<ShowDamageNumber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stuntimer >= 0 && Nowstuntime == true)
        {
            stuntimer -= 1;
            if (!isJampAttack)
            {
                if (Boss)
                {
                    if (enemy.GetisLeft())
                    {
                        if (!Effect)
                        {
                            Debug.Log("エフェクト生成");
                            Instantiate(AttackEffect, new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, 90, 0));
                            Effect = true;
                        }
                    }
                    else
                    {
                        if (!Effect)
                        {
                            Debug.Log("エフェクト生成");
                            Instantiate(AttackEffect, new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, -90, 0));
                            Effect = true;
                        }
                    }
                }
               
            }
            else
            {

                if (!Effect)
                {
                    Debug.Log("エフェクト生成");
                    Instantiate(AttackEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z), Quaternion.Euler(-90, 0, 90));
                    Effect = true;
                }
            }
            
           
        }
        if (stuntimer < 0)
        {
            Nowstuntime = false;
            Effect = false;
            Hit = false;
            EndAttack = true;
        }
    }

    public bool NowStunTime()
    {
        return Nowstuntime;
    }

    public bool GetEndAttack()
    {
        return EndAttack;
    }

    public void SetEndAttack()
    {
        EndAttack = false;
    }

    public void StartAttack(int StunTime,int Dameje,bool Attack,bool boss)
    {
        Nowstuntime = true;
        stuntimer = StunTime;
        EndAttack = false;
        AttackDameje = Dameje;
        isJampAttack = Attack;
        Boss = boss;
    }


    private void OnTriggerEnter(Collider other)
    {
        //当たったゲームオブジェクトがGroundタグだったとき
        if (other.gameObject.CompareTag("Player") && !Hit)
        {
            Debug.Log("プレイヤーに当たった！");
            other.gameObject.GetComponent<Player>().MinusHp(AttackDameje);
            Hit = true;
        }
    }
}
