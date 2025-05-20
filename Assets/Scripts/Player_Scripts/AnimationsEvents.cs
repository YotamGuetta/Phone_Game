using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private Animator anim;
    private PlayerCombat playerCombat;
    void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        anim.SetBool("IsAttacking", true);
    }
    public void AttackUp()
    {
        anim.SetBool("IsAttackingUp", true);
    }
    public void AttackDown()
    {
        anim.SetBool("IsAttackingDown", true);
    }
    public void FinishAttack()
    {
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsAttackingUp", false);
        anim.SetBool("IsAttackingDown", false);
    }
    public void TriggerDealDamage() 
    {
        playerCombat.DealDamage();
    }
    private void OnEnable()
    {
        playerCombat.AttackForwardTrigger += Attack;
        playerCombat.AttackUpTrigger += AttackUp;
        playerCombat.AttackDownTrigger += AttackDown;
    }
    private void OnDisable()
    {
        playerCombat.AttackForwardTrigger -= Attack;
        playerCombat.AttackUpTrigger -= AttackUp;
        playerCombat.AttackDownTrigger -= AttackDown;
    }

}
