using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


//�A�C�e���̎�ނ␶���̏���


[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public float nowPosY;
    public enum ItemType
    {
        Apple,
        Herb,
        Flower,
        HPPotion,
        MPPotion,
    }
    [SerializeField] public ItemType type;
    private int i;
    private string v;
    private Sprite sprite;

    public Item(int i, string v, Sprite sprite)
    {
        this.i = i;
        this.v = v;
        this.sprite = sprite;
    }

    void Start()
    {
        menu = GameObject.Find("MenuCanvas");
        nowPosY = this.transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, nowPosY + Mathf.PingPong(Time.time / 3, 0.3f), transform.position.z);
    }

    public void Initialize()
    {
        //�A�j���[�V�������I��܂œ����蔻��𖳌���
        var colliderCaher = GetComponent<Collider>();
        colliderCaher.enabled = false;

        //�o���A�j���[�V����
        var transformCache = transform;
        var dropPosition = transform.localPosition + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
        transformCache.DOLocalMove(dropPosition, 0.5f);
        var defaultScale = transformCache.localScale;
        transformCache.localScale = Vector3.zero;
        transformCache.DOScale(defaultScale, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                //�����蔻���L����
                colliderCaher.enabled = true;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        //�擾�����A�C�e����type�Ŕ��ʂ��ǉ�����
        OwnedltemsData.Instance.Add(type);
        //�擾�����A�C�e���̃X���C�hUI�����s
        if(type == ItemType.Apple)
        {
            menu.GetComponent<Menu>().GetAppleItem();
        }
        else if(type == ItemType.Herb)
        {
            menu.GetComponent<Menu>().GetHeabItem();
        }
        if (type == ItemType.HPPotion)
        {
            menu.GetComponent<Menu>().GetPotionItem();
        }
        else if (type == ItemType.MPPotion)
        {
            menu.GetComponent<Menu>().GetMPPotionItem();
        }
        else if (type == ItemType.Flower)
        {
            menu.GetComponent<Menu>().GetFlowerItem();
        }

        Destroy(gameObject);
    }

    //HP�A�C�e�������
    public void CreateHPPotion()
    {
        menu = GameObject.Find("MenuCanvas");
        if (OwnedltemsData.Instance.CountNumber(Item.ItemType.Herb) > 0&& OwnedltemsData.Instance.CountNumber(Item.ItemType.Apple) > 0)
        {
            //Herb�������
            OwnedltemsData.Instance.Use(Item.ItemType.Herb);
            //�����S�������
            OwnedltemsData.Instance.Use(Item.ItemType.Apple);
            //����A�C�e���̃X���C�hUI�����s
            menu.GetComponent<Menu>().MinusAppleItem();
            //����A�C�e���̃X���C�hUI�����s
            menu.GetComponent<Menu>().MinusHeabItem();
            OwnedltemsData.Instance.Add(ItemType.HPPotion);
            menu.GetComponent<Menu>().GetPotionItem();
        }
            
    }

    //MP�A�C�e�������
    public void CreateMPPotion()
    {
        menu = GameObject.Find("MenuCanvas");
        if (OwnedltemsData.Instance.CountNumber(Item.ItemType.Flower) > 0 && OwnedltemsData.Instance.CountNumber(Item.ItemType.Apple) > 0)
        {
            OwnedltemsData.Instance.Use(Item.ItemType.Flower);
            OwnedltemsData.Instance.Use(Item.ItemType.Apple);
            menu.GetComponent<Menu>().MinusFlowerItem();
            menu.GetComponent<Menu>().MinusAppleItem();
            OwnedltemsData.Instance.Add(ItemType.MPPotion);
            menu.GetComponent<Menu>().GetMPPotionItem();
        }
           
    }

}
