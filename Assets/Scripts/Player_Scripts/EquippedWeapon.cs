using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{
    [SerializeField] SkillAbilityManager playerSkillAbilityManager;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        // Ignores parameter
        playerSkillAbilityManager.SkillActivated += (skillType _) => hideWeapon();
        playerSkillAbilityManager.SkillFinished += showWeapon;
    }
    private void OnDisable()
    {
        playerSkillAbilityManager.SkillActivated -= (skillType _) => hideWeapon();
        playerSkillAbilityManager.SkillFinished -= showWeapon;
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
