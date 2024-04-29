using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//攻撃を受けた時のカメラの揺れ制御

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    //どれくらい揺らすか
    private float shakeDuration = 0.3f;
    public void CameraShaker()
    {
        //シーケンスをクリア
        cam.DOComplete();
        //カメラを揺らす
        cam.DOShakePosition(shakeDuration, positionStrength);
        //カメラを回転させる
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }
}
