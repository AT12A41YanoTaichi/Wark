using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCheck : MonoBehaviour
{

    bool damagecheck;
    int ColorTime;

    Material sphere1;

    public EnemyTankAI Enemy;

    public Material[] materialArray;
    
    void Start()
    {
        ColorTime = 0;
        damagecheck = false;
      
        GetComponent<SkinnedMeshRenderer>().material = materialArray[0];
      
    }

    // Update is called once per frame
    void Update()
    {


       
        if (damagecheck == true && ColorTime >= 10)
        {
            
            GetComponent<SkinnedMeshRenderer>().material = materialArray[0];

            
            damagecheck = false;
            ColorTime = 0;
            Enemy.DamageCheck = false;
           
        }


        if (Enemy.DamageCheck==true)
        {

            
            GetComponent<SkinnedMeshRenderer>().material = materialArray[1];

           


            damagecheck = true;
            ColorTime++;

        }
        
        
    }
    
   
}
