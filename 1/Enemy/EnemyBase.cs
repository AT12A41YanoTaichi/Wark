using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�G��HP�A��ԁA�h���b�v����A�C�e���AEXP�ʐ���v���O����


public class EnemyBase : MonoBehaviour
{
    [Header("�G��HP")]
    [SerializeField] public int EnemyHP;
    private int EnemyMaxHP;
    [Header("�G�̏��")]
    [SerializeField] public int Condition;
    [Header("�G��HPBar�擾")]
    [SerializeField] private PlayerHPBar pHPBar;

    [SerializeField] [Range(0, 1)] private float dropRate = 0.1f;//�A�C�e���o���m��
    [SerializeField] private Item itemPrefab;
    [SerializeField] private int number = 1;//�A�C�e���̏o����

    private bool _isDropInvoked;

    //�ړI�n
    Vector3 destination;
    [Header("���b�N�I��Icon")]
    [SerializeField] GameObject rookOn;
    private GameObject Player;

    [Header("�h���b�v����Exp�̗�")]
    [SerializeField] public int Exp;

    private bool Die = false;

    public GameObject statusUi;
    [Header("DamegTimeSlow�̎擾")]
    [SerializeField] private GameObject SlowTime;

    // Start is called before the first frame update
    void Start()
    {
        SlowTime = GameObject.Find("CameraObject");
        EnemyMaxHP = EnemyHP;
        Condition = 1;
        rookOn.SetActive(false);
        Player = GameObject.Find("Paladin WProp J Nordstrom (1)");
        statusUi = GameObject.Find("StatusUi");
    }

    public void MinusHP(int damage)
    {
        SlowTime.GetComponent<DamegTimeSlow>().SlowDown();
        EnemyHP -= damage;
        if (EnemyHP <= EnemyMaxHP / 3)
        {
            Condition = 3;
        }
        else if(EnemyHP <= EnemyMaxHP / 2)
        {
            Condition = 2;
        }


        if(!Die)
        {
            if (EnemyHP <= 0)
            {
                Die = true;
                var moreExp = Player.GetComponent<PlayerBase>().GetNeedForLvupExp(Player.GetComponent<PlayerBase>().LV + 1);
                statusUi.GetComponent<Status_UI>().AddExp(Exp, moreExp, Player.GetComponent<PlayerBase>().EXP);
                if (moreExp <= Exp + Player.GetComponent<PlayerBase>().EXP)
                {
                    Player.GetComponent<PlayerBase>().LV++;
                    Player.GetComponent<PlayerBase>().StartLevelUpUI();
                    int GetExp = Exp + Player.GetComponent<PlayerBase>().EXP - moreExp;
                    if (GetExp <= 0)
                    {
                        Player.GetComponent<PlayerBase>().EXP = 0;
                    }
                    else
                    {
                        Player.GetComponent<PlayerBase>().EXP = GetExp;
                    }
                }
                else
                {
                    Player.GetComponent<PlayerBase>().EXP += moreExp;
                }

            }
        }
       
        
        pHPBar.MinusHPUI(damage);
    }

    public int condition()
    {
        return Condition;
    }

    public int GetHP()
    {
        return EnemyHP;
    }

    public void DropIfNeeded()
    {
        if (_isDropInvoked)
        {
            return;
        }

        _isDropInvoked = true;

        if (Random.Range(0, 1f) >= dropRate)
        {
            return;
        }

        for (var i = 0; i < number; i++)
        {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.Initialize();
        }

    }

    public void SetDestination(Vector3 positin)
    {
        destination = positin;
    }

    //�A�b�v�f�[�g���łɂ��̒l��Ԃ�
    public Vector3 GetDestination()
    {
        return destination;
    }

    public void LookOn()
    {
        rookOn.SetActive(true);
    }
    public void LookOff()
    {
        rookOn.SetActive(false);
    }

    //�m������
    public bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;
        Debug.Log(fProbabilityRate + "�����_�����l");
        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
