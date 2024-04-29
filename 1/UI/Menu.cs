using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    [Header("�|�[�Y��OnOff")]
    [SerializeField] private bool pause;

    [Header("�|�[�Y��OnOff")]
    [SerializeField] private bool resipe = true;

    [Header("�|�[�Y�p�l���̎擾")]
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private ItemDialog itemsDialog;

    [Header("SlideUIControl�擾")]
    [SerializeField] private SlideUIControl uiCont;

    [Header("Parent�擾")]
    [SerializeField] GameObject parent;

    [Header("AddAppleUI�̎擾")]
    [SerializeField] GameObject itemUI;

    [Header("AddHerbUI�̎擾")]
    [SerializeField] GameObject herbUI;

    [Header("AddFlowerUI�̎擾")]
    [SerializeField] GameObject FlowerUI;

    [Header("AddHPPotionUI�̎擾")]
    [SerializeField] GameObject PotionUI;

    [Header("AddMPPotionUI�̎擾")]
    [SerializeField] GameObject MPPotionUI;

    [Header("MinusAppleUI�̎擾")]
    [SerializeField] GameObject MinusitemUI;

    [Header("MinusHerbUI�̎擾")]
    [SerializeField] GameObject MinusherbUI;

    [Header("MinusHerbUI�̎擾")]
    [SerializeField] GameObject MinusFlowerUI;

    [Header("MinusHPPotionUI�̎擾")]
    [SerializeField] GameObject MinusPotionUI;

    [Header("MinusMPPotionUI�̎擾")]
    [SerializeField] GameObject MinusMPPotionUI;

    [Header("RecipeUI�̎擾")]
    [SerializeField] GameObject RecipeUI;

    [Header("ItemDialogObject�̎擾")]
    [SerializeField] GameObject ItemDialog;

    [Header("�^�C�g��Scene�̖��O�����")]
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
   
    //�����S�擾���X���C�hUI
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
    //�n�[�u�擾���X���C�hUI
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
    //�Ԏ擾���X���C�hUI
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
    //HP�|�[�V�����擾���X���C�hUI
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
    //MP�|�[�V�����擾���X���C�hUI
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
    //�����S�g�p���X���C�hUI
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
    //�n�[�u�g�p���X���C�hUI
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
    //�Ԏg�p���X���C�hUI
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
    //HP�|�[�V�����g�p���X���C�hUI
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
    //MP�|�[�V�����g�p���X���C�hUI

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
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
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
