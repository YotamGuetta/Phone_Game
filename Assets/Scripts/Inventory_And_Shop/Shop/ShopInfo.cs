using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopInfo : MonoBehaviour
{
    [SerializeField] private CanvasGroup infoPanel;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    [Header ("Stat Fields")]
    [SerializeField] private TMP_Text[] statTexts;

    private RectTransform infoPanelRect;

    private void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemSO itemSO) 
    {
        infoPanel.alpha = 1;

        itemNameText.text = itemSO.ItemName;
        itemDescriptionText.text = itemSO.ItemDescription;

        List<string> stats = new List<string>();
        if (itemSO.maxHealth > 0)
            stats.Add("MaxHealth: " + itemSO.currentHealth.ToString());
        if (itemSO.currentHealth > 0)
            stats.Add("Health: " + itemSO.currentHealth.ToString());
        if (itemSO.damage > 0)
            stats.Add("Damage: " + itemSO.currentHealth.ToString());
        if (itemSO.speed > 0)
            stats.Add("Speed: " + itemSO.currentHealth.ToString());
        if (itemSO.duration > 0)
            stats.Add("Duration: " + itemSO.currentHealth.ToString());

        if (stats.Count <= 0) 
        {
            return;
        }
        for (int i = 0; i < statTexts.Length; i++)
        {
            if (i < stats.Count)
            {
                statTexts[i].text = stats[i];
                statTexts[i].gameObject.SetActive(true);
            }
            else 
            {
                statTexts[i].gameObject.SetActive(false);
            }
        }
    }
    public void HideItemInfo() 
    {
        infoPanel.alpha = 0;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }

    public void FollowMouse() 
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(10, -10, 0);

        infoPanelRect.position = mousePosition + offset;
    }
}
