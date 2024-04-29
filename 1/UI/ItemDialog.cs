using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDialog : MonoBehaviour
{
    [SerializeField] private int buttonNumver = 15;
    [SerializeField] private ItemButton itemButton;

    private ItemButton[] _itemButtons;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        for (var i = 0; i < buttonNumver - 1; i++) 
        {
            Instantiate(itemButton,transform);
        }

        _itemButtons = GetComponentsInChildren<ItemButton>();

    }

    //アイテムUIをアクティブにする
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if(gameObject.activeSelf)
        {
            for (var i = 0; i < buttonNumver; i++) 
            {
                _itemButtons[i].OwnedItem = OwnedltemsData.Instance.OwnedItems.Length > i 
                    ? OwnedltemsData.Instance.OwnedItems[i] : null;
            }
        }
        _itemButtons[0].GetComponent<Button>().Select();
    }
}
