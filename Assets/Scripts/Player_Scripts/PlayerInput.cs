using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private float DashPower;
    [SerializeField] private float ComboCooldown;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private SkillTreeManager skillTreeManager;
    [SerializeField] private StatsPanelManager statsPanelManager;
    [SerializeField] private EquipedSkillsPanel equipedSkillsPanel;
    private ShopKeeper shopKeeper;


    private HitInputLogic hitInputLogic;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerBow playerBow;
    private SkillAbilityManager skillAbilityManager;
    private List<eightDirection> inputsQueue;
    private float CurrentCommboCountdown = 0;

    private bool DisablePanels = false;
    private UIPanel openedLeftPanel = null;
    private UIPanel openedRightPanel = null;

    enum panelSide
    {
        Left,
        Right
    }

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

    /// <summary>
    /// Updates the shopkeeper that is in range. If there isn't one enter null.
    /// </summary>
    /// <param name="closeShopKeeper"> The shopkeeper who is in range</param>
    public void AddShopKeeperInRangeToPlayer(ShopKeeper closeShopKeeper)
    {
        shopKeeper = closeShopKeeper;
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
        checkIfPlayerPaused();
        checkIfShopOpened();
        checkIfPanelOpened();

    }
    private void checkIfShopOpened() 
    {
        if (shopKeeper == null) 
        {
            return;
        }
        if (Input.GetButtonDown("Interact"))
        {
            closeRightPanel();
            shopKeeper.ToggleShopPanel();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            shopKeeper.CloseShopPanel();
        }
        DisablePanels = shopKeeper.IsShopOpen;
    }
    private void checkIfPanelOpened()
    {
        panelSide side;
        if (DisablePanels) 
        {
            return;
        }

        //Left Panels
        side = panelSide.Left;
        if (Input.GetButtonDown("OpenInventory"))
        {
            togglePanel(inventoryManager, side);
        }
        if (Input.GetButtonDown("OpenSkillsAssigner")) 
        {
            togglePanel(equipedSkillsPanel, side);
        }
        //Right Panels
        side = panelSide.Right;
        if (Input.GetButtonDown("ToggleSkills")) 
        {
            togglePanel(skillTreeManager, side);
        }
        if (Input.GetButtonDown("ToggleStats"))
        {
            togglePanel(statsPanelManager, side);
        }
    }
    /// <summary>
    /// Toggle between show and hide panel
    /// </summary>
    /// <param name="panel">The panel</param>
    /// <param name="side">The side of the panel in the UI</param>
    private void togglePanel(UIPanel panel, panelSide side) 
    {
        if (side == panelSide.Left)
        {
            //Closes a pannel on the left side of the UI if there is an open one.
            if (!panel.IsPanelOpen())
            {
                closeLeftPanel();
            }

            //Toggle the Panel off/on.
            panel.TogglePanel();

            //Set as the active panel on the left.
            if (panel.IsPanelOpen())
            {
                openedLeftPanel = panel;
            }
        }
        else 
        {
            //Closes a pannel on the right side of the UI if there is an open one.
            if (!panel.IsPanelOpen())
            {
                closeRightPanel();
            }

            //Toggle the Panel off/on.
            panel.TogglePanel();

            //Set as the active panel on the right.
            if (panel.IsPanelOpen())
            {
                openedRightPanel = panel;
            }
        }
    }
    private bool closeLeftPanel() 
    {
        bool wasAPanelClosed = false;
        if (openedLeftPanel != null && openedLeftPanel.IsPanelOpen())
        {
            openedLeftPanel.TogglePanel();
            wasAPanelClosed = true;
        }
        openedLeftPanel = null;
        return wasAPanelClosed;
    }
    private bool closeRightPanel()
    {
        bool wasAPanelClosed = false;
        if (openedRightPanel != null && openedRightPanel.IsPanelOpen())
        {
            openedRightPanel.TogglePanel();
            wasAPanelClosed =  true;
        }
        openedRightPanel = null;
        return wasAPanelClosed;
    }
    /// <summary>
    /// searchs if panels in the UI are open and closes them.
    /// </summary>
    /// <returns>If panels were found and closed</returns>
    private bool closeAPanelIfPossible() 
    {
        return closeLeftPanel() || closeRightPanel();
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
        if (Input.GetButtonDown("Fire2") && playerBow.enabled)
        {
            playerBow.Shoot();
        }
    }
    private void checkIfPlayerPaused()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Closes Open panels if there are ones, else opens the menu.
            if (!closeAPanelIfPossible() && !DisablePanels)
            {
                SceneManager.LoadScene(sceneName: "MainMenu");
            }
        }
    }
}
