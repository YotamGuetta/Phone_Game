using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyHealthPoints : HealthPointsTrackerAbs
{
    public delegate void EnemyDefeated(int exp);
    public static event EnemyDefeated OnEnemyDefeated;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int expWorth;

    private bool showHealthInSlider = false;
    private Slider slider;
    private CanvasGroup canvasGroup;

    public override int CurrentHealth
    {
        get
        {
            return currentHealth;
        }   
        set
        {
            currentHealth = value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (currentHealth <= 0) {
                currentHealth = 0;
                uniteDied();
            }
            if (showHealthInSlider) 
            {
                slider.value = currentHealth;
                if (currentHealth <= 0) 
                {
                    FreeHealthInSlider();
                }
            }
        }
    }
    public override int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            MaxHealth = value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
    protected override void uniteDied()
    {

        if (showHealthInSlider)
        {
            canvasGroup.alpha = 0;
        }
        showHealthInSlider = false;

        OnEnemyDefeated(expWorth);

        Destroy(gameObject);
    }
    public void ShowHealthInSlider(GameObject activeSlider) 
    {
        activeSlider.GetComponentInChildren<TMP_Text>().text = GetComponent<EnemyStats>().EnemyName;
        slider = activeSlider.GetComponent<Slider>();
        canvasGroup = activeSlider.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        showHealthInSlider = true;
    }
    public void FreeHealthInSlider()
    {
        slider = null;
        canvasGroup = null;
        showHealthInSlider = false;
    }
}
