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
            //LT�{�^��UI��A�N�e�B�u
            RBButton.SetActive(false);
            KeyDonw = true;
            //ItemUI�A�N�e�B�u�A�X���C�hUI�J�n
            for (int i = 0; i < Item.Length; i++)
            {
                Item[i].SetActive(true);
                Item[i].GetComponent<SlideUIControl>().state = 1;
            }
            //�J�[�\��UI�A�N�e�B�u
            Cursor.SetActive(true);
        }
        else
        {
            if (KeyDonw)
            {
                //�I���A�C�e���ԍ��擾
                SlectIndex = (int)currentIndex;
                KeyDonw = false;
            }
            //ItemUI��A�N�e�B�u�A�X���C�hUI�߂�
            for (int i = 0; i < Item.Length; i++)
            {
                Item[i].GetComponent<SlideUIControl>().state = 2;
                if (i != SlectIndex)
                {
                    Item[i].SetActive(false);
                }
            }
            //�J�[�\��UI��A�N�e�B�u
            Cursor.SetActive(false);
            //LT�{�^��UI�A�N�e�B�u
            RBButton.SetActive(true);
        }
       
    }




    //R�X�e�B�b�N�̕�������J�[�\���̊p�x���擾���܂��B
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

    //R�X�e�B�b�N�̊p�x�ɉ����ăJ�[�\������]������
    private void UpdateMControlerDisplay()
    {
        float currentAngle = GetDirectionFromControler();
        int selectedSlotIndex = GetSelectedSlot(currentAngle);
        currentAngle = selectedSlotIndex * 90.0f;
        cursor.eulerAngles = new Vector3(cursor.eulerAngles.x, cursor.eulerAngles.y, currentAngle);
    }
    
    //�J�[�\���̊p�x����A�C�e���X���b�g��ԍ����擾����
    public int GetSelectedSlot(float cursorDegree)
    {
        currentIndex = cursorDegree / 90.0f;
        currentIndex = (float)Math.Round(currentIndex, MidpointRounding.AwayFromZero);


        return (int)currentIndex;
    }

}
