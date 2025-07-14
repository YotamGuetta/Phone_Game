using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public static event Action<ShopManager, bool> OnShopStateChanged;

    public static ShopKeeper currentShopkeeper;

    [SerializeField] private Animator anim;
    [SerializeField] private CanvasGroup shopCanvasGroup;
    [SerializeField] private ShopManager shopManager;

    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private List<ShopItems> shopWeapons;
    [SerializeField] private List<ShopItems> shopArmors;

    [SerializeField] private Camera shopKeeperCam;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -1);
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerInput playerInput;

    public bool IsShopOpen { get; private set; }

    public void ToggleShopPanel() 
    {
        //toggle the shop menu off/on and freezes the game accordingly
        Time.timeScale = shopCanvasGroup.alpha;
        MainMenu.ToggleCanvasGroup(shopCanvasGroup);
        IsShopOpen = shopCanvasGroup.alpha == 1;

        OnShopStateChanged?.Invoke(shopManager, IsShopOpen);

        //toggle the player inventory menu off/on
        inventoryManager.ToggleInventoryView(IsShopOpen);

        //sets the correct shopkepper to display
        if (IsShopOpen)
        {
            currentShopkeeper = this;
            shopKeeperCam.transform.position = transform.position + cameraOffset;
        }
        else
        {
            currentShopkeeper = null;
        }

        shopKeeperCam.gameObject.SetActive(IsShopOpen);

        OpenItemShop();
    }
    public void CloseShopPanel()
    {
        if (IsShopOpen) 
        {
            ToggleShopPanel();
        }
    }

    public void OpenItemShop() 
    {
        shopManager.PopulateShopItems(shopItems);
    }
    public void OpenWeaponShop()
    {
        shopManager.PopulateShopItems(shopWeapons);
    }
    public void OpenArmorShop()
    {
        shopManager.PopulateShopItems(shopArmors);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // did player enter shopkeeper's range
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInRange", true);
            playerInput.AddShopKeeperInRangeToPlayer(this);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // did player exit shopkeeper's range
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInRange", false);
            playerInput.AddShopKeeperInRangeToPlayer(null);
        }

    }
}
