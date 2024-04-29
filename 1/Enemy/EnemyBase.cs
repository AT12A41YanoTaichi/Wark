using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//敵のHP、状態、ドロップするアイテム、EXP量制御プログラム


public class EnemyBase : MonoBehaviour
{
    [Header("敵のHP")]
    [SerializeField] public int EnemyHP;
    private int EnemyMaxHP;
    [Header("敵の状態")]
    [SerializeField] public int Condition;
    [Header("敵のHPBar取得")]
    [SerializeField] private PlayerHPBar pHPBar;

    [SerializeField] [Range(0, 1)] private float dropRate = 0.1f;//アイテム出現確率
    [SerializeField] private Item itemPrefab;
    [SerializeField] private int number = 1;//アイテムの出現数

    private bool _isDropInvoked;

    //目的地
    Vector3 destination;
    [Header("ロックオンIcon")]
    [SerializeField] GameObject rookOn;
    private GameObject Player;

    [Header("ドロップするExpの量")]
    [SerializeField] public int Exp;

    private bool Die = false;

    public GameObject statusUi;
    [Header("DamegTimeSlowの取得")]
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

    //アップデート内でにその値を返す
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

    //確率判定
    public bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;
        Debug.Log(fProbabilityRate + "ランダム数値");
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
