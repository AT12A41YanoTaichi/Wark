using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----
//制作：矢野
//追記：髙橋
//追記：ジョンソン
//概要：敵が撃つ弾の処理
///----

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("通常弾の攻撃力")]
    private int BulletAttack;

    [SerializeField, Header("チャージ弾の攻撃力")]
    private int ChageBulletAttack;

    [SerializeField, Header("Aim弾の攻撃力")]
    private int AimBulletAttack;

    [SerializeField,Header("弾のスピード")]
    private float BulletSpeed;

    [SerializeField, Header("プレイヤーのスクリプトを取得")]
    private Player player;

    private float ChargeTime;

    private float AimTime;

    private float time;

    private bool IsCharge;

    private bool IsAim;

    private bool isfall;
    //[SerializeField, Header("敵のスクリプト取得")]
    //private Enemy enemy;

    private Vector3 size;
    [SerializeField, Header("effect")]
    private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        isfall = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(isfall)
       {
            Destroy(gameObject);
       }

        time += Time.deltaTime;
        if(IsCharge)
        {
            if (ChargeTime <= time)
            {
                //enemy.MinusMP(10);
                Vector3 velocity = gameObject.transform.rotation * new Vector3(BulletSpeed, 0, 0);

                gameObject.transform.position += velocity * Time.deltaTime;
                //生成されて4秒たったら消す
                Destroy(gameObject, 2.5f);
            }
            else
            {
               
                Vector3 Size = new Vector3(size.x, size.y, size.z);
                size.x += 0.015f;
                size.y += 0.015f;
                size.z += 0.015f;
                gameObject.transform.localScale = Size;
                effect.transform.localScale = gameObject.transform.localScale;
            }
        }
        else if(IsAim)
        {
            if (AimTime <= time)
            {
                //enemy.MinusMP(10);
                Vector3 velocity = transform.rotation * new Vector3(BulletSpeed, 0, 0);

                gameObject.transform.position += velocity * Time.deltaTime;
                //生成されて4秒たったら消す
                Destroy(gameObject, 2.5f);
            }
           
        }
        else
        {
            Vector3 velocity = transform.rotation * new Vector3(BulletSpeed, 0, 0);
           
            gameObject.transform.position += velocity * Time.deltaTime;
            //生成されて4秒たったら消す
            Destroy(gameObject, 2.5f);
           
        }
       
    }
    public void IsFall(bool fall)
    {
        isfall = fall;
    }

    public void Creeate(float ShotSpeed , bool isCharge,float Charge , bool isAim,float Aim)
    {
        BulletSpeed = ShotSpeed;
        ChargeTime = Charge;
        AimTime = Aim;
        IsCharge = isCharge;
        IsAim = isAim;
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag);
        //ぶつかったのがプレイヤーか
        if (col.gameObject.tag == "Player")
        {
            //プレイヤーのHPを攻撃力分減らす
            if(IsCharge)
            {
                col.gameObject.GetComponent<Player>().MinusHp(ChageBulletAttack);
            }
            else if(IsAim)
            {
                col.gameObject.GetComponent<Player>().MinusHp(AimBulletAttack);
            }
            else
            {
                col.gameObject.GetComponent<Player>().MinusHp(BulletAttack);
            }
            //ぶつかった弾は消す
            Destroy(gameObject);
        }
    }
}