using UnityEngine;

public class HandsMovement : MonoBehaviour
{
    /*
    public FixedJoystick joystick;
    public HitInputLogic hitInputLogic;
    public float movementSpeed;
   // [SerializeField] private float radius = 1f; // Max distance from center (like handleRange)
    //[SerializeField] private float deadZone = 0.1f; // Minimum input threshold
    //[SerializeField] private bool snapTo8Directions = true; // Enable 8-directional snapping
    
    private Vector3 basePosition; // Tracks position without joystick offset
    private bool hasMoved = false; // Tracks if movement has been applied for current input
    private Vector2 lastInputDirection; // Cached input direction
    private Transform parent;
    
    void Start()
    {
        if (joystick == null)
            Debug.LogError("Joystick reference is missing!");

        // Initialize basePosition
        //parent = GetComponentInParent<Transform>();
        //basePosition = Vector2.zero;

        // Subscribe to JoystickReleased event to reset movement state
        joystick.JoystickReleased += OnJoystickReleased;
     //   hitInputLogic.CheckButtonRelease += moveHands;
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        joystick.JoystickReleased -= OnJoystickReleased;
       // hitInputLogic.CheckButtonRelease -= moveHands;

    }
    private void moveHands() {
        transform.position = Vector3.zero;
        //hitInputLogic.
    }
    void Update()
    {
        
        // Update basePosition to follow GameObject's current position (captures external movement)
        basePosition = Vector2.zero;
        
        // Get input from the joystick
        Vector2 input = joystick.Direction;
        float inputMagnitude = input.magnitude;

        // Check if joystick is in dead zone/*
        if (inputMagnitude < deadZone)
        {
            lastInputDirection = Vector2.zero;
           // if (hasMoved)
            //    transform.position = basePosition; // Stay at basePosition if movement triggered
            return;
        }

        // If we've already moved for this input, do nothing until joystick is released
        if (hasMoved)
            return;
        
        // Normalize input for direction calculations
        Vector2 normalizedInput = inputMagnitude > 1f ? input.normalized : input;

        // Snap to 8 directions if enabled
        Vector2 snappedDirection = snapTo8Directions
            ? SnapTo8Directions(normalizedInput)
            : normalizedInput;

        // Apply movement ONCE: move to basePosition + offset
        Vector3 offset = new Vector3(snappedDirection.x, snappedDirection.y, 0) * radius;
        transform.position = basePosition + offset;

        // Mark movement as done
       hasMoved = true;
       lastInputDirection = snappedDirection;
       if (hasMoved && snappedDirection != lastInputDirection)
           hasMoved = false;
        

    }

    // Called when joystick is released
    private void OnJoystickReleased()
    {
        // Reset movement state to allow new movement on next input
        //hasMoved = false;
        //lastInputDirection = Vector2.zero;
        //basePosition = Vector2.zero;
        // Revert to basePosition (remove offset)
        transform.position = Vector2.zero;
    }
    
    // Adapted from your Joystick's SnapTo8Directions
    private Vector2 SnapTo8Directions(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector2.zero;

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        int directionIndex = Mathf.RoundToInt(angle / 45f) % 8;

        switch (directionIndex)
        {
            case 0: return Vector2.right;               // 0° 
            case 1: return new Vector2(1, 1).normalized; // 45° 
            case 2: return Vector2.up;                  // 90° 
            case 3: return new Vector2(-1, 1).normalized; // 135° 
            case 4: return Vector2.left;                // 180° 
            case 5: return new Vector2(-1, -1).normalized; // 225° 
            case 6: return Vector2.down;                // 270° 
            case 7: return new Vector2(1, -1).normalized; // 315° 
            default: return Vector2.zero;
        }
    }
    
    */
}
