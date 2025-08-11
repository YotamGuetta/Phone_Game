using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : UnitController
{
    private int facingDirection = -1;
    private NavMeshAgent agent;
    private Transform player;
    private Vector2 spawnPosition;

    [SerializeField] private float speed;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackCooldown = 2;
    [SerializeField] private float patrolRadius = 5;

    public bool isKnockedBack = false;
    public bool reachedPatrolPoint { get; private set; }
    public float AttackCooldown { get { return attackCooldown; } }
    private void Start()
    {
        reachedPatrolPoint = true;
        spawnPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }       
    }
    private void OnEnable()
    {
        EnemyStats.OnMovementSpeedChanged += setSpeed;
    }

    private void OnDisable()
    {
        EnemyStats.OnMovementSpeedChanged -= setSpeed;
    }

    public void StopMoving()
    {
        if(agent != null && agent.isOnNavMesh)
            agent.isStopped = true;
    }
    /// <summary>
    /// Chases the last player the enemy saw
    /// </summary>
    public void Chase() 
    {
        if (player == null)
        {
            return;
        }

        moveEnemyToAPosition(player.position);
    }
    private void moveEnemyToAPosition(Vector2 position) 
    {
        makeEnemyFacePosition(position);

        if (agent != null && agent.isOnNavMesh)
        {
            if (!isKnockedBack)
                agent.isStopped = false;
            agent.SetDestination(position);
        }
    }
    /// <summary>
    /// Set the player to chase and begin chasing
    /// </summary>
    /// <param name="player">The player to chase</param>
    public void Chase(Transform player)
    {
        this.player = player;
        Chase();
    }
    /// <summary>
    /// Checks if the player is in attack range and sends the direction he can be attacked from
    /// </summary>
    /// <param name="enemyAttackDirection">The direction the player can be reached</param>
    /// <returns>Returns true if there is a player in attack range from any direction</returns>
    public bool PlayerInAttackRange(out EnemyAttackDirection enemyAttackDirection)
    {
        enemyAttackDirection = EnemyAttackDirection.AttackingForward;

        if (player == null)
        {
            return false;
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)// && attackCooldownTimer <= 0)
        {

            //If the enemy is mostly horizontal to the player
            if (Mathf.Abs(transform.position.x - player.transform.position.x) >= attackRange / 2)
            {
                makeEnemyFacePosition(player.position);
                enemyAttackDirection = EnemyAttackDirection.AttackingForward;
            }
            //If the player is below the enemy
            else if (transform.position.y > player.transform.position.y)
            {
                enemyAttackDirection = EnemyAttackDirection.AttackingDown;
            }
            else
            {
                enemyAttackDirection = EnemyAttackDirection.AttackingUp;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// Selects a point to let the enemy patrol to.
    /// </summary>
    public void startEnemyPatrol() 
    {
        reachedPatrolPoint = false;
        Vector2 pointToGo = GetRandomPointInCircle(patrolRadius) + spawnPosition;
        //if its a valide point to go to.
        if (NavMesh.SamplePosition(pointToGo, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            moveEnemyToAPosition(pointToGo);
            // A coroutine to check when the enemy has reached the patrol point assigned.
            StartCoroutine(WaitForAgentArrival());
        }
        else 
        {
            Debug.Log("patrol possition out of navMesh area");
            reachedPatrolPoint = false;
        }
    }
    private IEnumerator WaitForAgentArrival()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        while (agent.velocity.sqrMagnitude > 0f)
        {
            yield return null;
        }

        reachedPatrolPoint = true;
    }
    private Vector2 GetRandomPointInCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        return new Vector2(x, y);
    }
    private void flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    private void makeEnemyFacePosition(Vector2 position)
    {

        if (position.x < transform.position.x && facingDirection == -1 ||
                position.x > transform.position.x && facingDirection == 1)
        {
            flip();
        }
    }
    private void setSpeed(float newVal)
    {
        speed = newVal;
    }
    // The patrol area gizmos
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
