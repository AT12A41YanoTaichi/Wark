using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : MonoBehaviour
{

    //敵の最大値MP(タオ)
    [SerializeField, Header("EnemyBaseの取得")]
    private EnemyBase Ebase;

    private int EnemyHP = 50;

    //仮プレイヤーの取得
    [Header("プレイヤーのスクリプト取得")]
    [SerializeField] private Player player;

    //仮プレイヤーの取得
    [Header("プレイヤーの取得")]
    [SerializeField] private GameObject playerObject;

    private Vector3 playerPos;

    [SerializeField, Header("敵のAnimatr")]
    private Animator animator;
    private AnimatorStateInfo animeInfo;
    [SerializeField, Header("移動速度")]
    private float Speed;

    // 自身のTransform
    [SerializeField] private Transform _self;

    // ターゲットのTransform
    [SerializeField] private Transform _target;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 _forward = Vector3.forward;
    private Vector3 eulerAngles;

    [Header("攻撃オブジェクト")]
    [SerializeField] private GameObject AttackObject;
    private Collider AttackCollider;
    private EnemyAttack Attack;

    private bool isLeft;
    private bool isRotation;

    public enum WeakEnemyState
    {
        //走る
        Run,
        //近接攻撃
        CloseAttack,
        //倒れる
        fall,
    };
    [Header("敵の現在のステート")]
    public WeakEnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        EnemyHP = Ebase.GetEnemyHp();
        enemyState = WeakEnemyState.Run;
        Attack = AttackObject.GetComponent<EnemyAttack>();
        AttackCollider = AttackObject.GetComponent<Collider>();
        OffColliderAttack();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        animeInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (playerObject)
        {
            playerPos.x = playerObject.transform.position.x;
        }

        float Pos = Vector3.Distance(transform.position, playerPos);
      
        

        switch (enemyState)
        {
            case WeakEnemyState.Run:

                if (Pos <= 4.0f)
                {
                    SetWeakEnemyState(WeakEnemyState.CloseAttack);
                }

                if (playerPos.x < transform.position.x)
                {
                    transform.position -= Vector3.right * Speed * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.right * Speed * Time.deltaTime;
                }


                break;

            case WeakEnemyState.CloseAttack:
                animator.speed = 1.5f;
                animator.SetBool("Attack", true);
                if (animeInfo.nameHash == Animator.StringToHash("Base Layer.アーマチュア|アクション"))
                {

                    if (playerPos.x < transform.position.x)
                    {
                        if (!isRotation)
                        {
                            isLeft = true;
                            isRotation = true;
                        }
                    }
                    else
                    {
                        if (!isRotation)
                        {
                            isLeft = false;
                            isRotation = true;
                        }
                    }

                    if (animeInfo.normalizedTime > 0.6f&& animeInfo.normalizedTime < 0.8f)
                    {
                        if (isLeft)
                        {
                            transform.position -= Vector3.right * Speed * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += Vector3.right * Speed * Time.deltaTime;
                          
                        }
                        OnColliderAttack();
                        Attack.StartAttack(51, 30, false, false);
                    }


                    if (animeInfo.normalizedTime > 1.0f)
                    {
                        animator.speed = 1.0f;
                        animator.SetBool("Attack", false);
                        isRotation = false;
                        SetWeakEnemyState(WeakEnemyState.Run);
                        OffColliderAttack();
                    }
                }
                  

                    break;

            case WeakEnemyState.fall:

                animator.SetBool("Fall", true);
                if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Fall"))
                {
                    if (animeInfo.normalizedTime > 1.0f)
                    {
                        Destroy(gameObject);
                    }
                }
                break;
        }

        if (Ebase.GetEnemyHp() <= 0)
        {
            enemyState = WeakEnemyState.fall;
        }

        if (playerObject)
        {
            var dir = _target.position - _self.position;
            // ターゲットの方向への回転
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);

            // 回転補正
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            _self.rotation = lookAtRotation * offsetRotation;
            eulerAngles = transform.eulerAngles; // ローカル変数に格納

            eulerAngles.z = 0;

            if (enemyState == WeakEnemyState.CloseAttack|| enemyState == WeakEnemyState.fall)
            {
                if(isLeft)
                {
                    eulerAngles.y = 0.0f;
                }
                else
                {
                    eulerAngles.y = 180.0f;
                }
            }
            transform.eulerAngles = eulerAngles; // ローカル変数を代入
        }



    }

    public void SetWeakEnemyState(WeakEnemyState state)
    {
        enemyState = state;
    }

    private void OffColliderAttack()
    {
        AttackCollider.enabled = false;
        //Attack.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnColliderAttack()
    {
        AttackCollider.enabled = true;
        //Attack.GetComponent<MeshRenderer>().enabled = true;
    }

    

}

