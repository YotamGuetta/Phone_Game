using TMPro;
using UnityEngine;

public class PlayerHealthPoints : HealthPointsTrackerAbs
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Animator healthTextAnim;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    private void Start()
    {
        updateHealthText();
    }
    public override int CurrentHealth
    {
        get
        {
            return PlayerStatsManager.Instance.currentHealth;
        }
        set
        {
            PlayerStatsManager.Instance.currentHealth = Mathf.Min(value, PlayerStatsManager.Instance.maxHealth);

            updateHealthText();
            if (PlayerStatsManager.Instance.currentHealth <= 0)
            {
                PlayerStatsManager.Instance.currentHealth = 0;
                uniteDied();
            }
        }
    }
    public override int MaxHealth
    {
        get
        {
            return PlayerStatsManager.Instance.maxHealth;
        }
        set
        {
            PlayerStatsManager.Instance.maxHealth = value;
            if (PlayerStatsManager.Instance.currentHealth > PlayerStatsManager.Instance.maxHealth)
            {
                PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
            }
            updateHealthText();
        }
    }
    protected override void uniteDied()
    {
        PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        updateHealthText();
        Debug.Log("Player died");
    }
    private void updateHealthText()
    {
        if (gameObject.tag == "Player")
        {
            healthText.text = "HP: " + PlayerStatsManager.Instance.currentHealth + " / " + PlayerStatsManager.Instance.maxHealth;
            healthTextAnim.Play("HpTextUpdate");
        }
    }
    public void updateHealthText( int maxHealth, int currHealth)
    {
        if (gameObject.tag == "Player")
        {
            healthText.text = "HP: " + maxHealth + " / " + currHealth;
            healthTextAnim.Play("HpTextUpdate");
        }
    }
}
