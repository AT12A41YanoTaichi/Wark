using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


//�v���C���[��HP�AEXP�A�A�C�e���g�p�̏���

public class PlayerBase : MonoBehaviour
{
    [Header("�v���C���[��HP")]
    [SerializeField] public int PlayerHP;

    public int PlayerMaxHP;

    [Header("�v���C���[��HPBar�擾")]
    [SerializeField] private PlayerHPBar pHPBar;

    public bool isHeal = false;
    private bool isInput = false;
    private float DPad;

    public bool StopMove = false;

    [Header("Animator�擾")]
    [SerializeField] Animator animator;
    AnimatorStateInfo animeInfo;

    [Header("WordlPointAndScreenPoint�̎擾")]
    [SerializeField] private WorldPointAndScreenPoint WPointSPoint;

    [Header("AimImage�̎擾")]
    [SerializeField]  private Image Aim;

    [Header("�v���C���[�̍��̈ʒu���擾")]
    [SerializeField] public GameObject waist;

    [Header("ItemMenu�X�N���v�g�擾")]
    [SerializeField] public ItemMenu itemmenu;

    private GameObject menu;

    public GameObject HealEffect;
    [Header("SaveManager�̎擾")]
    [SerializeField] private GameObject savemanager;

    [Header("HealPotion�̎擾")]
    [SerializeField] private GameObject HealPotion;

    [Header("�|�[�Y�p�l���̎擾")]
    [SerializeField] private GameObject pausePanel;

    private bool hasProcessed = false;

    [Header("�v���C���[�̌��݂̃��x��")]
    [SerializeField] public int LV;
    [Header("�v���C���[�̌��݂�Exp")]
    [SerializeField] public int EXP;

    [Header("Level UP�p�l���擾")]
    [SerializeField] GameObject LevelUP;




    [Header("Gaerd�G�t�F�N�g�擾")]
    [SerializeField] GameObject Gaerdeffect;
    [Header("GuardEffectPos�擾")]
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

        //�|�[�Y������Ȃ���Ώ���
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

        //���݂�HP���ő�l��菭�Ȃ�������
        if (PlayerHP < PlayerMaxHP)
        {
            if (isHeal && isInput)
            {
                //���ރA�j���[�V�����̊J�n
                animator.SetBool("Drink", true);
                HealPotion.SetActive(true);
                //�񕜒��̈ړ����~����
                StopMove = true;

                //�A�C�e���̎�ނɂ���ď�����ύX����
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
                //�A�j���[�V�������I�����Ă��邩
                if (animeInfo.normalizedTime > 1.0f)
                {
                    //�ړ��ł���悤�ɂ���
                    StopMove = false;
                    HealPotion.SetActive(false);
                    animator.SetBool("Drink", false);
                }
            }
        }
        
        Aim.transform.position = WPointSPoint.WorldPointToScreenPoint(waist.transform.position, Camera.main);

    }

    //�_���[�W���󂯂����̏���
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

    //�񕜎��̏���
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
        
        Debug.Log("�v���C���[��HP" + PlayerHP);
    }

    //���݂�HP���擾
    public int GetHP()
    {
        return PlayerHP;
    }

    //�G����_��ꂽ����Aicon�\��
    public void OnAimAicon()
    {
        Aim.color = new Vector4(Aim.color.r, Aim.color.g, Aim.color.b, 1.0f);
    }

    //�G����_��ꂽ����Aicon��\��
    public void OffAimAicon()
    {
        Aim.color = new Vector4(Aim.color.r, Aim.color.g, Aim.color.b, 0.0f);
    }

    //�v���C���[�̃��x���A�b�v���ɕK�v��EXP�ʂ��w��
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

    //���݂̃��x�����擾
    public int GetNeedForLvupExp(int lv)
    {
        return needExpDictionary[lv];
    }

    //���x���A�b�vUI�̕\���A�X���C�hUI���X�^�[�g������
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
