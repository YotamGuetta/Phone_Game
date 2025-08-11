using System;
using UnityEngine;

public class SkillAnimationManager : MonoBehaviour
{
    public Action OnAnimationEnded;
    public Action OnAnimationApex;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private SkillAbillityExecutioner thisSkillAbillityExecutioner;
    private SkillAbillityExecutioner activatingExecutioner;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        thisSkillAbillityExecutioner = GetComponentInParent<SkillAbillityExecutioner>();
    }

    public void StartAnimation(string animationName, SkillAbillityExecutioner activatingExecutioner)
    {
        this.activatingExecutioner = activatingExecutioner;
        spriteRenderer.enabled = true;
        animator.Play(animationName, 0, 0f);
        animator.speed = PlayerStatsManager.Instance.SkillSpeed;
    }
    public void EndAnimation()
    {
        if (activatingExecutioner != null)
        {

            OnAnimationEnded?.Invoke();
            activatingExecutioner.TurnColliderOnOrOff(false);
            activatingExecutioner = null;
            
        }
        animator.Play("Idle");
        spriteRenderer.enabled = false;
    }
    public void AnimationApex()
    {

        if (activatingExecutioner == thisSkillAbillityExecutioner)
        {
            OnAnimationApex?.Invoke();
        }
    }
}
