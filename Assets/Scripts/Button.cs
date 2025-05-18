using UnityEngine;
public enum buttonOutput
{
    A,
    B,
    Dash
};
public class Button : MonoBehaviour
{
    
    [SerializeField] private buttonOutput buttonValue;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float holdDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPressed;
    private float timer = 0.0f;
    private int holdDurationInSeconds;


    public void ButtonPressed() {
        playerInput.InputPressed(buttonValue);
    }
    public void ButtonHold()
    {
        playerInput.InputHold(buttonValue);
    }
    public void OnPointerDown()
    {
        isPressed = true;
        
    }
    public void OnPointerUp()
    {
        isPressed = false;
        timer = 0.0f;
        holdDurationInSeconds = (int)holdDuration;
    }
    void Awake()
    {
        holdDurationInSeconds = (int)holdDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //Update hold time
        if (isPressed)
        {
            timer += Time.deltaTime;
     
            if (timer % 60 > holdDurationInSeconds) {
                holdDurationInSeconds = (int)timer % 60 + 1;
                ButtonHold();
            }
        }
    }
}
