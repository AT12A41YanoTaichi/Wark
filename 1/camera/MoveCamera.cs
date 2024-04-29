using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//�J�����̈ړ��A��]����

public class MoveCamera : MonoBehaviour
{
    public GameObject TargetObject;
    [Header("�J�����̍����̃I�t�Z�b�g")]
    [SerializeField]public float Height = 1.5f;
    [Header("�J�����Ƃ̃I�t�Z�b�g")]
    [SerializeField] public float Distance = 15.0f;//
    [Header("���������̃J�����p�x")]
    [SerializeField] public float RotAngle = 0.0f;
    [Header("���������̃J�����p�x")]
    [SerializeField] public float HeightAngle = 10.0f;
    [Header("���グ�����̃J��������")]
    [SerializeField] public float dis_min = 5.0f;
    [Header("�ʏ�̃J��������")]
    [SerializeField] public float dis_mdl = 10.0f;
    [Header("�J�����̈ړ����x")]
    [SerializeField] public float CameraSpeed = 5;
    private Vector3 nowPos;
    //���̉�]�p�x
    private float nowRotAngle;
    //���̍����p�x
    private float nowHeightAngle;
    //�X�e�B�b�N���͂̐��l����
    private float rsv;
    private float rsh;

    private Vector3 Speed;
    public float RotSpped;

    //��������
    [Header("���������p�t���O")]
    [SerializeField] public bool EnavleAtten = true;
    public float AttenRate = 3.0f;
    public float ForwardDistance = 2.0f;
    private Vector3 addFoward;
    private Vector3 prevTargetPos;
    public float RotAngeleRate = 5.0f;
    public float AngleAttenRate = 1.0f;

    [Header("�GHPUI�̎擾")]
    [SerializeField] public Image HPUI;

    [Header("�G�̎擾")]
    [SerializeField] public GameObject Enemy;

    [Header("EnemyListManager�̎擾")]
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

        //�A�C�e����I�𒆂��ǂ���
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
            var target = TargetObject.transform.position;//�^�[�Q�b�g�̈ʒu���v���C���[�ɂ���

            var halfPoint = (TargetObject.transform.position + target)/2;
            var deltaPos = halfPoint - prevTargetPos;//�ʒu�̔���������
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
        //���������������邩�ǂ���
        if (EnavleAtten)
        {
            nowRotAngle = Mathf.Lerp(nowRotAngle, RotAngle, Time.deltaTime * RotAngeleRate);
        }
        else
        {
            nowRotAngle = RotAngle;
        }
        //���������������邩�ǂ���
        if (EnavleAtten)
        {
            nowHeightAngle = Mathf.Lerp(nowHeightAngle, HeightAngle, Time.deltaTime + RotAngeleRate); ;
        }
        else
        {
            nowHeightAngle = HeightAngle;
        }

        //�v���C���[�Ƃ̃J�����̍����ɉ����ċ�����Z������ύX����
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
        //�^�[�Q�b�g���Z�b�g����
        if (Input.GetKeyDown("joystick button 9"))
        {
            //���X�g�̐���0�����Ȃ���Ώ������I��
            if (enemyListManager.EnemyList.Count == 0) 
            {
                return;
            }

            //���X�g�̐����J�E���g�𒴂�����0�Ƀ��Z�b�g����
            if (enemyListManager.EnemyList.Count <= TargetCount)
            {
                TargetCount = 0;
            }
            //�^�[�Q�b�g�����X�g����Z�b�g����
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
            //�J�������㉺���Ȃ��悤�ɁA�����̓J������ɂ���
            pos.y = transform.position.y;

            transform.LookAt(pos);
        }

    }
}
