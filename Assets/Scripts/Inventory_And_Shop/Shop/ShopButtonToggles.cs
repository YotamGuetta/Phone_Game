using UnityEngine;

public class ShopButtonToggles : MonoBehaviour
{
    public void OpenItemShop() 
    {
        if (ShopKeeper.currentShopkeeper != null) 
        {
            ShopKeeper.currentShopkeeper.OpenItemShop();
        }
    }
    public void OpenWeaponShop()
    {
        if (ShopKeeper.currentShopkeeper != null)
        {
            ShopKeeper.currentShopkeeper.OpenWeaponShop();
        }
    }
    public void OpenArmorShop()
    {
        if (ShopKeeper.currentShopkeeper != null)
        {
            ShopKeeper.currentShopkeeper.OpenArmorShop();
        }
    }
}
