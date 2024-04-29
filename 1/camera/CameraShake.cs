using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�U�����󂯂����̃J�����̗h�ꐧ��

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    //�ǂꂭ�炢�h�炷��
    private float shakeDuration = 0.3f;
    public void CameraShaker()
    {
        //�V�[�P���X���N���A
        cam.DOComplete();
        //�J������h�炷
        cam.DOShakePosition(shakeDuration, positionStrength);
        //�J��������]������
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }
}
