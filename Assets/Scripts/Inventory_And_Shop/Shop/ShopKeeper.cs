using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public static event Action<ShopManager, bool> OnShopStateChanged;

    public static ShopKeeper currentShopkeeper;

    public Animator anim;
    public CanvasGroup shopCanvasGroup;
    public ShopManager shopManager;

    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private List<ShopItems> shopWeapons;
    [SerializeField] private List<ShopItems> shopArmors;

    [SerializeField] private Camera shopKeeperCam;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -1);

    private bool playerInRange;
    private bool isShopOpen = false;

    private void Update()
    {
        if (playerInRange) 
        {
            if (Input.GetButtonDown("Interact") || (isShopOpen && Input.GetButtonDown("Cancel")))
            {

                Time.timeScale = shopCanvasGroup.alpha;
                shopCanvasGroup.alpha = (shopCanvasGroup.alpha - 1) * (-1);
                isShopOpen = shopCanvasGroup.alpha == 1;
                shopCanvasGroup.blocksRaycasts = isShopOpen;
                shopCanvasGroup.interactable = isShopOpen;

                OnShopStateChanged?.Invoke(shopManager, isShopOpen);
                
                if (isShopOpen)
                {
                    currentShopkeeper = this;
                    shopKeeperCam.transform.position = transform.position + cameraOffset;
                }
                else
                {
                    currentShopkeeper = null;
                }

                shopKeeperCam.gameObject.SetActive(isShopOpen);

                    OpenItemShop();
            }
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
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInRange", true);
            playerInRange = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInRange", false);
            playerInRange = false;

        }

    }
}
