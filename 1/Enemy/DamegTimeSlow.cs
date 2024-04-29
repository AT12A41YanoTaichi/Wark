using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�U�����Ƀq�b�g�X�g�b�v��������v���O����

public class DamegTimeSlow : MonoBehaviour
{

    [Header("�x���Ȃ鎞�̎���")]
    [SerializeField] private float timeScale = 0.1f;
    [Header("�x�����Ă��鎞��")]
    [SerializeField] private float slowTime = 1f;
    //�o�ߎ���
    private float elapsedTime = 0f;
    //���Ԃ�x�����Ă��邩����
    public bool isSlowDown = false;

 
   // Update is called once per frame
    void Update()
    {
        if (!isSlowDown)
        {
            return;
        }
        elapsedTime += Time.unscaledDeltaTime;
        if (elapsedTime >= slowTime)
        {
            SetNormalTime();
        }
    }

    //�@���Ԃ�x�点�鏈��
    public void SlowDown()
    {
        elapsedTime = 0f;
        Time.timeScale = timeScale;
        isSlowDown = true;
    }
    //�@���Ԃ����ɖ߂�����
    public void SetNormalTime()
    {
        Time.timeScale = 1f;
        isSlowDown = false;
    }

}
