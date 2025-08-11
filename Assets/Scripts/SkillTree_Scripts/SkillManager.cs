using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private EquipedSkillsPanel EquipedSkillsPanel;
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
            case "Stabber":
                if (!slot.JustUnlockedNewSkill) 
                {
                    PlayerStatsManager.Instance.UpdateDamage(1);
                }
                break;
            default:
                Debug.Log("Unknown SkilL: " + skillName);
                break;
        
        }
        if (slot.JustUnlockedNewSkill) 
        {
            slot.AddNewlyUnlockedSkill(EquipedSkillsPanel);
        }
    }
}
