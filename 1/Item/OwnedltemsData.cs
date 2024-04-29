using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class OwnedltemsData
{
    //PlayerPrefskey保存先キー
    private const string PlayerPrefsKey = "4";

    //インスタンスを返す
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

    //どのアイテムを何個取得しているのかリスト
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

    //アイテムを追加する
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
    //アイテムを消費する
    public void Use(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item || item.Number < number)
        {
            throw new Exception("アイテムが足りません");
        }
        item.Use(number);
    }

    //アイテムタイプ別に個数を取得する
    public int CountNumber(Item.ItemType type)
    {
        var item = GetItem(type);
        return item.Number;
    }

    //アイテムタイプ別に取得したものを返す
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
        //アイテムのタイプを取得する
        public Owneditem(Item.ItemType type)
        {
            this.type = type;
        }
        //アイテムの個数を増やす
        public void Add(int numbar = 1)
        {
            this.number += numbar;
        }
        //アイテムの個数を減らす
        public void Use(int number = 1)
        {
            this.number -= number;
        }
       
    }


}
