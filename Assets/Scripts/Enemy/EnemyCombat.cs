using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float weaponRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackStunTime;

    
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

    public void Attack() 
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
            //hits[0].GetComponent<HealthPointsTracker>().CurrentHealth -= damage;
        
        }
    }
}
