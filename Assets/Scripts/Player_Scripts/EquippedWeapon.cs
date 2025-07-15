using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        SkillAbilityManager.SkillActivated += hideWeapon;
        SkillAbilityManager.SkillFinished += showWeapon;
    }
    private void OnDisable()
    {
        SkillAbilityManager.SkillActivated -= hideWeapon;
        SkillAbilityManager.SkillFinished -= showWeapon;
    }
    private void showWeapon() 
    {
        spriteRenderer.enabled = true;
    }
    private void hideWeapon()
    {
        spriteRenderer.enabled = false;
    }
}
