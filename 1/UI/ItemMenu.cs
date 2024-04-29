using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ItemMenu : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;
    public bool buttonDownFlag;

    public GameObject[] Item;
    public GameObject Cursor;

    private float currentIndex;
    public int SlectIndex;
    public bool KeyDonw = false;
    public GameObject RBButton;

    // Start is called before the first frame update
    [Obsolete]
    void Start()
    {
        for (int i = 1; i < Item.Length; i++)
        {
            Item[i].SetActive(false);
        }
        Cursor.SetActive(false);
        RBButton.SetActive(true);
    }

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
        UpdateMControlerDisplay();

        float tri = Input.GetAxis ("L_R_Trigger");
        if (tri < 0)
        {
            //LTボタンUI非アクティブ
            RBButton.SetActive(false);
            KeyDonw = true;
            //ItemUIアクティブ、スライドUI開始
            for (int i = 0; i < Item.Length; i++)
            {
                Item[i].SetActive(true);
                Item[i].GetComponent<SlideUIControl>().state = 1;
            }
            //カーソルUIアクティブ
            Cursor.SetActive(true);
        }
        else
        {
            if (KeyDonw)
            {
                //選択アイテム番号取得
                SlectIndex = (int)currentIndex;
                KeyDonw = false;
            }
            //ItemUI非アクティブ、スライドUI戻す
            for (int i = 0; i < Item.Length; i++)
            {
                Item[i].GetComponent<SlideUIControl>().state = 2;
                if (i != SlectIndex)
                {
                    Item[i].SetActive(false);
                }
            }
            //カーソルUI非アクティブ
            Cursor.SetActive(false);
            //LTボタンUIアクティブ
            RBButton.SetActive(true);
        }
       
    }




    //Rスティックの方向からカーソルの角度を取得します。
    private float GetDirectionFromControler()
    {
        var v = Input.GetAxis("R_Stick_H");
        var h = Input.GetAxis("R_Stick_V");

        float degree = Mathf.Atan2(-v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
        return degree;
    }

    //Rスティックの角度に応じてカーソルを回転させる
    private void UpdateMControlerDisplay()
    {
        float currentAngle = GetDirectionFromControler();
        int selectedSlotIndex = GetSelectedSlot(currentAngle);
        currentAngle = selectedSlotIndex * 90.0f;
        cursor.eulerAngles = new Vector3(cursor.eulerAngles.x, cursor.eulerAngles.y, currentAngle);
    }
    
    //カーソルの角度からアイテムスロットを番号を取得する
    public int GetSelectedSlot(float cursorDegree)
    {
        currentIndex = cursorDegree / 90.0f;
        currentIndex = (float)Math.Round(currentIndex, MidpointRounding.AwayFromZero);


        return (int)currentIndex;
    }

}
