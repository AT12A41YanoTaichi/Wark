using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;



public class SphereSensor : MonoBehaviour
{


    [SerializeField]
    private SphereCollider searchArea = default;
    [SerializeField]

    private EnemyTankAI enemyMove = default;
    private float searchAngle = 50f;
    private bool isDiscovery = false;

    private void Start()
    {

        enemyMove = transform.parent.GetComponent<EnemyTankAI>();

    }


    private void OnTriggerStay(Collider target)
    {
        if (target.tag == "Player")
        {
            EnemyTankAI.EnemyAIState state = enemyMove.GetState();
            var playerDirection = target.transform.position - this.transform.position;

            var angle = Vector3.Angle(transform.forward, playerDirection);
            if (angle <= searchAngle)
            {
                //これが一応ステートを切り替えるやつ↓
                //攻撃に切り替わってる
                //ここと同じやり方
                //そんでここは切り替わってる
                enemyMove.SetState(EnemyTankAI.EnemyAIState.Attack, target.transform);
                if (isDiscovery == false)
                {
                    enemyMove.discovery.SetActive(true);
                    isDiscovery = true;
                }

                Debug.Log("プレイヤー発見");
                if (Vector3.Distance(target.transform.position, transform.position) <= searchArea.radius * 1.0f
                        && Vector3.Distance(target.transform.position, transform.position) >= searchArea.radius * 0.5f)
                {
                    enemyMove.agent.isStopped = true;

                }
                else
                {
                    enemyMove.agent.isStopped = false;
                }

            }
            else if (angle > searchAngle)
            {

                if (state != EnemyTankAI.EnemyAIState.Teisatu)
                {
                    enemyMove.GotoNextPoint();
                    enemyMove.SetState(EnemyTankAI.EnemyAIState.Teisatu);
                    enemyMove.animator.SetBool("kougeki", false);
                    isDiscovery = false;
                    Debug.Log("見失った");

                }

            }




        }


    }

    void OnTriggerExit(Collider target)
    {
        if (target.tag == "Player")
        {
            EnemyTankAI.EnemyAIState state = enemyMove.GetState();
            if (state != EnemyTankAI.EnemyAIState.Teisatu)
            {
                enemyMove.GotoNextPoint();
                enemyMove.SetState(EnemyTankAI.EnemyAIState.Teisatu);
                enemyMove.animator.SetBool("kougeki", false);
                isDiscovery = false;
                Debug.Log("見失った");

            }
        }
    }


#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
    }
#endif
}