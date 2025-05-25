using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private GameObject enemyHealthSlider;

    private Vector2 aimDirection = Vector2.right;
    private float shootTimer;

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        handleAiming();

        if (Input.GetButtonDown("Fire1") && shootTimer <= 0)
        {
            Short();
        }

    }


    private void handleAiming() 
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal !=0 || vertical != 0) 
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
        }
    }

    public void Short() 
    {
        if (shootCooldown <= 0)
        {
            Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Direction = aimDirection;
            arrow.EnemyHealthSlider = enemyHealthSlider;
            shootTimer = shootCooldown;
        }
    }
}
