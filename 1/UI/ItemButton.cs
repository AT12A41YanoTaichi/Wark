using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{

    [SerializeField] private ItemTypeSpriteMap[] itemSprites;
    [SerializeField] private Image image;
    [SerializeField] private Text number;

    private Button _button;
    private OwnedltemsData.Owneditem _ownedItem;

   public OwnedltemsData.Owneditem OwnedItem
   {
        get { return _ownedItem; }
        //ボタンの状態変更
        set
        {
            _ownedItem = value;

            var isEmpty = null == _ownedItem;
            image.gameObject.SetActive(!isEmpty);
            number.gameObject.SetActive(!isEmpty);
            _button.interactable = !isEmpty;
            if(!isEmpty)
            {
                image.sprite = itemSprites.First(x => x.itemType == _ownedItem.Type).sprite;
                number.text = "" + _ownedItem.Number;
            }

        }

   }
   

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    //アイテムタイプスプライトクラス
    [Serializable]
    public class ItemTypeSpriteMap
    {
        public Item.ItemType itemType;
        public Sprite sprite;
    }

}
