using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : UnitController
{
    public FixedJoystick joystick;
    public Animator anim;

    private Vector2 lastDirectionMoved;
    
    private float hInput, vInput;
    private Rigidbody2D rb;
    
    private bool isKnockedback;
    private PlayerInput playerInput;

    private float currSpeed;
    private bool isShooting = false;
    private bool isSkillActive = false;

    public Vector2 LastDirectionMoved { get { return lastDirectionMoved; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastDirectionMoved = Vector2.up;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        currSpeed = PlayerStatsManager.Instance.movementSpeed;
    }
    private void OnEnable()
    {
        SkillAbilityManager.SkillActivated += skillActivated;
        SkillAbilityManager.SkillFinished += skillFinished;
    }
    private void OnDisable()
    {
        SkillAbilityManager.SkillActivated -= skillActivated;
        SkillAbilityManager.SkillFinished -= skillFinished;
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
    private void skillActivated() 
    {
        isSkillActive = true;
    }
    private void skillFinished()
    {
        isSkillActive = false;
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
            hInput < 0 && transform.localScale.x > 0) && !isShooting && !isSkillActive)
            {
                flipPlayerSprite();
            }

            //Save last direction walked for dash
            if (hInput != 0 || vInput != 0)
                lastDirectionMoved = new Vector2(hInput, vInput);

            //Save velocity for animation
            anim.SetFloat("Horizontal", Math.Abs(hInput));
            anim.SetFloat("Vertical", Math.Abs(vInput));

            //           MOVE PLAYER

            //Normalize for accurate diagonal speed.
            //Multiplied by the max value to keep the accurate movement momentum.
            Vector2 velocity = new Vector2(hInput, vInput).normalized
                * Mathf.Max(Mathf.Abs(hInput), Mathf.Abs(vInput))
                * currSpeed;
            //Slower movement when using a skill
            if (isSkillActive) 
            {
                velocity *= PlayerStatsManager.Instance.aimingMovmentPenelty;
            }
            rb.linearVelocity = velocity;
        }
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
