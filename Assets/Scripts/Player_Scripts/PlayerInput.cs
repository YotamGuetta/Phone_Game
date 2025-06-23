using System.Collections.Generic;
using UnityEngine;
 

public class PlayerInput : MonoBehaviour
{
    

    [SerializeField] private float DashPower;
    [SerializeField] private float ComboCooldown;

    private HitInputLogic hitInputLogic;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerBow playerBow;
    private SkillAbilityManager skillAbilityManager;
    private List<eightDirection> inputsQueue;
    private float CurrentCommboCountdown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitInputLogic = GetComponent<HitInputLogic>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerBow = GetComponent<PlayerBow>();
        skillAbilityManager = GetComponentInChildren<SkillAbilityManager>();
        inputsQueue = new List<eightDirection>();
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
        if (CurrentCommboCountdown > 0)
        {
            CurrentCommboCountdown -= Time.deltaTime;
        }
        else
        {
            if (inputsQueue.Count > 0) 
            {
                hitInputLogic.CheckActionByInputs(inputsQueue);
                inputsQueue.Clear();
            }
        }
    }
    private void checkSkillInput() 
    {
        if (skillAbilityManager.enabled == false)
        {
            return;
        }
        for (KeyCode key = KeyCode.Keypad1; key <= KeyCode.Keypad9; key++)
        {
            if (Input.GetKeyDown(key))
            {
                CurrentCommboCountdown = ComboCooldown;
                getInputByKey(key);
                break;
            }
        }
    }
    private void getInputByKey(KeyCode key) 
    {
        if (key == KeyCode.Keypad1)
        {
            inputsQueue.Add(eightDirection.downLeft);
        }
        if (key == KeyCode.Keypad2)
        {
            inputsQueue.Add(eightDirection.down);
        }
        if (key == KeyCode.Keypad3)
        {
            inputsQueue.Add(eightDirection.downRight);
        }
        if (key == KeyCode.Keypad4)
        {
            inputsQueue.Add(eightDirection.left);
        }
        if (key == KeyCode.Keypad6)
        {
            inputsQueue.Add(eightDirection.right);
        }
        if (key == KeyCode.Keypad7)
        {
            inputsQueue.Add(eightDirection.upLeft);
        }
        if (key == KeyCode.Keypad8)
        {
            inputsQueue.Add(eightDirection.up);
        }
        if (key == KeyCode.Keypad9)
        {
            inputsQueue.Add(eightDirection.upRight);
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
