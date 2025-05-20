using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public delegate void MaxHealthchanged(int healthAmount);
    public static event MaxHealthchanged OnMaxHealthchanged;

    public static PlayerStatsManager Instance;

    [Header("Combat Stats")]
    public float weaponRange = 0.7f;
    public int damage = 1;
    public float attackCooldown = 2;
    public float KnockbackDuration = 0.2f;
    public float stunTime = 0.2f;
    public float knockbackForce = 20;

    [Header("Movement Stats")]
    public float movementSpeed;


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
}
