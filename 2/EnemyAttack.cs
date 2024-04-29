using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----
//����F���
//�ǋL�F
//�T�v�F�G�̋ߐڍU���̓����蔻��̏���
///----
public class EnemyAttack : MonoBehaviour
{
    //�U���̍d��
    [Header("�Z�̑S�̍d��")]
    [SerializeField] private int StunTime = 30;

    //�G��HP
    [SerializeField, Header("Player�̎擾")]
    private Player player;
    [Header("�q�b�g�G�t�F�N�g")]
    [SerializeField] GameObject prefab;
    private int AttackDameje;


    private int stuntimer;
    private bool Nowstuntime;
    [SerializeField, Header("�G�̍U���ŏ��̈ʒu")]
    private Vector3 Fasttransform;
    private bool Hit;
    private bool EndAttack;
    private bool Effect = false;
    [SerializeField, Header("�U��Effect")]
    private GameObject AttackEffect;
    [SerializeField, Header("EnemyScript�擾")]
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
                            Debug.Log("�G�t�F�N�g����");
                            Instantiate(AttackEffect, new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, 90, 0));
                            Effect = true;
                        }
                    }
                    else
                    {
                        if (!Effect)
                        {
                            Debug.Log("�G�t�F�N�g����");
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
                    Debug.Log("�G�t�F�N�g����");
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
        //���������Q�[���I�u�W�F�N�g��Ground�^�O�������Ƃ�
        if (other.gameObject.CompareTag("Player") && !Hit)
        {
            Debug.Log("�v���C���[�ɓ��������I");
            other.gameObject.GetComponent<Player>().MinusHp(AttackDameje);
            Hit = true;
        }
    }
}
