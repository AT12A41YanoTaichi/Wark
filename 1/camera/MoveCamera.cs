using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//カメラの移動、回転処理

public class MoveCamera : MonoBehaviour
{
    public GameObject TargetObject;
    [Header("カメラの高さのオフセット")]
    [SerializeField]public float Height = 1.5f;
    [Header("カメラとのオフセット")]
    [SerializeField] public float Distance = 15.0f;//
    [Header("水平方向のカメラ角度")]
    [SerializeField] public float RotAngle = 0.0f;
    [Header("垂直方向のカメラ角度")]
    [SerializeField] public float HeightAngle = 10.0f;
    [Header("見上げた時のカメラ距離")]
    [SerializeField] public float dis_min = 5.0f;
    [Header("通常のカメラ距離")]
    [SerializeField] public float dis_mdl = 10.0f;
    [Header("カメラの移動速度")]
    [SerializeField] public float CameraSpeed = 5;
    private Vector3 nowPos;
    //今の回転角度
    private float nowRotAngle;
    //今の高さ角度
    private float nowHeightAngle;
    //スティック入力の数値入れ
    private float rsv;
    private float rsh;

    private Vector3 Speed;
    public float RotSpped;

    //減衰挙動
    [Header("減衰挙動用フラグ")]
    [SerializeField] public bool EnavleAtten = true;
    public float AttenRate = 3.0f;
    public float ForwardDistance = 2.0f;
    private Vector3 addFoward;
    private Vector3 prevTargetPos;
    public float RotAngeleRate = 5.0f;
    public float AngleAttenRate = 1.0f;

    [Header("敵HPUIの取得")]
    [SerializeField] public Image HPUI;

    [Header("敵の取得")]
    [SerializeField] public GameObject Enemy;

    [Header("EnemyListManagerの取得")]
    public EnemyListManager enemyListManager;
    public Transform Target;
    private int TargetCount;
    private int TargetNo;

    public ItemMenu itemmenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rsh = Input.GetAxis("R_Stick_H");
        rsv = Input.GetAxis("R_Stick_V");

        //アイテムを選択中かどうか
        if (!itemmenu.KeyDonw)
        {
            if (rsh > 0.0f)
            {
                Speed.x = RotSpped;
            }
            if (rsh < 0.0f)
            {
                Speed.x = -RotSpped;
            }
            if (rsh == 0.0f)
            {

                Speed.x = 0.0f;
            }
            if (rsv < 0.0f)
            {
                Speed.z = RotSpped;
            }
            if (rsv > 0.0f)
            {
                Speed.z = -RotSpped;
            }
            if (rsv == 0.0f)
            {
                Speed.z = 0.0f;
            }
        }

        RotAngle -= Speed.x * Time.deltaTime*50.0f;
        HeightAngle += Speed.z * Time.deltaTime * 20.0f;
        HeightAngle = Mathf.Clamp(HeightAngle, -40.0f, 45.0f);
        Distance = Mathf.Clamp(Distance, 1.0f, 40.0f);

        if(EnavleAtten)
        {
            var target = TargetObject.transform.position;//ターゲットの位置をプレイヤーにする

            var halfPoint = (TargetObject.transform.position + target)/2;
            var deltaPos = halfPoint - prevTargetPos;//位置の微小増加量
            prevTargetPos = halfPoint;
            deltaPos *= ForwardDistance;

            addFoward += deltaPos * Time.deltaTime * 20.0f;
            addFoward = Vector3.Lerp(addFoward,Vector3.zero,Time.deltaTime*AttenRate);

            nowPos = Vector3.Lerp(nowPos, halfPoint + Vector3.up * Height + addFoward, Mathf.Clamp01(Time.deltaTime * AttenRate));
        }
        else
        {
            nowPos = TargetObject.transform.position + Vector3.up * Height;
        }
        //減衰挙動をさせるかどうか
        if (EnavleAtten)
        {
            nowRotAngle = Mathf.Lerp(nowRotAngle, RotAngle, Time.deltaTime * RotAngeleRate);
        }
        else
        {
            nowRotAngle = RotAngle;
        }
        //減衰挙動をさせるかどうか
        if (EnavleAtten)
        {
            nowHeightAngle = Mathf.Lerp(nowHeightAngle, HeightAngle, Time.deltaTime + RotAngeleRate); ;
        }
        else
        {
            nowHeightAngle = HeightAngle;
        }

        //プレイヤーとのカメラの高さに応じて距離をZ距離を変更する
        if (HeightAngle > 40)
        {
            Distance = Mathf.Lerp(Distance, 5.0f * HeightAngle / 50.0f, CameraSpeed * Time.deltaTime);
        }
        else if (HeightAngle <= 40 && HeightAngle >= 10)
        {
            Distance = Mathf.Lerp(Distance, 1.0f, CameraSpeed * Time.deltaTime);
        }
        else if (HeightAngle <= 10)
        {
            Distance = Mathf.Lerp(Distance, dis_min, CameraSpeed * Time.deltaTime);
        }



        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;

        transform.position = nowPos + new Vector3(cx, cy, cz);
        var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        if (EnavleAtten)
        {
            transform.rotation = rot;
        }
        TargetLook();
    }

    void TargetLook()
    {
        //ターゲットをセットする
        if (Input.GetKeyDown("joystick button 9"))
        {
            //リストの数が0個何もなければ処理を終了
            if (enemyListManager.EnemyList.Count == 0) 
            {
                return;
            }

            //リストの数をカウントを超えたら0にリセットする
            if (enemyListManager.EnemyList.Count <= TargetCount)
            {
                TargetCount = 0;
            }
            //ターゲットをリストからセットする
            Target = enemyListManager.EnemyList[TargetCount];
            
            if(Target)
            {
                TargetNo = TargetCount;
            }

            enemyListManager.EnemyList[TargetCount].GetComponent<EnemyBase>().LookOn();
            TargetCount++;
        }

        if (Input.GetKeyDown("joystick button 8"))
        {
            enemyListManager.EnemyList[TargetNo].GetComponent<EnemyBase>().LookOff();
            Target = null;
        }

        if (enemyListManager.EnemyList.Count == 0)
        {
            Target = null;
        }

        if (Target)
        {
            var pos = Vector3.zero;
            pos = Target.position;
            //カメラが上下しないように、高さはカメラ基準にする
            pos.y = transform.position.y;

            transform.LookAt(pos);
        }

    }
}
