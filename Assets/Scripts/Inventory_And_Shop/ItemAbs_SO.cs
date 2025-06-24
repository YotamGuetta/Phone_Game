using System.Collections.Generic;
using UnityEngine;


public abstract class ItemAbs_SO : ScriptableObject
{
    [SerializeField] private string itemName;
    [TextArea] [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemIcon;

    public abstract List<IStat> GetItemStats();
    public abstract bool IsStackable();

    public string ItemName { get { return itemName; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public string ItemDescription { get { return itemDescription; } }
    public bool IsGold { get { return this.GetType().Name == "GoldSO"; } }
    public virtual itemType GetItemType()
    {
        if (this is WeaponSO)
        {
            return itemType.Weapon;
        }
        return itemType.Consumable;
    }
}
public enum itemType
{
    Helmet,
    BodyArmor,
    Boots,
    Weapon,
    Consumable
}
