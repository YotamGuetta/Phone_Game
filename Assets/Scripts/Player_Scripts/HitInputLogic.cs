using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;


public class HitInputLogic : MonoBehaviour
{
    public FixedJoystick joystick;

    private eightDirection lastDirectionChange;
    private PlayerCombat playerCombat;
    [SerializeField] private SkillAbilityManager skillAbilityManager;
    public eightDirection LastDirectionChange   // get property
    {
        get { return lastDirectionChange; }
    }

    List<eightDirection> inputsQueue;
    public event Action CheckButtonHit;
    public event Action CheckButtonRelease;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputsQueue = new List<eightDirection>();
        joystick.JoystickReleased += OnJoystickLetGo;
        playerCombat = GetComponent<PlayerCombat>();
    }
    // Update is called once per frame
    void Update()
    {
        eightDirection newDirection = joystick.EnumDirection;
        if (lastDirectionChange != newDirection) {
            inputsQueue.Add(newDirection);
            //Debug.Log("from :" + lastDirectionChange + " to: " + newDirection);
            lastDirectionChange = newDirection;
            CheckButtonHit?.Invoke();
        }
        
    }
    private void OnJoystickLetGo()
    {

        //foreach (eightDirection direction in inputsQueue)

        checkActionByInputs();

        inputsQueue.Clear();
        lastDirectionChange = eightDirection.center;
        CheckButtonRelease?.Invoke();
    }
    private eightDirection checkActionByInputs() {
        Debug.Log("Checking hit input");
        eightDirection eDirection;
        if ((eDirection = checkJab()) == eightDirection.center)
        {
            if ((eDirection = checkFullCircle()) == eightDirection.center)
            {
                if ((eDirection = checkHalfCircle()) == eightDirection.center)
                {
                    if ((eDirection = checkReverseHalfCircle()) == eightDirection.center)
                    {
                        if ((eDirection = checkArc()) == eightDirection.center)
                        {
                            if ((eDirection = checkReverseArc()) == eightDirection.center)
                            {
                                Debug.Log("is jibrish");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //playerCombat.Attack(eDirection);
            skillAbilityManager.ActivateSkill(eDirection);
        }
        return eDirection;
    }
    private eightDirection checkJab() {
        if (inputsQueue.Count == 1) {
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
    private eightDirection checkArc()
    {
        return CheckPattern(3, true, "arc");
    }

    private eightDirection checkReverseArc()
    {
        return CheckPattern(3, false, "reverse arc");
    }

    private eightDirection checkHalfCircle()
    {
        return CheckPattern(5, true, "half circle");
    }

    private eightDirection checkReverseHalfCircle()
    {
        return CheckPattern(5, false, "reverse half circle");
    }
    private eightDirection checkFullCircle()
    {
        eightDirection direction =  CheckPattern(7, true, "full circle");
        if (direction != eightDirection.center) 
        {
            return direction;
        }
        return CheckPattern(7, false, "full circle");
    }
}
