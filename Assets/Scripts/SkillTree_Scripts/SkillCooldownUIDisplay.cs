using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillCooldownUIDisplay : MonoBehaviour
{
    [SerializeField] private SkillAbilityManager skillAbilityManager;

    [SerializeField] private SkillCooldownSlot jabSkillSlot;
    [SerializeField] private SkillCooldownSlot ArcSkillSlot;
    [SerializeField] private SkillCooldownSlot fullCircleSkillSlot;

    private Dictionary<skillType, SkillCooldownSlot> skillsSlotsToTypeDictionary;

    private void OnEnable()
    {
        SkillAbilityManager.RunAftherInitialize += initializeSkillSlots;
        skillAbilityManager.SkillActivated += SkillUsed;
    }
    private void OnDisable()
    {
        SkillAbilityManager.RunAftherInitialize -= initializeSkillSlots;
        skillAbilityManager.SkillActivated -= SkillUsed;
    }
    private void initializeSkillSlots() 
    {
        skillsSlotsToTypeDictionary = new Dictionary<skillType, SkillCooldownSlot>();
        addSkillToDictionary(skillType.Jab, jabSkillSlot);
        addSkillToDictionary(skillType.Arc, ArcSkillSlot);
        addSkillToDictionary(skillType.Circle, fullCircleSkillSlot);
    }

    private void addSkillToDictionary(skillType type, SkillCooldownSlot skill) 
    {
        skill.SetSkillSlot(skillAbilityManager.GetASkillCooldownByType(type), skillAbilityManager.GetASkillIconByType(type));
        skillsSlotsToTypeDictionary.Add(type, skill);
    }
    private void SkillUsed( skillType type) 
    {
        skillsSlotsToTypeDictionary[type].skillActivated();
    }
}
