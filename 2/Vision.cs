using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [Header("ザコ敵のスクリプト取得")]
    [SerializeField] private WeakEnemy weakenemy;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーみっけ");
            if (weakenemy.enemyState != WeakEnemy.WeakEnemyState.Run&& weakenemy.enemyState != WeakEnemy.WeakEnemyState.CloseAttack)
            {
                weakenemy.SetWeakEnemyState(WeakEnemy.WeakEnemyState.Run);

            }
        }
    }
}
