using System;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCombat : MonoBehaviour
{
    private event Action AttackTrigger;

    public event Action AttackUpTrigger;
    public event Action AttackForwardTrigger;
    public event Action AttackDownTrigger;

    [SerializeField] private Transform upAttackPoint;
    [SerializeField] private Transform forwardAttackPoint;
    [SerializeField] private Transform downAttackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject enemyHealthSlider;

    private Transform activeAttackPoint;
    private float attackTimer;
    private GameObject lastEnemyHit;

    private void Awake()
    {
        activeAttackPoint = forwardAttackPoint;
    }

    private void Update()
    {
        if (attackTimer > 0) 
        {
            attackTimer -= Time.deltaTime;
        }
    }
    private void activeAtttackAnimations(eightDirection attackDirection)
    {
        switch (attackDirection)
        {
            case eightDirection.up:
                AttackTrigger = AttackUpTrigger;
                break;
            case eightDirection.down:
                AttackTrigger = AttackDownTrigger;
                break;
            default:
                AttackTrigger = AttackForwardTrigger;
                break;
        }
    }
    //activate attack
    public void Attack(eightDirection attackDirection) 
    {
        if (attackTimer <= 0)
        {
            //use the correct direction the player is attacking
            setActivaAttackPoint(attackDirection);
            activeAtttackAnimations(attackDirection);
            setDirectionFacing(attackDirection);

            //start attack animation
            AttackTrigger?.Invoke();

            //start attack cooldown
            attackTimer = PlayerStatsManager.Instance.attackCooldown;
        }
    }
    private void setActivaAttackPoint(eightDirection attackDirection)
    {
        switch (attackDirection) 
        {
            case eightDirection.up:
                activeAttackPoint = upAttackPoint;
                break;
            case eightDirection.down:
                activeAttackPoint = downAttackPoint;
                break;
            default:
                activeAttackPoint = forwardAttackPoint;
                break;
        }
    }
    private void setDirectionFacing(eightDirection attackDirection)
    {
        switch (attackDirection)
        {
            case eightDirection.upLeft:
            case eightDirection.left:
            case eightDirection.downLeft:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                break;
            case eightDirection.upRight:
            case eightDirection.right:
            case eightDirection.downRight:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
        }
    }
    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(activeAttackPoint.position, PlayerStatsManager.Instance.weaponRange, enemyLayer);
        if (enemies.Length > 0)
        {
            //Add new enemy to show his health
            if (lastEnemyHit != enemies[0])
            {
                if (lastEnemyHit != null) 
                {
                    lastEnemyHit.GetComponent<HealthPointsTracker>().FreeHealthInSlider();
                }
                lastEnemyHit = enemies[0].gameObject;
                lastEnemyHit.GetComponent<HealthPointsTracker>().ShowHealthInSlider(enemyHealthSlider);
            }
            // Deal damage to enemy and aplay stun, knockback
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<HealthPointsTracker>().CurrentHealth -= PlayerStatsManager.Instance.damage;
                enemy.GetComponent<EnemyKnockback>().Knockback(transform, PlayerStatsManager.Instance.knockbackForce, PlayerStatsManager.Instance.KnockbackDuration, PlayerStatsManager.Instance.stunTime);
            }
        }
    }

}
