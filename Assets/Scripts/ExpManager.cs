using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int currExp;
    [SerializeField] private int expToLevelUp = 10;
    [SerializeField] private float expGrowthMultiplier = 1.2f;

    private Slider expSlider;
    private TMP_Text levelText;

    private void Start()
    {
        expSlider = GetComponent<Slider>();
        levelText = GetComponentInChildren<TMP_Text>();
        UpdateUI();
    }
    private void OnEnable()
    {
        HealthPointsTracker.OnEnemyDefeated += GainExperience;
    }
    private void OnDisable()
    {
        HealthPointsTracker.OnEnemyDefeated -= GainExperience;
    }
    private void GainExperience(int amount) 
    {
        currExp += amount;
        while (currExp >= expToLevelUp) 
        {
            levelUp();
        }
        UpdateUI();
    }
    private void levelUp() 
    {
        level++;
        currExp -= expToLevelUp;
        expToLevelUp = Mathf.RoundToInt(expToLevelUp * expGrowthMultiplier);
    }
    private void UpdateUI() 
    {
        expSlider.maxValue = expToLevelUp;
        levelText.text = "Level: " + level;
        expSlider.value = currExp;
    }
}
