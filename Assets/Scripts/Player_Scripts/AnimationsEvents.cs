using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private Animator anim;
    private PlayerCombat playerCombat;
    private PlayerBow playerBow;

    private 
    void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
        playerBow = GetComponentInParent<PlayerBow>();
        anim = GetComponent<Animator>();
    }
    private void ActivateAttackAnimation(eightDirection attackDirection) 
    {       
        switch (attackDirection)
        {
            case eightDirection.up:
                anim.SetFloat("FaceX", 0);
                anim.SetFloat("FaceY", 1);
                break;
            case eightDirection.upRight:
                anim.SetFloat("FaceX", 1);
                anim.SetFloat("FaceY", 1);
                break;
            case eightDirection.right:
                anim.SetFloat("FaceX", 1);
                anim.SetFloat("FaceY", 0);
                break;
            case eightDirection.downRight:
                anim.SetFloat("FaceX", 1);
                anim.SetFloat("FaceY", -1);
                break;
            case eightDirection.down:
                anim.SetFloat("FaceX", 0);
                anim.SetFloat("FaceY", -1);
                break;
            case eightDirection.downLeft:
                anim.SetFloat("FaceX", -1);
                anim.SetFloat("FaceY", -1);
                break;
            case eightDirection.left:
                anim.SetFloat("FaceX", -1);
                anim.SetFloat("FaceY", 0);
                break;
            default:
                anim.SetFloat("FaceX", -1);
                anim.SetFloat("FaceY", 1);
                break;
        }

        anim.SetBool("IsAttacking", true);
    }
    private void ActivateShootingAnimation(Vector2 direction) 
    {
        anim.SetBool("IsShooting", true);
        anim.SetFloat("AimX", direction.x);
        anim.SetFloat("AimY", direction.y);
    }
    private void changeLayer(PlayerAnimationLayers layer) 
    {
        int layerValue = (int)layer;
        for (int i = 0; i < anim.layerCount; i++) 
        {
            if (layerValue == i)
            {
                anim.SetLayerWeight(i, 1);
            }
            else 
            {
                anim.SetLayerWeight(i, 0);
            }
        }
        
    }
    private void changeLayerToSword()
    {
        changeLayer(PlayerAnimationLayers.Sword);
}
    private void changeLayerToBow()
    {
        changeLayer(PlayerAnimationLayers.Bow);
    }
    public void FinishAttack()
    {
        anim.SetBool("IsAttacking", false);

    }
    public void FinishShot()
    {
        anim.SetBool("IsShooting", false);

    }
    public void TriggerDealDamage() 
    {
        playerCombat.DealDamage();
    }
    public void Shoot()
    {
        playerBow.Shoot();
    }
    private void OnEnable()
    {
        playerCombat.AttackInADirection += ActivateAttackAnimation;
        playerBow.ShootInADirection += ActivateShootingAnimation;
        PlayerBow.OnEnabled += changeLayerToBow;
        PlayerBow.OnDisabled += changeLayerToSword;

    }
    private void OnDisable()
    {
        playerCombat.AttackInADirection -= ActivateAttackAnimation;
        playerBow.ShootInADirection -= ActivateShootingAnimation;
        PlayerBow.OnEnabled -= changeLayerToBow;
        PlayerBow.OnDisabled -= changeLayerToSword;
    }

}
public enum PlayerAnimationLayers 
{
    Sword,
    Bow
}