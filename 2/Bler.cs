using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----
//制作：矢野
//追記：
//概要：攻撃された時の横ブレ処理
///----
public class Bler : MonoBehaviour
{
    [SerializeField, Header("親オブジェクトの取得")]
    private GameObject _parent;


    [SerializeField, Header("揺れる時間")]
    private float Duration;
    [SerializeField, Header("揺れの強さ")]
    private float Strength;
    [SerializeField, Header("振動の強さ")]
    private float Vibrato;
    private Vector3 _initPosition; // 初期位置
    [SerializeField, Header("実行する")]
    private bool _isDoShake;       // 揺れ実行中か？
    private float _totalShakeTime; // 揺れ経過時間

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
            // 揺れ位置情報更新
            gameObject.transform.position = UpdateShakePosition(
                gameObject.transform.position,
                _totalShakeTime,
                _initPosition);

            // duration分の時間が経過したら揺らすのを止める
            _totalShakeTime += Time.deltaTime;
            if (_totalShakeTime >= Duration)
            {
                _isDoShake = false;
                _totalShakeTime = 0.0f;
                // 初期位置に戻す
            }
        }
        else
        { 
            // 初期位置に戻す 
            gameObject.transform.position = _initPosition;
            //Strength = 0.2f;
           // Vibrato = 0.2f;
        }
       
    }
    private Vector3 UpdateShakePosition(Vector3 currentPosition, float totalTime, Vector3 initPosition)
    {
        // -strength ~ strength の値で揺れの強さを取得
        var strength = Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        // var randomY = Random.Range(-1.0f * strength, strength);

        // 現在の位置に加える
        var position = currentPosition;
        position.x += randomX;
        // position.y += randomY;

        // 初期位置-vibrato ~ 初期位置+vibrato の間に収める
        var vibrato = Vibrato;
        var ratio = 1.0f - totalTime / Duration;
        vibrato *= ratio; // フェードアウトさせるため、経過時間により揺れの量を減衰
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
