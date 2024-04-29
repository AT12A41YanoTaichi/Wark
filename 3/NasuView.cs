using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class NasuView : MonoBehaviour
{
    [SerializeField]
    private SphereCollider searchArea = default;
    [SerializeField]

    private Eggplant enemyMove = default;
    private float searchAngle = 50f;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = transform.parent.GetComponent<Eggplant>();
    }

    private void OnTriggerStay(Collider target)
    {
        if (target.tag == "Player")
        {
            Eggplant.NasuState state = enemyMove.GetState();
            var playerDirection = target.transform.position - this.transform.position;

            var angle = Vector3.Angle(transform.forward, playerDirection);
            if (angle <= searchAngle)
            {
              
                enemyMove.SetState(Eggplant.NasuState.Attack, target.transform);
                Debug.Log("プレイヤー発見");
                //if (Vector3.Distance(target.transform.position, transform.position) <= searchArea.radius * 1.0f
                //        && Vector3.Distance(target.transform.position, transform.position) >= searchArea.radius * 0.5f)
                //{
                //    enemyMove.agent.isStopped = true;

                //}
                //else
                //{
                //    enemyMove.agent.isStopped = false;
                //}

            }
            else if (angle > searchAngle)
            {

                if (state != Eggplant.NasuState.Teisatu)
                {
                    enemyMove.animator.SetBool("Atack", false);
                    enemyMove.GotoNextPoint();
                    enemyMove.SetState(Eggplant.NasuState.Teisatu);
                    Debug.Log("見失った");

                }

            }




        }
    }

    void OnTriggerExit(Collider target)
    {
        if (target.tag == "Player")
        {
            Eggplant.NasuState state = enemyMove.GetState();
            if (state != Eggplant.NasuState.Teisatu)
            {
                enemyMove.animator.SetBool("Atack", false);
                enemyMove.GotoNextPoint();
                enemyMove.SetState(Eggplant.NasuState.Teisatu);
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