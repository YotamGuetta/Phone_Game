using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private int damage;
    [SerializeField] private float weaponRange;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackStunTime;

    [Header("Movement Stats")]
    [SerializeField] private float movementSpeed;

    [Header("Unique Stats")]
    [SerializeField] private string enemyName;

    private void Start()
    {
        OnDamageChanged(damage);
        OnWeaponRangeChanged(weaponRange);
        OnKnockbackStunTimeChanged(knockbackStunTime);
        OnKnockbackForceChanged(knockbackForce);
        OnMovementSpeedChanged(movementSpeed);
    }

    public string EnemyName { get => enemyName; }

    public delegate void DamageChanged(int newDamge);
    public static event DamageChanged OnDamageChanged;
    public int Damage
    {

        get
        {
            return damage;
        }
        set
        {
            damage = value;
            OnDamageChanged(value);
        }
    }

    public delegate void WeaponRangeChanged(float newWeaponRange);
    public static event WeaponRangeChanged OnWeaponRangeChanged;
    public float WeaponRange
    {

        get
        {
            return weaponRange;
        }
        set
        {
            weaponRange = value;
            OnWeaponRangeChanged(value);
        }
    }

    public delegate void KnockbackForceChanged(float newKnockbackForce);
    public static event KnockbackForceChanged OnKnockbackForceChanged;
    public float KnockbackForce
    {

        get
        {
            return knockbackForce;
        }
        set
        {
            knockbackForce = value;
            OnKnockbackForceChanged(value);
        }
    }

    public delegate void KnockbackStunTimeChanged(float newKnockbackStunTime);
    public static event KnockbackStunTimeChanged OnKnockbackStunTimeChanged;
    public float KnockbackStunTime
    {

        get
        {
            return knockbackStunTime;
        }
        set
        {
            knockbackStunTime = value;
            OnKnockbackStunTimeChanged(value);
        }
    }

    public delegate void MovementSpeedChanged(float newMovementSpeed);
    public static event MovementSpeedChanged OnMovementSpeedChanged;
    public float MovementSpeed
    {

        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
            OnMovementSpeedChanged(value);
        }
    }

}
