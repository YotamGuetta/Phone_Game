using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public Animator anim;

    private Vector2 lastDirectionMoved;
    
    private float hInput, vInput;
    private Rigidbody2D rb;
    
    private int facingDirection = 1;
    private bool isKnockedback;
    private PlayerInput playerInput;

    private float currSpeed;
    private bool isShooting = false;

    public Vector2 LastDirectionMoved { get { return lastDirectionMoved; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastDirectionMoved = Vector2.up;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        currSpeed = PlayerStatsManager.Instance.movementSpeed;
    }
    private void Update()
    {
        playerInput.CheckForAttackInputs();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        playerJoystickMovement();
    }
    public void SetIsPlayerAiming(bool IsShooting) 
    {
        if (IsShooting)
        {
            currSpeed = PlayerStatsManager.Instance.movementSpeed * PlayerStatsManager.Instance.aimingMovmentPenelty;
        }
        else
        {
            currSpeed = PlayerStatsManager.Instance.movementSpeed;
        }
        isShooting = IsShooting;
    }
    private void playerJoystickMovement() {

        //Joystick inputs
        hInput = joystick.Horizontal;
        vInput = joystick.Vertical;

        //Other inputs
        hInput += Input.GetAxis("Horizontal");
        vInput += Input.GetAxis("Vertical");
       
        if (!isKnockedback)
        {
            //flipe sprite if the player is walking the opposite direction he is faceing
            if ((hInput > 0 && transform.localScale.x < 0 ||
            hInput < 0 && transform.localScale.x > 0) && !isShooting)
            {
                flipPlayerSprite();
            }

            //Save last direction walked for dash
            if (hInput != 0 || vInput != 0)
                lastDirectionMoved = new Vector2(hInput, vInput);

            //Save velocity for animation
            anim.SetFloat("Horizontal", Math.Abs(hInput));
            anim.SetFloat("Vertical", Math.Abs(vInput));

            //Move player
            rb.linearVelocity = new Vector2(hInput, vInput) * currSpeed;

            /*
            Vector3 moveDirection = new Vector3(hInput, vInput, 0).normalized * movementSpeed;
            transform.position += moveDirection * Time.deltaTime;
            */
        }

    }
    void flipPlayerSprite() {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void Knockedback(Transform enemy, float knockbackForce, float stunTime) 
    {
        isKnockedback = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(KnockedbackCounter(stunTime));
    }
    IEnumerator KnockedbackCounter(float stunTime) 
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedback = false;
    }
}
