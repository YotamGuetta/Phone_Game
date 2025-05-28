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

    public void ShowItemInfo(ItemAbs_SO itemSO) 
    {
        infoPanel.alpha = 1;

        itemNameText.text = itemSO.ItemName;
        itemDescriptionText.text = itemSO.ItemDescription;

        List<string> stats = new List<string>();

        foreach (var item in itemSO.GetItemStats())
        {
            stats.Add(item.ToString());
        }
        

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
