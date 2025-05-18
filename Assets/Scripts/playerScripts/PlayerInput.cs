using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float DashPower;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
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
        
    }
}
