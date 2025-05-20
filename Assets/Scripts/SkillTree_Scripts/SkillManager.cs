using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
    }
    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;

    }

    private void HandleAbilityPointSpent(SkillSlot slot) 
    {
        string skillName = slot.SkillName;

        switch (skillName) 
        {
            case "Max Health Boost":
                PlayerStatsManager.Instance.UpdateMaxHealth(1);
                break;
            default:
                Debug.Log("Unknown SkilL: " + skillName);
                break;
        
        }
    }
}
