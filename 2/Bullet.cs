using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----
//����F���
//�ǋL�F����
//�ǋL�F�W�����\��
//�T�v�F�G�����e�̏���
///----

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("�ʏ�e�̍U����")]
    private int BulletAttack;

    [SerializeField, Header("�`���[�W�e�̍U����")]
    private int ChageBulletAttack;

    [SerializeField, Header("Aim�e�̍U����")]
    private int AimBulletAttack;

    [SerializeField,Header("�e�̃X�s�[�h")]
    private float BulletSpeed;

    [SerializeField, Header("�v���C���[�̃X�N���v�g���擾")]
    private Player player;

    private float ChargeTime;

    private float AimTime;

    private float time;

    private bool IsCharge;

    private bool IsAim;

    private bool isfall;
    //[SerializeField, Header("�G�̃X�N���v�g�擾")]
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
                //���������4�b�����������
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
                //���������4�b�����������
                Destroy(gameObject, 2.5f);
            }
           
        }
        else
        {
            Vector3 velocity = transform.rotation * new Vector3(BulletSpeed, 0, 0);
           
            gameObject.transform.position += velocity * Time.deltaTime;
            //���������4�b�����������
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
        //�Ԃ������̂��v���C���[��
        if (col.gameObject.tag == "Player")
        {
            //�v���C���[��HP���U���͕����炷
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
            //�Ԃ������e�͏���
            Destroy(gameObject);
        }
    }
}