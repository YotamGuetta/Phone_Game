using UnityEngine;
using TMPro;
using System;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;
    [SerializeField] private int gold;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private UseItem useItem;

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
        if (slot.itemSO != null && slot.quantity >= 0)
        {
            useItem.ApplyItemEffects(slot.itemSO);
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }
}
