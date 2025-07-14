using UnityEngine;
using TMPro;

public class StatsPanelManager : UIPanel
{
    [SerializeField] private GameObject[] statsSlots;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateAllStats();
    }
    public override void TogglePanel() 
    {
        base.TogglePanel();
        //Updates The stats labels when opening / closing the stats panel
        UpdateAllStats();
    }
    private void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + PlayerStatsManager.Instance.damage;
    }
    private void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + PlayerStatsManager.Instance.movementSpeed;
    }
    private void RemoveExsesSlots() 
    {
        for (int i = 2; i < statsSlots.Length; i++)
        {
            statsSlots[i].SetActive(false);
        }
    }
    public void UpdateAllStats() 
    {
        UpdateDamage();
        UpdateSpeed();
        RemoveExsesSlots();
    }
}
