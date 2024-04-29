using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float interval;
    public float SpwanRange;
    private float time = 0f;
    public int count;
    public GameObject syugou;
    private Vector3 Syutugennbasyo;
    private bool NotInterval = true;

    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        interval = 0f;
        Syutugennbasyo = syugou.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if (count < 5)
        {
            if (time > interval)
            {
                //anim.SetBool("SetEnemy", false);
                Instantiate(enemyPrefab, new Vector3(Syutugennbasyo.x, Syutugennbasyo.y, Syutugennbasyo.z), Quaternion.identity);
                //anim.SetBool("SetEnemy", true);
                //anim.SetBool("SetEnemy", false);
                time = 0f;
                count += 1;
                if (NotInterval == true) 
                {
                    interval = 10.0f;
                    NotInterval = false;
                    
                    
                }
            }
            
           
        }
    }

    public int Count
    {
        get { return this.count; }
    }
}