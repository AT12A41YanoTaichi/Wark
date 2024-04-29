using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//範囲内にいる敵をリストに追加、範囲外を削除する処理

public class EnemyListManager : MonoBehaviour
{

    public List<Transform> EnemyList = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        //リスト内で重複しないようにする
        for (int i = 0; i < EnemyList.Count; i++)
        {
            //次のやつから比較する
            for (int k = i + 1; k < EnemyList.Count; k++)
            {
                //重複したやつが居たら削除する
                if (EnemyList[i] == EnemyList[k]) 
                {
                    EnemyList.RemoveAt(k);
                }
            }
            //敵が削除済みならリストからも削除
            if (!EnemyList[i])
            {
                EnemyList.RemoveAt(i);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //範囲内に入った敵をリストに追加する
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
                //リストから同じ敵を見つけて削除する
                if (EnemyList[i] == other.gameObject.transform)
                {
                    other.GetComponent<EnemyBase>().LookOff();
                    EnemyList.RemoveAt(i);
                }
            }
        }
    }


}
