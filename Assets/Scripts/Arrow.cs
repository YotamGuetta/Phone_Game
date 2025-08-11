using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float lifeSpawn = 2;
    [SerializeField] private float speed;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask ObstacleLayer;

    private SpriteRenderer sr;
    [SerializeField] private Sprite buriedSprite;

    [SerializeField] private int damage;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;

    public static bool IgnoreObstacles = false;
    public void InitializeArrow(Vector2 direction, LayerMask enemyLayer) 
    {
        this.direction = direction;
        this.enemyLayer = enemyLayer;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifeSpawn);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Binery comparison between the enemy layer and the object hit
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            EnemyKnockback enemyKnockback = collision.gameObject.GetComponent<EnemyKnockback>();
            if (enemyKnockback != null)
            {
                enemyKnockback.Knockback(transform, knockbackForce, knockbackTime, stunTime);
            }

            HealthPointsTrackerAbs enemyHealth = collision.gameObject.GetComponent<HealthPointsTrackerAbs>();

            if (enemyHealth is EnemyHealthPoints)
            {
                PlayerInteractions.ShowEnemyHealth(collision.gameObject);
            }
            enemyHealth.CurrentHealth -= damage;
            AttachToTarget(collision.gameObject.transform);
        }
        else if (!IgnoreObstacles &&(ObstacleLayer.value & (1 << collision.gameObject.layer)) > 0) 
        {
            AttachToTarget(collision.gameObject.transform);
        }
    }
    private void AttachToTarget(Transform target) 
    {
        sr.sprite = buriedSprite;

        //rb.linearVelocity = Vector2.zero;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        Destroy(rb);
        transform.SetParent(target);
    }
}
