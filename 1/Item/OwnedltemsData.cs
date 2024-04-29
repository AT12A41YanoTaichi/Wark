using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class OwnedltemsData
{
    //PlayerPrefskey�ۑ���L�[
    private const string PlayerPrefsKey = "4";

    //�C���X�^���X��Ԃ�
    public static OwnedltemsData Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = PlayerPrefs.HasKey(PlayerPrefsKey) 
                    ? JsonUtility.FromJson<OwnedltemsData>
                    (PlayerPrefs.GetString(PlayerPrefsKey)) 
                    : new OwnedltemsData();
            }

            return _instance;

        }
    }
    private static OwnedltemsData _instance;

    public Owneditem[] OwnedItems
    {
        get { return ownedItems.ToArray(); }
    }

    //�ǂ̃A�C�e�������擾���Ă���̂����X�g
    [SerializeField] private List<Owneditem> ownedItems = new List<Owneditem>();

    private OwnedltemsData()
    {
       
    }

    public void Save()
    {
        var jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PlayerPrefsKey, jsonString);
        PlayerPrefs.Save();
    }
    public static void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        PlayerPrefs.Save();
        _instance = null; // Reset the instance to force creating a new one next time it's accessed.
    }

    //�A�C�e����ǉ�����
    public void Add(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if(null == item)
        {
            item = new Owneditem(type);
            ownedItems.Add(item);
        }
        item.Add(number);
    }
    //�A�C�e���������
    public void Use(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item || item.Number < number)
        {
            throw new Exception("�A�C�e��������܂���");
        }
        item.Use(number);
    }

    //�A�C�e���^�C�v�ʂɌ����擾����
    public int CountNumber(Item.ItemType type)
    {
        var item = GetItem(type);
        return item.Number;
    }

    //�A�C�e���^�C�v�ʂɎ擾�������̂�Ԃ�
    public Owneditem GetItem(Item.ItemType type)
    {
        return ownedItems.FirstOrDefault(x => x.Type == type);
    }

    [Serializable]
    public class Owneditem
    {
        public Item.ItemType Type
        {
            get { return type; }
        }
        public int Number
        {
            get { return number; }
        }

        [SerializeField] private Item.ItemType type;
        [SerializeField] private int number = 0;
        //�A�C�e���̃^�C�v���擾����
        public Owneditem(Item.ItemType type)
        {
            this.type = type;
        }
        //�A�C�e���̌��𑝂₷
        public void Add(int numbar = 1)
        {
            this.number += numbar;
        }
        //�A�C�e���̌������炷
        public void Use(int number = 1)
        {
            this.number -= number;
        }
       
    }


}
