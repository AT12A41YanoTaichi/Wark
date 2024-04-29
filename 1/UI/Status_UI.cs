using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Status_UI : MonoBehaviour
{
    [Header("PlayerBase���擾����")]
    [SerializeField] private PlayerBase Pbase;

    [Header("ExpSlider�̎擾")]
    [SerializeField] private Slider expSlider;

    [Header("���݂�EXP��")]
    [SerializeField]private int NowEXP;

    [Header("���̃��x���ɕK�v��EXP��")]
    [SerializeField] private int NextEXP;

    [Header("�擾����EXP��")]
    [SerializeField] private int GetEXP;

    [Header("��x���炵�Ă���̌o�ߎ���")]
    private float countTime = 0f;

    [Header("����EXP�����炷�܂ł̎���")]
    [SerializeField]private float nextCountTime = 0f;

    [Header("�X���C�_�[��ω�������܂ł̑ҋ@����")]
    [SerializeField]private float delayTime = 1f;

    public bool IsGetEXP = false;

    private int XP;

    [Header("�v���C���[�̃��x���\��")]
    [SerializeField] private Text LV;

    [Header("SaveManager�̎擾")]
    [SerializeField] private GameObject savemanager;

    private bool hasProcessed = false;

    // Start is called before the first frame update
    void Start()
    {
        expSlider.value = 0;
        NowEXP = Pbase.EXP;
        XP = 0;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasProcessed)
        {
            NowEXP = Pbase.GetEXP();
            LV.text = "" + Pbase.LV;
            NextEXP = Pbase.GetNeedForLvupExp(Pbase.LV + 1);
            expSlider.value = (float)NowEXP / NextEXP;
            hasProcessed = true;
        }

        if (IsGetEXP)
        {
            if (countTime >= nextCountTime)
            {
                if (XP < GetEXP)
                {
                    XP += 1;
                    NowEXP += 1;
                }
                else
                {
                    IsGetEXP = false;
                    XP = 0;
                }
                expSlider.value = (float)NowEXP / NextEXP;

                if (NowEXP >= NextEXP)
                {
                    GetEXP -= XP;
                    NowEXP = 0;
                    expSlider.value = 0;
                    LV.text = "" + Pbase.LV;
                }
                countTime = 0f;
            }
              countTime += Time.deltaTime;
        }
    }




    public void AddExp(int GetXP, int NEXTXP, int XP)
    {
        GetEXP = GetXP;
        NextEXP = NEXTXP;
        NowEXP = XP;
        countTime = 0f;

        IsGetEXP = true;
    }

    public void StartGetEXP()
    {
        IsGetEXP = true;
    }
}
