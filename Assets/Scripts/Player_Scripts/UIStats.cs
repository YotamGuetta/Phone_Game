using UnityEngine;
using TMPro;

public class UIStats : MonoBehaviour
{
    [SerializeField] private GameObject[] statsSlots;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        UpdateAllStats();
    }
    private void Update()
    {
        // Toggle the stats menu on and off
        if (Input.GetButtonDown("ToggleStats")) 
        {

            // If alpha = 0: timscale = 0, alpha is 1 and viceversa
            Time.timeScale = canvasGroup.alpha;
            canvasGroup.alpha = (canvasGroup.alpha - 1) * (-1);
            canvasGroup.blocksRaycasts = canvasGroup.alpha == 1;
            canvasGroup.interactable = canvasGroup.alpha == 1;

            UpdateAllStats();

        }
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
