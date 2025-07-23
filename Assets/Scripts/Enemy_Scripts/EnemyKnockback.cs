using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;
    private NavMeshAgent agent;
    private BehaviorGraphAgent behaviorGraphAgent;

    private BlackboardVariable<bool> isKnockedBack;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
        agent = GetComponent<NavMeshAgent>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        behaviorGraphAgent.GetVariable<bool>("IsKnockedbacked", out isKnockedBack);
    }

    public void Knockback(Transform forceTransform, float knockbackForce, float knockbackDuration, float stunTime)
    {
        // enemyMovement.changeState(EnemyState.Knockedback);
        enemyMovement.isKnockedBack = true;
        
        isKnockedBack.Value = true;
        StartCoroutine(stunTimer(knockbackDuration, stunTime));
        Vector2 direction = (transform.position - forceTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        enemyMovement.StopMoving();

    }
    IEnumerator stunTimer(float knockbackDuration, float stunTime) {
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemyMovement.isKnockedBack = false;
        isKnockedBack.Value = false;
        // enemyMovement.changeState(EnemyState.Idle);

    }
}
