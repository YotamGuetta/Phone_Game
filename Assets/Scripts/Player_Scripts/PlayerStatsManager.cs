using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public delegate void Healthchanged(int healthAmount);
    public static event Healthchanged OnMaxHealthchanged;
    public static event Healthchanged OnCurrentHealthchanged;

    public static PlayerStatsManager Instance;

    public UIStats uIStats;

    [Header("Combat Stats")]
    public float weaponRange = 0.7f;
    public int damage = 1;
    public float attackCooldown = 2;
    public float KnockbackDuration = 0.2f;
    public float stunTime = 0.2f;
    public float knockbackForce = 20;

    [Header("Movement Stats")]
    public float movementSpeed;
    public float aimingMovmentPenelty = 0.3f;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    [Header("Visualized Stats")]
    [SerializeField] private Transform attackPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
    public void UpdateMaxHealth(int healthAmount) 
    {
        maxHealth += healthAmount;
        currentHealth = healthAmount;
        OnMaxHealthchanged(healthAmount);
    }
    public void UpdateCurrentHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        currentHealth = Math.Min(currentHealth, maxHealth);
        OnCurrentHealthchanged(healthAmount);
    }
    public void UpdateSpeed(int amount)
    {
        movementSpeed += amount;
        uIStats.UpdateAllStats();
    }
    public void UpdateDamage(int amount)
    {
        damage += amount;
        uIStats.UpdateAllStats();
    }
}
