using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    [Header("ポーズのOnOff")]
    [SerializeField] private bool pause;

    [Header("ポーズのOnOff")]
    [SerializeField] private bool resipe = true;

    [Header("ポーズパネルの取得")]
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private ItemDialog itemsDialog;

    [Header("SlideUIControl取得")]
    [SerializeField] private SlideUIControl uiCont;

    [Header("Parent取得")]
    [SerializeField] GameObject parent;

    [Header("AddAppleUIの取得")]
    [SerializeField] GameObject itemUI;

    [Header("AddHerbUIの取得")]
    [SerializeField] GameObject herbUI;

    [Header("AddFlowerUIの取得")]
    [SerializeField] GameObject FlowerUI;

    [Header("AddHPPotionUIの取得")]
    [SerializeField] GameObject PotionUI;

    [Header("AddMPPotionUIの取得")]
    [SerializeField] GameObject MPPotionUI;

    [Header("MinusAppleUIの取得")]
    [SerializeField] GameObject MinusitemUI;

    [Header("MinusHerbUIの取得")]
    [SerializeField] GameObject MinusherbUI;

    [Header("MinusHerbUIの取得")]
    [SerializeField] GameObject MinusFlowerUI;

    [Header("MinusHPPotionUIの取得")]
    [SerializeField] GameObject MinusPotionUI;

    [Header("MinusMPPotionUIの取得")]
    [SerializeField] GameObject MinusMPPotionUI;

    [Header("RecipeUIの取得")]
    [SerializeField] GameObject RecipeUI;

    [Header("ItemDialogObjectの取得")]
    [SerializeField] GameObject ItemDialog;

    [Header("タイトルSceneの名前を入力")]
    [SerializeField] private string SampleScene;

    GameObject ManagerObject;
    SceneFadeManager fademanager;

    
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        RecipeUI.SetActive(false);
        ManagerObject = GameObject.Find("ManagerObject");
        fademanager = ManagerObject.GetComponent<SceneFadeManager>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        if (!RecipeUI.active && !ItemDialog.active)
        {
            if (Input.GetKeyDown("joystick button 7"))
            {
                pause ^= true;
            }
        }

        if(pause)
        {       
            if(!pausePanel.active)
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
            }
           
        }
        else
        {
            if (pausePanel.active)
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
            }
           
        }

        if(!pause)
        {

            if (Input.GetKeyDown("joystick button 6"))
            {
                ToggleItemsDialog();
            }


            if (Input.GetKeyDown("joystick button 2"))
            {
                resipe ^= true;
                if (resipe)
                {
                    RecipeUI.SetActive(true);
                }
                else
                {
                    RecipeUI.SetActive(false);
                }

            }
        }
        

    }


    private void ToggleItemsDialog()
    {
        itemsDialog.Toggle();
    }
   
    //リンゴ取得時スライドUI
   public void GetAppleItem()    
   {
        var icon = Instantiate(itemUI,itemUI.transform.position,Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;
    
   }
    //ハーブ取得時スライドUI
    public void GetHeabItem()
    {
        var icon = Instantiate(herbUI, herbUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //花取得時スライドUI
    public void GetFlowerItem()
    {
        var icon = Instantiate(FlowerUI, FlowerUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //HPポーション取得時スライドUI
    public void GetPotionItem()
    {
        var icon = Instantiate(PotionUI, PotionUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //MPポーション取得時スライドUI
    public void GetMPPotionItem()
    {
        var icon = Instantiate(MPPotionUI, MPPotionUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //リンゴ使用時スライドUI
    public void MinusAppleItem()
    {
        var icon = Instantiate(MinusitemUI, MinusitemUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //ハーブ使用時スライドUI
    public void MinusHeabItem()
    {
        var icon = Instantiate(MinusherbUI, MinusherbUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //花使用時スライドUI
    public void MinusFlowerItem()
    {
        var icon = Instantiate(MinusFlowerUI, MinusFlowerUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //HPポーション使用時スライドUI
    public void MinusPotionItem()
    {
        var icon = Instantiate(MinusPotionUI, MinusPotionUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }
    //MPポーション使用時スライドUI

    public void MinusMPPotionItem()
    {
        var icon = Instantiate(MinusMPPotionUI, MinusMPPotionUI.transform.position, Quaternion.identity);
        icon.SetActive(true);
        icon.transform.SetParent(parent.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, parent.GetComponent<DropUIMoveContorl>().top - (parent.transform.childCount - 1) * parent.GetComponent<DropUIMoveContorl>().interval, icon.transform.position.z);
        icon.transform.GetComponent<SlideUIControl>().posY = icon.transform.localPosition.y;
        icon.GetComponent<SlideUIControl>().state = 1;

    }


    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    public void StartTitle()
    { 
        fademanager.fadeOutStart(0, 0, 0, 0, SampleScene);
        pause = false;
    }


    public void resumptionGame()
    {
        pause = false;
    }
}
