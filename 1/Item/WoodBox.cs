using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//バラバラに壊れる木箱の処理

public class WoodBox : MonoBehaviour
{

    [SerializeField] private Item[] itemPrefab;
    [SerializeField] private int[] number;//アイテムの出現数

    private bool _isDropInvoked;

    //オブジェクトの子オブジェクトに分割しバラバラにする
    public void DestroyObject()
    {
        var random = new System.Random();
        //最小値
        var min = -3;
        //最大値
        var max = 3;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r =>
        {
            r.isKinematic = false;
            //親子関係を無くす
            r.transform.SetParent(null);
            //2秒後に消す処理
            r.gameObject.AddComponent<AutoDestroy>().time = 2f;
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
        DropIfNeeded();
    }

    //ItemPrefabの配列を同じ配列番号のnumberの数値分ドロップする
    public void DropIfNeeded()
    {
        if (_isDropInvoked)
        {
            return;
        }

        _isDropInvoked = true;

        for (int k = 0; k < itemPrefab.Length; k++)
        {
            int loopCount = number[k];
            for(int i = 0; i < loopCount; i++)
            {
                var item = Instantiate(itemPrefab[k], transform.position, Quaternion.identity);
                item.Initialize();
            }
        }


    }
}
