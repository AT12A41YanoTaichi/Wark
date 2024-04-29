using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


//プレイヤーのHP、EXP、アイテム使用の処理

public class PlayerBase : MonoBehaviour
{
    [Header("プレイヤーのHP")]
    [SerializeField] public int PlayerHP;

    public int PlayerMaxHP;

    [Header("プレイヤーのHPBar取得")]
    [SerializeField] private PlayerHPBar pHPBar;

    public bool isHeal = false;
    private bool isInput = false;
    private float DPad;

    public bool StopMove = false;

    [Header("Animator取得")]
    [SerializeField] Animator animator;
    AnimatorStateInfo animeInfo;

    [Header("WordlPointAndScreenPointの取得")]
    [SerializeField] private WorldPointAndScreenPoint WPointSPoint;

    [Header("AimImageの取得")]
    [SerializeField]  private Image Aim;

    [Header("プレイヤーの腰の位置を取得")]
    [SerializeField] public GameObject waist;

    [Header("ItemMenuスクリプト取得")]
    [SerializeField] public ItemMenu itemmenu;

    private GameObject menu;

    public GameObject HealEffect;
    [Header("SaveManagerの取得")]
    [SerializeField] private GameObject savemanager;

    [Header("HealPotionの取得")]
    [SerializeField] private GameObject HealPotion;

    [Header("ポーズパネルの取得")]
    [SerializeField] private GameObject pausePanel;

    private bool hasProcessed = false;

    [Header("プレイヤーの現在のレベル")]
    [SerializeField] public int LV;
    [Header("プレイヤーの現在のExp")]
    [SerializeField] public int EXP;

    [Header("Level UPパネル取得")]
    [SerializeField] GameObject LevelUP;




    [Header("Gaerdエフェクト取得")]
    [SerializeField] GameObject Gaerdeffect;
    [Header("GuardEffectPos取得")]
    [SerializeField] GameObject GuardEffectPos;

    public GameObject camera;


    // Start is called before the first frame update
    void Start()
    {
        PlayerMaxHP = PlayerHP;
        OffAimAicon();
        menu = GameObject.Find("MenuCanvas");
        HealPotion.SetActive(false);
        LV = 1;
        EXP = 0;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        if (!hasProcessed)
        {
            PlayerHP = savemanager.GetComponent<SaveManager>().GetPlayerHP();
            PlayerMaxHP =  150;
            LV = savemanager.GetComponent<SaveManager>().GetPlayerLV();
            EXP = savemanager.GetComponent<SaveManager>().GetPlayerEXP();

            hasProcessed = true;
        }
        animeInfo = animator.GetCurrentAnimatorStateInfo(0);

        DPad = Input.GetAxis("D_Pad");

        //ポーズ中じゃなければ処理
        if(!pausePanel.active)
        {
            if (!isInput)
            {
                if (DPad < 0.0f)
                {
                    isHeal = true;
                    isInput = true;
                }
            }
        }
       
        if (DPad == 0.0f)
        {
            isInput = false;
        }

        //現在のHPが最大値より少ないか判定
        if (PlayerHP < PlayerMaxHP)
        {
            if (isHeal && isInput)
            {
                //飲むアニメーションの開始
                animator.SetBool("Drink", true);
                HealPotion.SetActive(true);
                //回復中の移動を停止する
                StopMove = true;

                //アイテムの種類によって処理を変更する
                switch (itemmenu.SlectIndex)
                {
                    case 0:
                        if (OwnedltemsData.Instance.CountNumber(Item.ItemType.Herb) > 0)
                        {
                            HealHP(50);
                            isHeal = false;
                            GameObject effect = Instantiate(HealEffect);
                            effect.transform.position = transform.position;
                        }

                        break;

                    case 1:
                        if (OwnedltemsData.Instance.CountNumber(Item.ItemType.HPPotion) > 0)
                        {
                            HealHP(100);
                            isHeal = false;
                            GameObject effect = Instantiate(HealEffect);
                            effect.transform.position = transform.position;
                        }
                        break;

                    case 2:

                        break;

                    case 3:

                        break;
                }
                
               
            }
        }
        if (animator.GetBool("Drink"))
        {
            if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Drink"))
            {
                //アニメーションが終了しているか
                if (animeInfo.normalizedTime > 1.0f)
                {
                    //移動できるようにする
                    StopMove = false;
                    HealPotion.SetActive(false);
                    animator.SetBool("Drink", false);
                }
            }
        }
        
        Aim.transform.position = WPointSPoint.WorldPointToScreenPoint(waist.transform.position, Camera.main);

    }

    //ダメージを受けた時の処理
    public void MinusHP(int damage)
    {
        camera.GetComponent<CameraShake>().CameraShaker();
       
        MovementPlayer Player = GetComponent<MovementPlayer>();
        if (Player.GetisGuard())
        {
            GameObject effect = Instantiate(Gaerdeffect);
            Vector3 effectPos = new Vector3(GuardEffectPos.transform.position.x, GuardEffectPos.transform.position.y, GuardEffectPos.transform.position.z);
            effect.transform.position = effectPos;
            damage /= 2;
            PlayerHP -= damage;
            Debug.Log(damage);
            pHPBar.MinusHPUI(damage);
        }
        else
        {
            PlayerHP -= damage;
            Debug.Log(damage);
            pHPBar.MinusHPUI(damage);
        }
    }

    //回復時の処理
    public void HealHP(int heal)
    {
        PlayerHP += heal;
        if (itemmenu.SlectIndex == 0)
        {
            OwnedltemsData.Instance.Use(Item.ItemType.Herb);
             menu.GetComponent<Menu>().MinusHeabItem();
           
        }
        else if (itemmenu.SlectIndex == 1)
        {
            OwnedltemsData.Instance.Use(Item.ItemType.HPPotion);
            menu.GetComponent<Menu>().MinusPotionItem();
        }
        
        if (PlayerHP > PlayerMaxHP)
        {
            int  excess;
            int Heal;
            excess = PlayerHP - PlayerMaxHP;
            Heal = heal - excess;
            PlayerHP = PlayerMaxHP;
            pHPBar.AddHPUI(Heal);
        }
        else
        {
            pHPBar.AddHPUI(heal);
        }
        
        Debug.Log("プレイヤーのHP" + PlayerHP);
    }

    //現在のHPを取得
    public int GetHP()
    {
        return PlayerHP;
    }

    //敵から狙われた時のAicon表示
    public void OnAimAicon()
    {
        Aim.color = new Vector4(Aim.color.r, Aim.color.g, Aim.color.b, 1.0f);
    }

    //敵から狙われた時のAicon非表示
    public void OffAimAicon()
    {
        Aim.color = new Vector4(Aim.color.r, Aim.color.g, Aim.color.b, 0.0f);
    }

    //プレイヤーのレベルアップ時に必要なEXP量を指定
    private Dictionary<int, int> needExpDictionary = new Dictionary<int, int>
    {
        {2,10},
         {3,11},
          {4,12},
           {5,13},
            {6,14},
             {7,16},
             {8,19},
         {9,21},
          {10,23},
           {11,25},
            {12,28},
             {13,31},
             {14,34},
         {15,37},
          {16,41},
           {17,45},
            {18, 50},
             {19,55},
              {20,61},
         {21,67},
          {22,74},
           {23,81},
            {24,89},
             {25,98},
             {26,108},
         {27,119},
          {28,131},
           {29,144},
            {30,158},
             {31,174},
             {32,191},
         {33,210},
          {34,231},
           {35,254},
            {36, 279},
             {37,307},
              {38,338},
               {39,372},
                {40,409},
    };

    //現在のレベルを取得
    public int GetNeedForLvupExp(int lv)
    {
        return needExpDictionary[lv];
    }

    //レベルアップUIの表示、スライドUIをスタートさせる
    public void StartLevelUpUI()
    {
        LevelUP.GetComponent<SlideUIControl>().state = 0;
        LevelUP.SetActive(true);
    }
    public int GetEXP()
    {
        return EXP;
    }

    public int GetLV()
    {
        return LV;
    }
}
