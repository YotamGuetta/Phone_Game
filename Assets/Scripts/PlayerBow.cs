using System;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public event Action<Vector2> ShootInADirection;
    public static event Action OnEnabled;
    public static event Action OnDisabled;

    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private GameObject enemyHealthSlider;

    private PlayerMovement playerMovement;
    private Vector2 aimDirection = Vector2.right;
    private float shootTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }
    public void Shoot() 
    {
        if (shootTimer <= 0)
        {
            handleAiming();
            playerMovement.SetIsPlayerAiming(true);
            ShootInADirection?.Invoke(aimDirection);
        }
    }
    private void OnEnable()
    {
        OnEnabled?.Invoke();
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
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

    public void GenerateArrow() 
    {
        if (shootTimer <= 0)
        {
            Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Direction = aimDirection;
            arrow.EnemyHealthSlider = enemyHealthSlider;
            shootTimer = shootCooldown;
            playerMovement.SetIsPlayerAiming(false);

            //Ignore the players own arrows
            arrow.GetComponent<Collider2D>().excludeLayers = arrow.GetComponent<Collider2D>().excludeLayers | LayerMask.GetMask("Player");
        }
    }
}
