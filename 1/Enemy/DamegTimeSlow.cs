using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻撃時にヒットストップをかけるプログラム

public class DamegTimeSlow : MonoBehaviour
{

    [Header("遅くなる時の時間")]
    [SerializeField] private float timeScale = 0.1f;
    [Header("遅くしている時間")]
    [SerializeField] private float slowTime = 1f;
    //経過時間
    private float elapsedTime = 0f;
    //時間を遅くしているか判定
    public bool isSlowDown = false;

 
   // Update is called once per frame
    void Update()
    {
        if (!isSlowDown)
        {
            return;
        }
        elapsedTime += Time.unscaledDeltaTime;
        if (elapsedTime >= slowTime)
        {
            SetNormalTime();
        }
    }

    //　時間を遅らせる処理
    public void SlowDown()
    {
        elapsedTime = 0f;
        Time.timeScale = timeScale;
        isSlowDown = true;
    }
    //　時間を元に戻す処理
    public void SetNormalTime()
    {
        Time.timeScale = 1f;
        isSlowDown = false;
    }

}
