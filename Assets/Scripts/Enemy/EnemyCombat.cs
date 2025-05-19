using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform attackPointUp;
    [SerializeField] private Transform attackPointForward;
    [SerializeField] private Transform attackPointDown;
    [SerializeField] private float weaponRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackStunTime;

    private Transform attackPoint;
    /*
    // On Collision with enemy damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthPointsTracker>().CurrentHealth -= damage;
        }
    }
    */
    private void Awake()
    {
        attackPoint = attackPointForward;
    }
    private void OnEnable()
    {
        EnemyStats.OnDamageChanged += setDamage;
        EnemyStats.OnWeaponRangeChanged += setWeaponRange;
        EnemyStats.OnKnockbackForceChanged += setKnockbackForce;
        EnemyStats.OnKnockbackStunTimeChanged += setKnockbackStunTime;
    }
    
    private void OnDisable()
    {
        EnemyStats.OnDamageChanged -= setDamage;
        EnemyStats.OnWeaponRangeChanged -= setWeaponRange;
        EnemyStats.OnKnockbackForceChanged -= setKnockbackForce;
        EnemyStats.OnKnockbackStunTimeChanged -= setKnockbackStunTime;
    }

    private void setDamage(int newVal)
    {
        damage = newVal;
    }

    private void setWeaponRange(float newVal)
    {
        weaponRange = newVal;
    }

    private void setKnockbackForce(float newVal)
    {
        knockbackForce = newVal;
    }

    private void setKnockbackStunTime(float newVal)
    {
        knockbackStunTime = newVal;
    }

    public void AttackUp() {
        attackPoint = attackPointUp;
        Attack();
    }
    public void AttackForward()
    {
        attackPoint = attackPointForward;
        Attack();
    }
    public void AttackDown()
    {
        attackPoint = attackPointDown;
        Attack();
    }

    private void Attack() 
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0) 
        {
            foreach (Collider2D hit in hits) 
            {
                if (hit.tag == "Player")
                {
                    hit.GetComponent<HealthPointsTracker>().CurrentHealth -= damage;
                    hit.GetComponent<PlayerMovement>().Knockedback(transform, knockbackForce, knockbackStunTime);
                    break;
                }
            }
        
        }
    }
}
