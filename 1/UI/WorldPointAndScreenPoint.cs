using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ワールド空間上のプレイヤーに重なるようにUIを表示させる


public class WorldPointAndScreenPoint : MonoBehaviour
{

    //ワールド座標をスクリーン座標に変換する
    public Vector3 WorldPointToScreenPoint(Vector3 targetPos, Camera targetCamera)
    {
        return targetCamera.WorldToScreenPoint(targetPos);
    }

    //スクリーン座標をワールド座標に変換する
    public static Vector3 ScreenPointToWorldPoint(Vector3 targetPos, Camera targetCamera)
    {
        return targetCamera.ScreenToWorldPoint(targetPos);
    }
}
