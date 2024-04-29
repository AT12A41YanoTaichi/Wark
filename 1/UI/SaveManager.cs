using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//セーブデータ管理

public class SaveManager : MonoBehaviour
{
    [Header("プレイヤーのHP")]
    [SerializeField] private GameObject player;

    [Header("SaveSlideUIの取得")]
    [SerializeField] GameObject SaveSlideUi;

    public Vector3 setPos;
    public int SetHP;
    public int SetEXP;
    public int SetLV;
    void Start()
    {
        setPos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX", 314), PlayerPrefs.GetFloat("PlayerPosY", 10), PlayerPrefs.GetFloat("PlayerPosZ", 117));

        SetHP = PlayerPrefs.GetInt("PlayerHP", 150);
        SetEXP = PlayerPrefs.GetInt("PlayerEXP",0);
        SetLV = PlayerPrefs.GetInt("PlayerLV", 1);
    }


    public void Seve()
    {
        SaveSlideUi.SetActive(true);
        SaveSlideUi.GetComponent<SlideUIControl>().state = 0;
        PlayerPrefs.SetInt("PlayerHP", player.GetComponent<PlayerBase>().GetHP());
        PlayerPrefs.SetInt("PlayerEXP", player.GetComponent<PlayerBase>().GetEXP());
        PlayerPrefs.SetInt("PlayerLV", player.GetComponent<PlayerBase>().GetLV());
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);
        PlayerPrefs.Save();    
        OwnedltemsData.Instance.Save();
    }

    public void Load()
    {
        setPos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX", 314), PlayerPrefs.GetFloat("PlayerPosY", 10), PlayerPrefs.GetFloat("PlayerPosZ", 117));

        SetHP = PlayerPrefs.GetInt("PlayerHP", 150);

        SetEXP = PlayerPrefs.GetInt("PlayerEXP", 0);

        SetLV = PlayerPrefs.GetInt("PlayerLV", 1);
    }

    public void Delet()
    {
        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            PlayerPrefs.DeleteKey("PlayerHP");
        }

        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            PlayerPrefs.DeleteKey("PlayerPosX");
        }

        if (PlayerPrefs.HasKey("PlayerPosY"))
        {
            PlayerPrefs.DeleteKey("PlayerPosY");
        }

        if (PlayerPrefs.HasKey("PlayerPosZ"))
        {
            PlayerPrefs.DeleteKey("PlayerPosZ");
        }

        if(PlayerPrefs.HasKey("PlayerEXP"))
        {
            PlayerPrefs.DeleteKey("PlayerEXP");
        }

        if (PlayerPrefs.HasKey("PlayerLV"))
        {
            PlayerPrefs.DeleteKey("PlayerLV");
        }

        OwnedltemsData.DeleteSaveData();

    }

    public Vector3 GetPlayerPos()
    {
        return setPos;
    }
    public int GetPlayerHP()
    {
        return SetHP;
    }

    public int GetPlayerLV()
    {
        return SetLV;
    }

    public int GetPlayerEXP()
    {
        return SetEXP;
    }

}
