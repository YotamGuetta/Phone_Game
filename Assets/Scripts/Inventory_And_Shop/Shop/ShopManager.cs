using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private ShopSlot[] shopSlots;

    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private float itemSellMultiplier = 0.5f;

    //Updates the shop when the player toggles between shop types
    public void PopulateShopItems(List<ShopItems> shopItems)
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }
        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(ItemAbs_SO itemSO, int price)
    {
        if (itemSO != null && inventoryManager.Gold >= price) 
        {
            if (HasSpaceForItem(itemSO)) 
            {
                inventoryManager.Gold -= price;
                inventoryManager.AddItem(itemSO, 1);
            }
        }
    }

    private bool HasSpaceForItem(ItemAbs_SO itemSO)
    {
        foreach (var slot in inventoryManager.itemSlots)
        {

            if (slot.itemSO == itemSO && slot.itemSO.IsStackable() && slot.quantity < ((ConsumableSO)slot.itemSO).StackSize)
            {
                return true;
            }
            else if (slot.itemSO == null)
            {
                return true;
            }
        }

        return false;
    }

    public bool SellItem(ItemAbs_SO itemSO)
    {
        if (itemSO == null) 
        {
            return false;
        }
        foreach (var slot in shopSlots)
        {
            //if the shopkeeper owns the items he sells it for a redused price
            if (slot.SlotItemSO == itemSO)
            {
                inventoryManager.Gold += Mathf.RoundToInt(slot.Price * itemSellMultiplier);
                return true;
            }
        }
        return false;
    }
}


[System.Serializable]
public class ShopItems
{
    public ItemAbs_SO itemSO;
    public int price;
}
