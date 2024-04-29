using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Collider取得")]
    [SerializeField] private Collider AttackCollider;
    private bool Hit = false;
    public GameObject HitEffect;
    // Start is called before the first frame update
    void Start()
    {
        // AttackCollider.GetComponent<Collider>();
    }
   public bool GetHit()
   {
        return Hit;
   }

    public void SetHit(bool hit)
    {
        Hit = hit;
    }

    private void OnTriggerEnter(Collider other)
    {
       //敵に当たったら
        if (other.gameObject.CompareTag("Enemy")) 
        {
            //当たったオブジェクトのEnemyBase取得
            EnemyBase Ebase = other.GetComponent<EnemyBase>();
            if (Ebase != null)
            {
                //敵のHPを減らす
                Ebase.MinusHP(20);

                //敵のRigidbody取得
                Rigidbody rb = other.GetComponent<Rigidbody>();

                //プレイヤーから敵へのベクトルを取得
                Vector3 distination = (other.transform.position- transform.position).normalized;

                //プレイヤーの反対方向へノックバックさせる
                rb.AddForce(distination * 20.0f, ForceMode.VelocityChange);

                //ヒットエフェクト表示
                GameObject effect = Instantiate(HitEffect);
                Vector3 HitPos = new Vector3(Ebase.transform.position.x, Ebase.transform.position.y+1.0f, Ebase.transform.position.z);
                effect.transform.position = HitPos;

                AttackCollider.enabled = false;
                Hit = true;
            }
        }
        //木箱に当たったら
        if(other.gameObject.CompareTag("WoodBox"))
        {
            WoodBox Box = other.GetComponent<WoodBox>();
            Box.DestroyObject();
        }

    }
}
