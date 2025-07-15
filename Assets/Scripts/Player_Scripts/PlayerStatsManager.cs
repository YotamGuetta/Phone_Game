using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public delegate void Healthchanged(int healthAmount);
    public static event Healthchanged OnMaxHealthchanged;
    public static event Healthchanged OnCurrentHealthchanged;

    public static PlayerStatsManager Instance;

    public StatsPanelManager uIStats;

    //[Header("Combat Stats")]
    public float weaponRange = 0.7f;
    public int damage = 1;
    public float attackCooldown = 2;
    public float KnockbackDuration = 0.2f;
    public float stunTime = 0.2f;
    public float knockbackForce = 20;
    public float SkillSpeed = 1;

    //[Header("Movement Stats")]
    public float movementSpeed;
    public float aimingMovmentPenelty = 0.3f;

    //[Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    //[Header("Visualized Stats")]
    [SerializeField] private Transform attackPoint;

    [SerializeField] private bool activateDynamicHealth;

    public bool ActivateDynamicHealth { get { return activateDynamicHealth; } }
    public readonly float[] R_COMBAT_STATS_DEFULT_VALUES = { 0.7f, 1, 2, 0.2f, 20, 1};
    public readonly float[] R_MOVEMENT_STATS_DEFULT_VALUES = {3, 0.3f};
    public readonly float[] R_HEALTH_STATS_DEFULT_VALUES = { 20, 20 };

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

    public void SetAllCombatStatsToDefultValues()
    {
        try
        {
            weaponRange = R_COMBAT_STATS_DEFULT_VALUES[0];
            damage = (int)R_COMBAT_STATS_DEFULT_VALUES[1];
            attackCooldown = R_COMBAT_STATS_DEFULT_VALUES[2];
            KnockbackDuration = R_COMBAT_STATS_DEFULT_VALUES[3];
            stunTime = R_COMBAT_STATS_DEFULT_VALUES[4];
            knockbackForce = R_COMBAT_STATS_DEFULT_VALUES[5];
            SkillSpeed = R_COMBAT_STATS_DEFULT_VALUES[6];
        }
        catch(ArgumentOutOfRangeException)
        {
            Debug.Log("More combat stats then defult values");
        }
    }

    public void SetAllMovementStatsToDefultValues()
    {
        try
        {
            movementSpeed = R_MOVEMENT_STATS_DEFULT_VALUES[0];
            aimingMovmentPenelty = R_MOVEMENT_STATS_DEFULT_VALUES[1];
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("More movement stats then defult values");
        }
    }

    public void SetAllHealthStatsToDefultValues()
    {
        try
        {

            maxHealth = (int)R_HEALTH_STATS_DEFULT_VALUES[0]; ;
            currentHealth = (int)R_HEALTH_STATS_DEFULT_VALUES[1];
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("More health stats then defult values");
        }
    }
    public void AdjustCurrentHealthChangeToValid() 
    {
        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
    }
    public void AdjustMaxHealthChangeToValid(int oldValue)
    {
        
        currentHealth = Mathf.Max((currentHealth + maxHealth - oldValue), 0);
    }
}
