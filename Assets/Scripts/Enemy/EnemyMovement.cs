using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private int facingDirection = -1;
    private EnemyState currEnemyState;
    private Rigidbody2D rb;
    private Animator anim;
    private float attackCooldownTimer;

    [SerializeField] private Transform player;
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerDetectionRange = 5;
    [SerializeField] private float speed;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackCooldown = 2;
    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        changeState(EnemyState.Idle);
    }
    private void OnEnable()
    {
        EnemyStats.OnMovementSpeedChanged += setSpeed;
    }

    private void OnDisable()
    {
        EnemyStats.OnMovementSpeedChanged -= setSpeed;
    }

    private void setSpeed(float newVal)
    {
        speed = newVal;
    }

    private void Update()
    {
        if (currEnemyState != EnemyState.Knockedback)
        {
            checkForPlayer();

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            if (currEnemyState == EnemyState.Chasing)
            {
                chase();
            }
            else if (currEnemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;

            }
        }
    }

    private void chase()
    {

        if (player.position.x > transform.position.x && facingDirection == -1 ||
             player.position.x < transform.position.x && facingDirection == 1)
        {
            flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
    private void checkForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer);
        if (hits.Length > 0)
        {
            player = hits[0].transform;

            //if player is in attack range and attack cooldown is ready => attack player
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                changeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.transform.position) > attackRange && currEnemyState != EnemyState.Attacking)
            {
                changeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            changeState(EnemyState.Idle);
        }
    }
    private void flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void changeState(EnemyState enemyState) 
    {
        switch (currEnemyState) 
        {
            case EnemyState.Chasing: anim.SetBool("isChasing", false);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", false);
                break;
            default:
                 anim.SetBool("isIdle", false);
                break;
        }
        currEnemyState = enemyState;
        switch (currEnemyState)
        {
            case EnemyState.Chasing:
                anim.SetBool("isChasing", true);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", true);
                break;
            default:
                anim.SetBool("isIdle", true);
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectionRange);
    }
}

public enum EnemyState 
{
    Idle,
    Chasing,
    Attacking,
    Knockedback
}