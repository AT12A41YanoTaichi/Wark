using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���[���h��ԏ�̃v���C���[�ɏd�Ȃ�悤��UI��\��������


public class WorldPointAndScreenPoint : MonoBehaviour
{

    //���[���h���W���X�N���[�����W�ɕϊ�����
    public Vector3 WorldPointToScreenPoint(Vector3 targetPos, Camera targetCamera)
    {
        return targetCamera.WorldToScreenPoint(targetPos);
    }

    //�X�N���[�����W�����[���h���W�ɕϊ�����
    public static Vector3 ScreenPointToWorldPoint(Vector3 targetPos, Camera targetCamera)
    {
        return targetCamera.ScreenToWorldPoint(targetPos);
    }
}
