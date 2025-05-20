using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    [SerializeField] private SkillSlot[] skillSlots;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private int availablePoints;


    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += handleAbilityPointSpent;
        SkillSlot.OnSkillMaxed += handleSkillMaxed;
        ExpManager.OnLevelUp += UpdateAbilityPoints;

    }
    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= handleAbilityPointSpent;
        SkillSlot.OnSkillMaxed -= handleSkillMaxed;
        ExpManager.OnLevelUp -= UpdateAbilityPoints;

    }

    private void Start()
    {
        foreach (SkillSlot slot in skillSlots) 
        {
            slot.ButtonOnClick.AddListener(() => checkAvailablePoints(slot));
        }
        UpdateAbilityPoints(0);
    }

    private void checkAvailablePoints(SkillSlot slot) 
    {
        if (availablePoints > 0) 
        {
            slot.TryUpgradeSkill();
        }
    }

    public void UpdateAbilityPoints(int amount) 
    {
        availablePoints += amount;
        pointsText.text = "Points: " + availablePoints;
    }

    private void handleAbilityPointSpent(SkillSlot skillSlot) 
    {
        if (availablePoints > 0) 
        {
            UpdateAbilityPoints(-1);
        }
    }
    private void handleSkillMaxed(SkillSlot skillSlot)
    {
        foreach (SkillSlot slot in skillSlots) 
        {
            if (slot.CanUnlockSkill())
            {
                slot.Unlock();
            }
        
        }
    }

}
