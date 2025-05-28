using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private ItemAbs_SO itemSO;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image itemImage;

    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopInfo shopInfo;

    public int Price { get; set; }
    public ItemAbs_SO SlotItemSO { get { return itemSO; } }

    public void Initialize(ItemAbs_SO newItemSO, int price) 
    {
        itemSO = newItemSO;
        itemImage.sprite = itemSO.ItemIcon;
        itemNameText.text = itemSO.ItemName;
        this.Price = price;
        priceText.text = price.ToString();
    }

    public void OnBuyButtonClicked() 
    {
        shopManager.TryBuyItem(itemSO, Price);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null)
        {
            shopInfo.ShowItemInfo(itemSO);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemSO != null)
        {
            shopInfo.FollowMouse();
        }
    }
}
