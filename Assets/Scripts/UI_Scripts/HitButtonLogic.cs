using System;
using UnityEngine;
using UnityEngine.UI;

public class HitButtonLogic : MonoBehaviour
{
    [SerializeField] private HitInputLogic HitInputLogic;
    [SerializeField] private eightDirection ButtonDirection;
    [SerializeField] private Color idleColor = Color.white;
    [SerializeField] private Color pressedOnceColor = Color.yellow;
    [SerializeField] private Color pressedTwiceColor = Color.yellow;
    [SerializeField] private Color OffColor = Color.gray;
    private Image imageRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        imageRenderer = GetComponent<Image>();
        imageRenderer.color = idleColor;
        HitInputLogic.CheckButtonHit += ButtonHit;
        HitInputLogic.CheckButtonRelease += ButtonRelease;
    }
    public void ButtonHit()
    {
        if (ButtonDirection == HitInputLogic.LastDirectionChange)
        {
            if (imageRenderer.color == idleColor)
            {
                imageRenderer.color = pressedOnceColor;
            }
            else if (imageRenderer.color == pressedOnceColor)
            {
                imageRenderer.color = pressedTwiceColor;
            }
        }
    }
    public void ButtonRelease()
    {
        imageRenderer.color = idleColor;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
