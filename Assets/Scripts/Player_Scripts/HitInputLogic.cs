using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;


public class HitInputLogic : MonoBehaviour
{
    public event Action CheckButtonHit;
    public event Action CheckButtonRelease;

    public FixedJoystick joystick;

    [SerializeField] private SkillAbilityManager skillAbilityManager;

    List<eightDirection> inputsQueueHitButton;
    List<eightDirection> inputsQueue;

    private eightDirection lastDirectionChange;
    public eightDirection LastDirectionChange
    {
        get { return lastDirectionChange; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputsQueueHitButton = new List<eightDirection>();
        joystick.JoystickReleased += OnJoystickLetGo;
    }
    // Update is called once per frame
    void Update()
    {
        //Stores every movement in the hitbox untill it is released
        eightDirection newDirection = joystick.EnumDirection;
        if (lastDirectionChange != newDirection)
        {
            inputsQueueHitButton.Add(newDirection);
            lastDirectionChange = newDirection;
            CheckButtonHit?.Invoke();
        }

    }
    private void OnJoystickLetGo()
    {
        //Checks what action intended buy the input string
        CheckActionByInputs(inputsQueueHitButton);

        inputsQueueHitButton.Clear();
        lastDirectionChange = eightDirection.center;
        CheckButtonRelease?.Invoke();
    }
    public eightDirection CheckActionByInputs(List<eightDirection> newInputsQueue)
    {
        eightDirection eDirection;
        inputsQueue = newInputsQueue;

        Debug.Log("Checking hit input");

        //Check every input string possibility for the input
        if ((eDirection = checkJab()) == eightDirection.center)
            if ((eDirection = checkFullCircle()) == eightDirection.center)
                if ((eDirection = checkHalfCircle()) == eightDirection.center)
                    if ((eDirection = checkReverseHalfCircle()) == eightDirection.center)
                        if ((eDirection = checkArc()) == eightDirection.center)
                            if ((eDirection = checkReverseArc()) == eightDirection.center)
                                Debug.Log("is jibrish");
                            else
                                skillAbilityManager.ActivateSkill(eDirection, skillType.Arc);
                        else
                            skillAbilityManager.ActivateSkill(eDirection, skillType.Arc);
                    else
                        skillAbilityManager.ActivateSkill(eDirection, skillType.HalfCircle);
                else
                    skillAbilityManager.ActivateSkill(eDirection, skillType.HalfCircle);
            else
                skillAbilityManager.ActivateSkill(eDirection, skillType.Circle);
        else
            skillAbilityManager.ActivateSkill(eDirection, skillType.Jab);
        return eDirection;
    }
    //                 0
    //              0     0
    //            0         *
    //              0     0
    //                 0
    private eightDirection checkJab()
    {
        if (inputsQueue.Count == 1)
        {
            Debug.Log("is jab");
            return inputsQueue[0];
        }
        return eightDirection.center;
    }

    private eightDirection CheckPattern(int length, bool clockwise, string patternName)
    {
        if (inputsQueue.Count < length) return eightDirection.center;

        eightDirection eDirectionPrev = inputsQueue[0];
        for (int i = 1; i < length; i++)
        {
            eightDirection eDirectionNext = inputsQueue[i];

            if (clockwise)
            {
                if (!(eDirectionPrev == eDirectionNext - 1 || (eDirectionPrev == eightDirection.upLeft && eDirectionNext == eightDirection.up)))
                    return eightDirection.center;
            }
            else
            {
                if (!(eDirectionPrev == eDirectionNext + 1 || (eDirectionNext == eightDirection.upLeft && eDirectionPrev == eightDirection.up)))
                    return eightDirection.center;
            }

            eDirectionPrev = eDirectionNext;
        }

        Debug.Log($"is {patternName}");

        // Optionally return center/middle element as the result
        return inputsQueue[length / 2];
    }
    //                 0
    //              0     *   
    //            0         *
    //              0     *
    //                 0
    private eightDirection checkArc()
    {
        return CheckPattern(3, true, "arc");
    }
    //                 0
    //              0     *    
    //            0         * 
    //              0     *
    //                 0
    private eightDirection checkReverseArc()
    {
        return CheckPattern(3, false, "reverse arc");
    }
    //                 *
    //              0     *    
    //            0         * 
    //              0     *
    //                 *
    private eightDirection checkHalfCircle()
    {
        return CheckPattern(5, true, "half circle");
    }
    //                 *
    //              0     *    
    //            0         * 
    //              0     *
    //                 *
    private eightDirection checkReverseHalfCircle()
    {
        return CheckPattern(5, false, "reverse half circle");
    }
    //                 *
    //              *     *    
    //            0         * 
    //              *     *
    //                 *
    private eightDirection checkFullCircle()
    {
        eightDirection direction = CheckPattern(7, true, "full circle");
        if (direction != eightDirection.center)
        {
            return direction;
        }
        return CheckPattern(7, false, "full circle");
    }
}
