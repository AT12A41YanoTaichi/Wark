using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//プレイヤーのHPバーUI

public class PlayerHPBar : MonoBehaviour
{
    [Header("PlayerBaseを取得する")]
    [SerializeField] private PlayerBase Pbase;
    [Header("EnemyBaseを取得する")]
    [SerializeField] private EnemyBase Ebase;
    private int FastHP = 150;
    private int maxHP;
    private int hp;
    //　最終的なhp計測に使う
    public int finalHP;
    //　HPを一度減らしてからの経過時間
    private float countTime = 0f;
    //　次にHPを減らすまでの時間
    [SerializeField]
    private float nextCountTime = 0f;
    //　HP表示用スライダー
    [Header("hpSliderを入れる")]
    [SerializeField]private Slider hpSlider;
    //　一括HP表示用スライダー
    [Header("bulkHPSliderを入れる")]
    [SerializeField]
    private Slider bulkHPSlider;
    //　現在のダメージ量
    private int damage = 0;
    //　現在の回復力 量
    private int Add = 0;
    //　一回に減らすダメージ量
    [SerializeField]
    private int amountOfDamageAtOneTime;
    //　一回に増やす回復量
    [SerializeField]
    private int amountOfHealAtOneTime;
    //　HPを減らしているかどうか
    private bool isReducing;
    //　HPを回復どうか
    private bool isHealing;
    //　HP用表示スライダーを減らすまでの待機時間
    [SerializeField]
    private float delayTime = 1f;
    [Header("trueならプレイヤーのfalseなら敵の")]
    [SerializeField] private bool IsPlayer;

    [Header("Fallの取得")]
    [SerializeField] private GameObject Fall;

    private bool hasProcessed = false;
    // Start is called before the first frame update
    void Start()
    {
        if(IsPlayer)
        {
            maxHP = Pbase.PlayerHP;
        }
        else
        {
            maxHP = Ebase.EnemyHP;
        }
        //現在のHPを最大HPと同じにする
        hp = maxHP;
        finalHP = maxHP;
        //Sliderを最大値にする
        hpSlider.value = 1;
        bulkHPSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasProcessed)
        {
            if (IsPlayer)
            {
                maxHP = Pbase.PlayerHP;
            }
            hp = maxHP;
            finalHP = maxHP;
            hpSlider.value = (float)hp / FastHP;
            bulkHPSlider.value  = (float)hp / FastHP;
            hasProcessed = true;
        }
            //　ダメージなければ何もしない
        if (isReducing)
        {
            if (countTime >= nextCountTime)
            {
                int tempDamage;
                //　決められた量よりも残りダメージ量が小さければ小さい方を1回のダメージに設定
                tempDamage = Mathf.Min(amountOfDamageAtOneTime, damage);
                hp -= tempDamage;
                //　全体の比率を求める
                hpSlider.value = (float)hp / FastHP;
                //　全ダメージ量から1回で減らしたダメージ量を減らす
                damage -= tempDamage;
                //　全ダメージ量が0より下になったら0を設定
                damage = Mathf.Max(damage, 0);

                countTime = 0f;
                //　ダメージがなくなったらHPバーの変更処理をしないようにする
                if (damage <= 0)
                {
                    isReducing = false;
                }
            }
        }
        //　次に減らす時間がきたら

        countTime += Time.deltaTime;
        if(IsPlayer)
        {
            if (hp >= maxHP / 2)
            {
                Fall.GetComponent<Image>().color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            }
            else if (hp >= maxHP / 4)
            {
                Fall.GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                Fall.GetComponent<Image>().color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }
       

    }

    public void MinusHPUI(int damage)
    {
        //　ダメージを受けた時に一括HP用のバーの値を変更する
        var tempHP = Mathf.Max(finalHP -= damage, 0);
        bulkHPSlider.value = (float)tempHP / FastHP;
        this.damage += damage;
        countTime = 0f;
        //　一定時間後にHPバーを減らすフラグを設定
        Invoke("StartReduceHP", delayTime);
    }

    public void AddHPUI(int Add)
    {
        var tempHP = Mathf.Max(finalHP += Add, 0);
        hpSlider.value = (float)tempHP / FastHP;
        bulkHPSlider.value = (float)tempHP / FastHP;
        hp += Add;
    }


    //　徐々にHPバーを減らすのをスタート
    public void StartReduceHP()
    {
        isReducing = true;
    }
}
