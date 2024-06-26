using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropUIMoveContorl : MonoBehaviour
{
    //生成位置
    public float top = 100.0f;
    //移動するまでの時間
    public float interval = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < this.gameObject.transform.childCount; ++i)
            {
                if (this.gameObject.transform.GetChild(i).GetComponent<SlideUIControl>().posY != top - i * interval)
                {
                    this.gameObject.transform.GetChild(i).GetComponent<SlideUIControl>().GetPosY(top - i * interval);
                }
            }
        }



    }
}
