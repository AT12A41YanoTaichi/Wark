using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//�v���C���[��HP�o�[UI

public class PlayerHPBar : MonoBehaviour
{
    [Header("PlayerBase���擾����")]
    [SerializeField] private PlayerBase Pbase;
    [Header("EnemyBase���擾����")]
    [SerializeField] private EnemyBase Ebase;
    private int FastHP = 150;
    private int maxHP;
    private int hp;
    //�@�ŏI�I��hp�v���Ɏg��
    public int finalHP;
    //�@HP����x���炵�Ă���̌o�ߎ���
    private float countTime = 0f;
    //�@����HP�����炷�܂ł̎���
    [SerializeField]
    private float nextCountTime = 0f;
    //�@HP�\���p�X���C�_�[
    [Header("hpSlider������")]
    [SerializeField]private Slider hpSlider;
    //�@�ꊇHP�\���p�X���C�_�[
    [Header("bulkHPSlider������")]
    [SerializeField]
    private Slider bulkHPSlider;
    //�@���݂̃_���[�W��
    private int damage = 0;
    //�@���݂̉񕜗� ��
    private int Add = 0;
    //�@���Ɍ��炷�_���[�W��
    [SerializeField]
    private int amountOfDamageAtOneTime;
    //�@���ɑ��₷�񕜗�
    [SerializeField]
    private int amountOfHealAtOneTime;
    //�@HP�����炵�Ă��邩�ǂ���
    private bool isReducing;
    //�@HP���񕜂ǂ���
    private bool isHealing;
    //�@HP�p�\���X���C�_�[�����炷�܂ł̑ҋ@����
    [SerializeField]
    private float delayTime = 1f;
    [Header("true�Ȃ�v���C���[��false�Ȃ�G��")]
    [SerializeField] private bool IsPlayer;

    [Header("Fall�̎擾")]
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
        //���݂�HP���ő�HP�Ɠ����ɂ���
        hp = maxHP;
        finalHP = maxHP;
        //Slider���ő�l�ɂ���
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
            //�@�_���[�W�Ȃ���Ή������Ȃ�
        if (isReducing)
        {
            if (countTime >= nextCountTime)
            {
                int tempDamage;
                //�@���߂�ꂽ�ʂ����c��_���[�W�ʂ���������Ώ���������1��̃_���[�W�ɐݒ�
                tempDamage = Mathf.Min(amountOfDamageAtOneTime, damage);
                hp -= tempDamage;
                //�@�S�̂̔䗦�����߂�
                hpSlider.value = (float)hp / FastHP;
                //�@�S�_���[�W�ʂ���1��Ō��炵���_���[�W�ʂ����炷
                damage -= tempDamage;
                //�@�S�_���[�W�ʂ�0��艺�ɂȂ�����0��ݒ�
                damage = Mathf.Max(damage, 0);

                countTime = 0f;
                //�@�_���[�W���Ȃ��Ȃ�����HP�o�[�̕ύX���������Ȃ��悤�ɂ���
                if (damage <= 0)
                {
                    isReducing = false;
                }
            }
        }
        //�@���Ɍ��炷���Ԃ�������

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
        //�@�_���[�W���󂯂����ɈꊇHP�p�̃o�[�̒l��ύX����
        var tempHP = Mathf.Max(finalHP -= damage, 0);
        bulkHPSlider.value = (float)tempHP / FastHP;
        this.damage += damage;
        countTime = 0f;
        //�@��莞�Ԍ��HP�o�[�����炷�t���O��ݒ�
        Invoke("StartReduceHP", delayTime);
    }

    public void AddHPUI(int Add)
    {
        var tempHP = Mathf.Max(finalHP += Add, 0);
        hpSlider.value = (float)tempHP / FastHP;
        bulkHPSlider.value = (float)tempHP / FastHP;
        hp += Add;
    }


    //�@���X��HP�o�[�����炷�̂��X�^�[�g
    public void StartReduceHP()
    {
        isReducing = true;
    }
}
