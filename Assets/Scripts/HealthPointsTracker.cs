using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthPointsTracker : MonoBehaviour
{
    public delegate void EnemyDefeated(int exp);
    public static event EnemyDefeated OnEnemyDefeated;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Animator healthTextAnim;
    [SerializeField] private int expWorth;

    private bool showHealthInSlider = false;
    private Slider slider;
    private CanvasGroup canvasGroup;
    private void OnEnable()
    {
        if (gameObject.tag == "Player")
        {
            PlayerStatsManager.OnMaxHealthchanged += addMaxHealth;
        }
    }
    private void OnDisable()
    {
        if (gameObject.tag == "Player")
        {
            PlayerStatsManager.OnMaxHealthchanged -= addMaxHealth;
        }
    }
    private void addMaxHealth(int amount) 
    {
        maxHealth += amount;
        currentHealth += amount;
        updateHealthText();
    }
    public int CurrentHealth
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
            updateHealthText();
            if (currentHealth <= 0) {
                currentHealth = 0;
                uniteDied();
            }
            if (showHealthInSlider) 
            {
                slider.value = currentHealth;
            }
        }
    }
    public int MaxHealth
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

            updateHealthText();
        }
    }
    private void Start()
    {
        updateHealthText();
        if (gameObject.tag == "Player")
        {
            currentHealth = PlayerStatsManager.Instance.currentHealth;
            maxHealth = PlayerStatsManager.Instance.maxHealth;
        }

    }
    private void updateHealthText()
    {
        if (gameObject.tag == "Player")
        {
            PlayerStatsManager.Instance.currentHealth = currentHealth;
            PlayerStatsManager.Instance.maxHealth = maxHealth;
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
            healthTextAnim.Play("HpTextUpdate");
        }
    }
    private void uniteDied() {
        if(gameObject.tag == "Player")
        {
            currentHealth = maxHealth;
            updateHealthText();
            Debug.Log("Player died");
        }
        else
        {
            if (showHealthInSlider) 
            {
                canvasGroup.alpha = 0;
            }
            showHealthInSlider = false;

            OnEnemyDefeated(expWorth);

            Destroy(gameObject);
        }
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
