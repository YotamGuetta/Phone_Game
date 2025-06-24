using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemAbs_SO itemSO;
    public int quantity;

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;

    private InventoryManager inventoryManager;
    private static ShopManager activeShop;

    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }
    private void OnEnable()
    {
        ShopKeeper.OnShopStateChanged += HandleShopStateChanged;
    }
    private void OnDisable()
    {
        ShopKeeper.OnShopStateChanged -= HandleShopStateChanged;
    }

    private void HandleShopStateChanged(ShopManager shopManager, bool isOpen)
    {
        activeShop = isOpen ? shopManager : null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (quantity > 0) 
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (activeShop != null)
                {
                    if (activeShop.SellItem(itemSO))
                    {
                        quantity--;
                        UpdateUI();
                    }
                }
                else 
                {
                    if (inventoryManager.InventorySlotISEquipmentSlot(this))
                    {
                        inventoryManager.removeEquipedItem(this);
                    }
                    else
                    {
                        inventoryManager.UseItem(this);
                    }
                }             
            }
            else if (eventData.button == PointerEventData.InputButton.Right) 
            {
                inventoryManager.DropItem(this);
            }
        }
        
    }

    public void UpdateUI() 
    {
        if (quantity <= 0) 
        {
            itemSO = null;
        }

        if (itemSO != null)
        {
            itemImage.sprite = itemSO.ItemIcon;
            itemImage.gameObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else 
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }
}
