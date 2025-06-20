using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float DashPower;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerBow playerBow;
    private SkillAbilityManager skillAbilityManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerBow = GetComponent<PlayerBow>();
        skillAbilityManager = GetComponentInChildren<SkillAbilityManager>();
    }
    public void InputPressed(buttonOutput output) {
        switch (output) {
            case buttonOutput.Dash:
                Vector2 lastDirection = playerMovement.LastDirectionMoved * DashPower;
                Vector3 moveDirection = new Vector3(lastDirection.x, lastDirection.y, 0).normalized;
                transform.position += moveDirection;
                break;
        }
        Debug.Log("player pressed : " + output);
    }
    public void InputHold(buttonOutput output)
    {
        Debug.Log("player Hold : " + output);
    }
    public void CheckForAttackInputs()
    {
        checkCombatInput();
        checkSkillInput();
    }
    private void checkSkillInput() 
    {
        if (skillAbilityManager.enabled == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            skillAbilityManager.ActivateSkill(eightDirection.downLeft);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            skillAbilityManager.ActivateSkill(eightDirection.down);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            skillAbilityManager.ActivateSkill(eightDirection.downRight);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            skillAbilityManager.ActivateSkill(eightDirection.left);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            skillAbilityManager.ActivateSkill(eightDirection.right);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            skillAbilityManager.ActivateSkill(eightDirection.upLeft);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            skillAbilityManager.ActivateSkill(eightDirection.up);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            skillAbilityManager.ActivateSkill(eightDirection.upRight);
        }
    }
    private void checkCombatInput() 
    {
        if (playerCombat.enabled == false)
        {
            return;
        }


        if (Input.GetButtonDown("Slash"))
        {
            playerCombat.Attack(eightDirection.right);
        }
        else if (Input.GetButtonDown("SlashUp"))
        {
            playerCombat.Attack(eightDirection.up);
        }
        else if (Input.GetButtonDown("SlashDown"))
        {
            playerCombat.Attack(eightDirection.down);
        }
        else if (Input.GetButtonDown("SlashBack"))
        {
            playerCombat.Attack(eightDirection.left);
        }
    }
    // Update is called once per frame
    void Update()
    {
        checkForCharecterChange();
        checkIfPlayerShoot();
        checkIfPlayerCastSkill();
    }
    private void checkForCharecterChange()
    {
        if (Input.GetButtonDown("SwitchPlayer"))
        {
            playerCombat.enabled = !playerCombat.enabled;
            playerBow.enabled = !playerBow.enabled;
        }
    }
    private void checkIfPlayerShoot()
    {
        if (Input.GetButtonDown("Fire1") && playerBow.enabled)
        {
            playerBow.Shoot();
        }
    }
    private void checkIfPlayerCastSkill()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //skillAbilityManager.ActivateSkill();
        }
    }
}
