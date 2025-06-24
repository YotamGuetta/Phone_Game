using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

[Serializable]
public struct EquipmentSlots 
{
    [SerializeField]
    public itemType type;
    [SerializeField]
    public InventorySlot itemSlots;
}

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;
    [SerializeField] private List<EquipmentSlots> equipmentSlots;
    [SerializeField] private int gold;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private UseItem useItem;
    private CanvasGroup inventoryCanvasGroup;
    private Dictionary<itemType, InventorySlot> equipmentDictionery;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }
    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
        inventoryCanvasGroup = GetComponent<CanvasGroup>();

        equipmentDictionery = new Dictionary<itemType, InventorySlot>();
        foreach (var item in equipmentSlots)
        {
            item.itemSlots.UpdateUI();
            equipmentDictionery.Add(item.type, item.itemSlots);
        }
    }

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;
        goldText.text = gold.ToString();
    }
    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    public void ToggleInventoryView()
    {
        ToggleInventoryView(inventoryCanvasGroup.alpha == 0);
    }
    public void ToggleInventoryView(bool turnOn)
    {
        if (turnOn)
        {
            Time.timeScale = 0;
            inventoryCanvasGroup.alpha = 1;
        }
        else
        {
            Time.timeScale = 1;
            inventoryCanvasGroup.alpha = 0;
        }
        inventoryCanvasGroup.blocksRaycasts = turnOn;
        inventoryCanvasGroup.interactable = turnOn;
    }

    //Adds Item to inventory
    public void AddItem(ItemAbs_SO itemSO, int quantity)
    {
        //Add gold to the inventory if the item is gold
        if (itemSO.IsGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();
            return;
        }

        //Search for an item in the inventory if its stackable
        if (itemSO.IsStackable())
        {
            foreach (var slot in itemSlots)
            {
                if (slot.itemSO == itemSO)
                {
                    if (slot.itemSO is ConsumableSO consumableItem && slot.quantity < consumableItem.StackSize)
                    {
                        int availableSpace = consumableItem.StackSize - slot.quantity;
                        int amountToAdd = Mathf.Min(availableSpace, quantity);

                        slot.quantity += amountToAdd;
                        quantity -= amountToAdd;

                        slot.UpdateUI();
                        if (quantity <= 0)
                        {
                            return;
                        }
                    }
                }
            }

        }

        //Search for an empty slot to put the item
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = 1;
                if (itemSO.IsStackable())
                {
                    amountToAdd = Mathf.Min(((ConsumableSO)itemSO).StackSize, quantity);
                    //quantity -= amountToAdd;
                }

                slot.itemSO = itemSO;
                slot.quantity = amountToAdd;
                slot.UpdateUI();
                return;
            }
        }
        if (quantity > 0)
        {
            dropLoot(itemSO, quantity);
        }
    }
    public bool InventorySlotISEquipmentSlot(InventorySlot inventorySlot) 
    {
        return equipmentDictionery.ContainsValue(inventorySlot);
    }

    private void addItemToEquipmentSlot(ItemAbs_SO itemSO, InventorySlot slot)
    {
        if (slot == null || itemSO == null)
        {
            return;
        }
        if (itemSO.IsStackable())
        {
            Debug.LogError("can't equip stackable item : " + itemSO);
        }
        slot.itemSO = itemSO;
        slot.quantity = 1;
        slot.UpdateUI();
    }
    public void removeEquipedItem(InventorySlot slot)
    {
        if (slot == null || slot.itemSO == null)
        {
            return;
        }
        ItemAbs_SO itemSO = slot.itemSO;
        useItem.ApplyItemEffects(itemSO, true);
        AddItem(itemSO, 1);
        slot.itemSO = null;
        slot.UpdateUI();
    }
    public void DropItem(InventorySlot slot)
    {
        dropLoot(slot.itemSO, 1);
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
    }

    private void dropLoot(ItemAbs_SO itemSO, int quantity)
    {
        Loot loot = Instantiate(lootPrefab, player.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemSO, quantity);
    }
    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO == null || slot.quantity <= 0)
        {
            return;
        }

        itemType type = slot.itemSO.GetItemType();
        if (type != itemType.Consumable)
        {
            equipmentDictionery.TryGetValue(type, out InventorySlot equipmentInventorySlot);

            removeEquipedItem(equipmentInventorySlot);
            addItemToEquipmentSlot(slot.itemSO, equipmentInventorySlot);
        }

        useItem.ApplyItemEffects(slot.itemSO);
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.itemSO = null;
        }

        slot.UpdateUI();
    }

}
