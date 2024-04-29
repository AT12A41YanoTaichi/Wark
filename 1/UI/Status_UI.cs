using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Status_UI : MonoBehaviour
{
    [Header("PlayerBaseを取得する")]
    [SerializeField] private PlayerBase Pbase;

    [Header("ExpSliderの取得")]
    [SerializeField] private Slider expSlider;

    [Header("現在のEXP量")]
    [SerializeField]private int NowEXP;

    [Header("次のレベルに必要なEXP量")]
    [SerializeField] private int NextEXP;

    [Header("取得したEXP量")]
    [SerializeField] private int GetEXP;

    [Header("一度減らしてからの経過時間")]
    private float countTime = 0f;

    [Header("次にEXPを減らすまでの時間")]
    [SerializeField]private float nextCountTime = 0f;

    [Header("スライダーを変化させるまでの待機時間")]
    [SerializeField]private float delayTime = 1f;

    public bool IsGetEXP = false;

    private int XP;

    [Header("プレイヤーのレベル表示")]
    [SerializeField] private Text LV;

    [Header("SaveManagerの取得")]
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
