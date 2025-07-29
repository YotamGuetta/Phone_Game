using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInteractions playerInteractions;

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
    public Vector2 Direction { get { return direction; } set { direction = value; rotateArrow(); } }
    public GameObject EnemyHealthSlider { get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerInteractions = GetComponentInParent<PlayerInteractions>();

        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifeSpawn);
    }
    private void rotateArrow() 
    {
        //Calculate the angle between 2 points in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Turns Euker angles and converts them to quaternion values
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

            HealthPointsTrackerAbs enemyHealth = collision.gameObject.GetComponent<EnemyHealthPoints>();
            enemyHealth.CurrentHealth -= damage;

            if (playerInteractions != null)
            {
                playerInteractions.ShowEnemyHealth(collision.gameObject);
            }

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

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.SetParent(target);
    }

}
