using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : MonoBehaviour
{

    //�G�̍ő�lMP(�^�I)
    [SerializeField, Header("EnemyBase�̎擾")]
    private EnemyBase Ebase;

    private int EnemyHP = 50;

    //���v���C���[�̎擾
    [Header("�v���C���[�̃X�N���v�g�擾")]
    [SerializeField] private Player player;

    //���v���C���[�̎擾
    [Header("�v���C���[�̎擾")]
    [SerializeField] private GameObject playerObject;

    private Vector3 playerPos;

    [SerializeField, Header("�G��Animatr")]
    private Animator animator;
    private AnimatorStateInfo animeInfo;
    [SerializeField, Header("�ړ����x")]
    private float Speed;

    // ���g��Transform
    [SerializeField] private Transform _self;

    // �^�[�Q�b�g��Transform
    [SerializeField] private Transform _target;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 _forward = Vector3.forward;
    private Vector3 eulerAngles;

    [Header("�U���I�u�W�F�N�g")]
    [SerializeField] private GameObject AttackObject;
    private Collider AttackCollider;
    private EnemyAttack Attack;

    private bool isLeft;
    private bool isRotation;

    public enum WeakEnemyState
    {
        //����
        Run,
        //�ߐڍU��
        CloseAttack,
        //�|���
        fall,
    };
    [Header("�G�̌��݂̃X�e�[�g")]
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
                if (animeInfo.nameHash == Animator.StringToHash("Base Layer.�A�[�}�`���A|�A�N�V����"))
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
            // �^�[�Q�b�g�̕����ւ̉�]
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);

            // ��]�␳
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            _self.rotation = lookAtRotation * offsetRotation;
            eulerAngles = transform.eulerAngles; // ���[�J���ϐ��Ɋi�[

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
            transform.eulerAngles = eulerAngles; // ���[�J���ϐ�����
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

