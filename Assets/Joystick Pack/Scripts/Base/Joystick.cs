﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum eightDirection
{
    center,
    up, upRight, right, downRight, down, downLeft, left, upLeft
}

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
   
    public eightDirection EnumDirection { get { return enumDirection; } }
    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
    public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;
    private eightDirection enumDirection = eightDirection.center;
    public event Action JoystickReleased;

    protected virtual void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    private Vector2 SnapTo8Directions(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector2.zero;
        
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        int directionIndex = Mathf.RoundToInt(angle / 45f) % 8;
       
        
        switch (directionIndex)
        {
            case 0:
                enumDirection = eightDirection.right;
                return Vector2.right;             // 0° → 
            case 1:
                enumDirection = eightDirection.upRight;
                return new Vector2(1, 1).normalized;  // 45° ↗
            case 2:
                enumDirection = eightDirection.up;
                return Vector2.up;                // 90° ↑
            case 3:
                enumDirection = eightDirection.upLeft;
                return new Vector2(-1, 1).normalized; // 135° ↖
            case 4:
                enumDirection = eightDirection.left;
                return Vector2.left;              // 180° ←
            case 5:
                enumDirection = eightDirection.downLeft;
                return new Vector2(-1, -1).normalized; // 225° ↙
            case 6:
                enumDirection = eightDirection.down;
                return Vector2.down;              // 270° ↓
            case 7:
                enumDirection = eightDirection.downRight;
                return new Vector2(1, -1).normalized; // 315° ↘
            default:
                enumDirection = eightDirection.center;
                return Vector2.zero;
        }
    }
    public Vector2 From8DirectionsToVector(eightDirection directionIndex)
    {
        switch (directionIndex)
        {
            case eightDirection.right:
                return Vector2.right;             // 0° → 
            case eightDirection.upRight:
                return new Vector2(1, 1).normalized;  // 45° ↗
            case eightDirection.up:
                return Vector2.up;                // 90° ↑
            case eightDirection.upLeft:
                return new Vector2(-1, 1).normalized; // 135° ↖
            case eightDirection.left:
                return Vector2.left;              // 180° ←
            case eightDirection.downLeft:
                return new Vector2(-1, -1).normalized; // 225° ↙
            case eightDirection.down:
                return Vector2.down;              // 270° ↓
            case eightDirection.downRight:
                return new Vector2(1, -1).normalized; // 315° ↘
            default:
                return Vector2.zero;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        FormatInput();
        Vector2 snappedDirection;
        if (input.magnitude > 0.5f)
            snappedDirection = SnapTo8Directions(input.normalized);
        else
        {
            enumDirection = eightDirection.center;
            snappedDirection = Vector2.zero;
        }
        handle.anchoredPosition = snappedDirection * radius * handleRange;

        HandleInput(input.magnitude, snappedDirection, radius, cam);
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == AxisOptions.Vertical)
            input = new Vector2(0f, input.y);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        enumDirection = eightDirection.center;

        JoystickReleased?.Invoke();
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
}

public enum AxisOptions { Both, Horizontal, Vertical }