using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//�o���o���ɉ���ؔ��̏���

public class WoodBox : MonoBehaviour
{

    [SerializeField] private Item[] itemPrefab;
    [SerializeField] private int[] number;//�A�C�e���̏o����

    private bool _isDropInvoked;

    //�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɕ������o���o���ɂ���
    public void DestroyObject()
    {
        var random = new System.Random();
        //�ŏ��l
        var min = -3;
        //�ő�l
        var max = 3;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r =>
        {
            r.isKinematic = false;
            //�e�q�֌W�𖳂���
            r.transform.SetParent(null);
            //2�b��ɏ�������
            r.gameObject.AddComponent<AutoDestroy>().time = 2f;
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
        DropIfNeeded();
    }

    //ItemPrefab�̔z��𓯂��z��ԍ���number�̐��l���h���b�v����
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
