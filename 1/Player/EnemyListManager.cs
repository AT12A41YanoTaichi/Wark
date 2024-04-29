using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�͈͓��ɂ���G�����X�g�ɒǉ��A�͈͊O���폜���鏈��

public class EnemyListManager : MonoBehaviour
{

    public List<Transform> EnemyList = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        //���X�g���ŏd�����Ȃ��悤�ɂ���
        for (int i = 0; i < EnemyList.Count; i++)
        {
            //���̂�����r����
            for (int k = i + 1; k < EnemyList.Count; k++)
            {
                //�d���������������폜����
                if (EnemyList[i] == EnemyList[k]) 
                {
                    EnemyList.RemoveAt(k);
                }
            }
            //�G���폜�ς݂Ȃ烊�X�g������폜
            if (!EnemyList[i])
            {
                EnemyList.RemoveAt(i);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //�͈͓��ɓ������G�����X�g�ɒǉ�����
        if (other.tag == "Enemy")
        {
            EnemyList.Add(other.gameObject.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                //���X�g���瓯���G�������č폜����
                if (EnemyList[i] == other.gameObject.transform)
                {
                    other.GetComponent<EnemyBase>().LookOff();
                    EnemyList.RemoveAt(i);
                }
            }
        }
    }


}
