using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class SphereSensor : MonoBehaviour
{
    [Header("�R���C�_�[�擾")]
    [SerializeField] private SphereCollider searhArea = default;

    [Header("�G�̃X�N���v�g�擾")]
    [SerializeField] private Enemy enemy;

    [Header("EnamyBase�X�N���v�g�擾")]
    [SerializeField] private EnemyBase Ebase;

    [Header("RabbitoEnemy�X�N���v�g�擾")]
    [SerializeField] private RabbitEnemy rabbitenemy;

    [Header("SkeltonEnemy�X�N���v�g�擾")]
    [SerializeField] private SkeletonEnemy skeltonenemy;

    [Header("BatEnemy�X�N���v�g�擾")]
    [SerializeField] private BatEnemy batenemy;

    [Header("�G�̎���̍L��")]
    [SerializeField]  private float searchAngle;

    [Header("Enemy�Ȃ�true")]
    [SerializeField] private bool isEnemy;

    [Header("RabbitEnemy�Ȃ�true")]
    [SerializeField] private bool israbbitEnemy;

    [Header("SkeletonEnemy�Ȃ�true")]
    [SerializeField] private bool isSkeltonEnemy;

    [Header("BatEnemy�Ȃ�true")]
    [SerializeField] private bool isBatEnemy;

    //�s�����ɔ͈͊O�Ƀv���C���[���o��
    private bool Out = false;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

        if (!Out)
        {
            return;
        }

        if (isEnemy)
        {
            if (enemy.StopAttack())
            {
                enemy.SetState(Enemy.EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                Out = false;
            }

        }

        if (israbbitEnemy)
        {
            if (rabbitenemy.StopAttack())
            {
                rabbitenemy.SetState(RabbitEnemy.EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                Out = false;
            }
        }

        if (isSkeltonEnemy)
        {
            if (skeltonenemy.StopAttack())
            {
                skeltonenemy.SetState(SkeletonEnemy.EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                Out = false;
            }
        }

        if(isBatEnemy)
        {
            if(batenemy.StopAttack())
            {
                batenemy.SetState(BatEnemy.EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                Out = false;
            }
        }

    }

    private void OnTriggerStay(Collider target)
    {
        if(isEnemy)
        {
            if (enemy.StopAttack())
            {
                if (target.tag == "Player")
                {
                    //Enemy.EnemyState state = enemy.GetState();
                    //�v���C���[�̕���
                    var playerDirection = target.transform.position - transform.position;
                    //�v���C���[�̂���p�x
                    var angle = Vector3.Angle(transform.forward, playerDirection);
                    //�v���C���[�Ƃ̋��������߂�
                    var Dis = Vector3.Distance(target.gameObject.transform.position, transform.position);


                    if (angle <= searchAngle)
                    {
                        Debug.Log("�͈͓�");
                        if (Dis <= searhArea.radius * 0.12f)
                        {
                            Debug.Log("�ߐڍU��");
                            enemy.SetState(Enemy.EnemyState.Attack, target.transform.position);
                        }
                        else if (Dis <= searhArea.radius * 0.7f && Dis >= searhArea.radius * 0.12f)
                        {
                            Debug.Log("�ǂ�������");
                            enemy.SetState(Enemy.EnemyState.Nears, target.transform.position);
                        }
                        else if (Dis <= searhArea.radius * 1.0f && Dis >= searhArea.radius * 0.7f)
                        {
                            Debug.Log("��������");
                            enemy.SetState(Enemy.EnemyState.Shot, new Vector3(0.0f, 0.0f, 0.0f));
                        }
                    }
                    else
                    {
                        enemy.SetState(Enemy.EnemyState.Teisatu, new Vector3(0.0f, 0.0f, 0.0f));
                    }

                }
            }
        }

        if(israbbitEnemy)
        {
            if (target.tag == "Player")
            {
                //�v���C���[�̕���
                var playerDirection = target.transform.position - transform.position;
                //�v���C���[�̂���p�x
                var angle = Vector3.Angle(transform.forward, playerDirection);
                //�v���C���[�Ƃ̋��������߂�
                var Dis = Vector3.Distance(target.gameObject.transform.position, transform.position);

                if (angle <= searchAngle)
                {
                    if (rabbitenemy.StopAttack())
                    {
                        if (Dis <= searhArea.radius * 0.15f)
                        {
                            switch (Ebase.Condition)
                            {
                                case 1:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                                case 2:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Attack, target.transform.position);

                                    break;

                                case 3:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Attack, target.transform.position);

                                    break;
                            }
                        }
                        else if (Dis <= searhArea.radius * 0.8f && Dis >= searhArea.radius * 0.15f)
                        {
                            switch (Ebase.Condition)
                            {
                                case 1:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                                case 2:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Leave, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                                case 3:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Nears, target.transform.position);

                                    break;
                            }
                        }
                        else if (Dis <= searhArea.radius * 1.0f && Dis >= searhArea.radius * 0.8f)
                        {
                            switch (Ebase.Condition)
                            {
                                case 1:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                                case 2:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;
                                case 3:

                                    rabbitenemy.SetState(RabbitEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                            }


                        }
                    }
                    else
                    {
                        Out = true;
                    }

                }
            }

             
        }

        if(isSkeltonEnemy)
        {
            if (target.tag == "Player")
            { //�v���C���[�̕���
                var playerDirection = target.transform.position - transform.position;
                //�v���C���[�̂���p�x
                var angle = Vector3.Angle(transform.forward, playerDirection);
                //�v���C���[�Ƃ̋��������߂�
                var Dis = Vector3.Distance(target.gameObject.transform.position, transform.position);

                if (angle <= searchAngle)
                {
                    if (skeltonenemy.StopAttack())
                    {
                        if (Dis <= searhArea.radius * 0.15f)
                        {
                            skeltonenemy.SetState(SkeletonEnemy.EnemyState.Attack, target.transform.position);
                        }
                        else if (Dis <= searhArea.radius * 0.7f && Dis >= searhArea.radius * 0.15f)
                        {
                            skeltonenemy.SetState(SkeletonEnemy.EnemyState.Nears, target.transform.position);

                        }
                        else if (Dis <= searhArea.radius * 1.0f && Dis >= searhArea.radius * 0.7f)
                        {
                            switch (Ebase.Condition)
                            {
                                case 1:

                                    skeltonenemy.SetState(SkeletonEnemy.EnemyState.Alent, target.transform.position);

                                    break;

                                case 2:

                                    skeltonenemy.SetState(SkeletonEnemy.EnemyState.Nears, target.transform.position);

                                    break;

                                case 3:

                                    skeltonenemy.SetState(SkeletonEnemy.EnemyState.Nears, target.transform.position);

                                    break;
                            }


                        }
                    }
                    else
                    {
                        Out = true;
                    }

                }
            }
                
        }
       
        if(isBatEnemy)
        {
            if (target.tag == "Player")
            {
                //�v���C���[�̕���
                var playerDirection = target.transform.position - transform.position;
                //�v���C���[�̂���p�x
                var angle = Vector3.Angle(transform.forward, playerDirection);
                //�v���C���[�Ƃ̋��������߂�
                var Dis = Vector3.Distance(target.gameObject.transform.position, transform.position);

                if (angle <= searchAngle)
                {
                    if (batenemy.StopAttack())
                    {
                        if (Dis <= searhArea.radius * 0.15f)
                        {
                            batenemy.SetState(BatEnemy.EnemyState.Leave, target.transform.position);
                        }
                        else if (Dis <= searhArea.radius * 0.8f && Dis >= searhArea.radius * 0.15f)
                        {
                            batenemy.SetState(BatEnemy.EnemyState.Shot, target.transform.position);

                        }
                        else if (Dis <= searhArea.radius * 1.0f && Dis >= searhArea.radius * 0.8f)
                        {
                            switch (Ebase.Condition)
                            {
                                case 1:

                                    batenemy.SetState(BatEnemy.EnemyState.Alent, new Vector3(0.0f, 0.0f, 0.0f));

                                    break;

                                case 2:

                                    batenemy.SetState(BatEnemy.EnemyState.Shot, target.transform.position);

                                    break;
                                case 3:

                                    batenemy.SetState(BatEnemy.EnemyState.Shot, target.transform.position);

                                    break;

                            }

                        }
                    }
                    else
                    {
                        Out = true;
                    }

                }
            }

        }

    }


    private void OnTriggerExit(Collider target)
    {
        if (target.tag == "Player")
        {
            if (isEnemy)
            {
                if (!enemy.StopAttack())
                {
                    Out = true;
                }
            }
            
            if(israbbitEnemy)
            {
                if (!rabbitenemy.StopAttack() || rabbitenemy.State == RabbitEnemy.EnemyState.Alent) 
                {
                    Out = true;
                }
            }

            if(isSkeltonEnemy)
            {
                if (!skeltonenemy.StopAttack())
                {
                    Out = true;
                }
            }

            if (isBatEnemy)
            {
                if (!batenemy.StopAttack())
                {
                    Out = true;
                }
            }

        }
    }
           
    

//#if UNITY_EDITOR
//    //�@�T�[�`����p�x�\��
//    private void OnDrawGizmos()
//    {
//        Handles.color = Color.red;
//        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searhArea.radius);
//    }
//#endif

}
