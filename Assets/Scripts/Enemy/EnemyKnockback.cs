using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void Knockback(Transform player, float knockbackForce, float knockbackDuration, float stunTime)
    {
        enemyMovement.changeState(EnemyState.Knockedback);
        StartCoroutine(stunTimer(knockbackDuration, stunTime));
        Vector2 direction = (transform.position - player.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
    }
    IEnumerator stunTimer(float knockbackDuration, float stunTime) {
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemyMovement.changeState(EnemyState.Idle);

    }
}
