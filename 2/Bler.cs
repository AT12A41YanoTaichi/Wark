using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----
//����F���
//�ǋL�F
//�T�v�F�U�����ꂽ���̉��u������
///----
public class Bler : MonoBehaviour
{
    [SerializeField, Header("�e�I�u�W�F�N�g�̎擾")]
    private GameObject _parent;


    [SerializeField, Header("�h��鎞��")]
    private float Duration;
    [SerializeField, Header("�h��̋���")]
    private float Strength;
    [SerializeField, Header("�U���̋���")]
    private float Vibrato;
    private Vector3 _initPosition; // �����ʒu
    [SerializeField, Header("���s����")]
    private bool _isDoShake;       // �h����s�����H
    private float _totalShakeTime; // �h��o�ߎ���

    // Start is called before the first frame update
    void Start()
    {
        _isDoShake = false;
        _totalShakeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _initPosition = _parent.transform.position;

        if (_isDoShake)
        {
            // �h��ʒu���X�V
            gameObject.transform.position = UpdateShakePosition(
                gameObject.transform.position,
                _totalShakeTime,
                _initPosition);

            // duration���̎��Ԃ��o�߂�����h�炷�̂��~�߂�
            _totalShakeTime += Time.deltaTime;
            if (_totalShakeTime >= Duration)
            {
                _isDoShake = false;
                _totalShakeTime = 0.0f;
                // �����ʒu�ɖ߂�
            }
        }
        else
        { 
            // �����ʒu�ɖ߂� 
            gameObject.transform.position = _initPosition;
            //Strength = 0.2f;
           // Vibrato = 0.2f;
        }
       
    }
    private Vector3 UpdateShakePosition(Vector3 currentPosition, float totalTime, Vector3 initPosition)
    {
        // -strength ~ strength �̒l�ŗh��̋������擾
        var strength = Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        // var randomY = Random.Range(-1.0f * strength, strength);

        // ���݂̈ʒu�ɉ�����
        var position = currentPosition;
        position.x += randomX;
        // position.y += randomY;

        // �����ʒu-vibrato ~ �����ʒu+vibrato �̊ԂɎ��߂�
        var vibrato = Vibrato;
        var ratio = 1.0f - totalTime / Duration;
        vibrato *= ratio; // �t�F�[�h�A�E�g�����邽�߁A�o�ߎ��Ԃɂ��h��̗ʂ�����
        position.x = Mathf.Clamp(position.x, initPosition.x - vibrato, initPosition.x + vibrato);
        //position.y = Mathf.Clamp(position.y, initPosition.y - vibrato, initPosition.y + vibrato);
        return position;
    }
    public void isBler(bool bler, float Dameje)
    {
        _isDoShake = bler;
        Strength = Strength * Dameje / 20;
        Vibrato = Vibrato * Dameje / 20;
    }
    
}
