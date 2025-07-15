using System;
using UnityEngine;

public class SkillAnimationManager : MonoBehaviour
{
    public static Action OnAnimationEnded;
    public static Action OnAnimationApex;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();  
    }

    public void StartAnimation(string animationName)
    {
        spriteRenderer.enabled = true;
        animator.Play(animationName, 0, 0f);
        animator.speed = PlayerStatsManager.Instance.SkillSpeed;
    }
    public void EndAnimation()
    {
        animator.Play("Idle");
        spriteRenderer.enabled = false;
        OnAnimationEnded?.Invoke();
    }
    public void AnimationApex() 
    {
        OnAnimationApex?.Invoke();
    }
}
