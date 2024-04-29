using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Collider�擾")]
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
       //�G�ɓ���������
        if (other.gameObject.CompareTag("Enemy")) 
        {
            //���������I�u�W�F�N�g��EnemyBase�擾
            EnemyBase Ebase = other.GetComponent<EnemyBase>();
            if (Ebase != null)
            {
                //�G��HP�����炷
                Ebase.MinusHP(20);

                //�G��Rigidbody�擾
                Rigidbody rb = other.GetComponent<Rigidbody>();

                //�v���C���[����G�ւ̃x�N�g�����擾
                Vector3 distination = (other.transform.position- transform.position).normalized;

                //�v���C���[�̔��Ε����փm�b�N�o�b�N������
                rb.AddForce(distination * 20.0f, ForceMode.VelocityChange);

                //�q�b�g�G�t�F�N�g�\��
                GameObject effect = Instantiate(HitEffect);
                Vector3 HitPos = new Vector3(Ebase.transform.position.x, Ebase.transform.position.y+1.0f, Ebase.transform.position.z);
                effect.transform.position = HitPos;

                AttackCollider.enabled = false;
                Hit = true;
            }
        }
        //�ؔ��ɓ���������
        if(other.gameObject.CompareTag("WoodBox"))
        {
            WoodBox Box = other.GetComponent<WoodBox>();
            Box.DestroyObject();
        }

    }
}
