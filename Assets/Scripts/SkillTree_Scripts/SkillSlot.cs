using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillSlot[] prerequisiteSkillSlots;
    [SerializeField] private SkillSO skillSO;

    [SerializeField] private int currLevel;
    [SerializeField] private bool isUnlocked;

    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text skillLevelText;
    [SerializeField] private UnityEngine.UI.Button skillButton;

    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;

    public bool IsUnlocked { get { return isUnlocked; } }
    public int CurrLevel { get { return currLevel; } }
    public int MaxLevel { get { return skillSO.MaxLevel; } }
    public string SkillName { get { return skillSO.SkillName; } }
    public UnityEngine.UI.Button.ButtonClickedEvent ButtonOnClick { get { return skillButton.onClick; } }
    private void OnValidate()
    {
        if (skillSO != null && skillLevelText != null && skillButton != null) 
        {
            updateUI();
        }
    }

    public void TryUpgradeSkill() 
    {
        if (isUnlocked && currLevel < skillSO.MaxLevel) 
        {
            currLevel++;
            OnAbilityPointSpent?.Invoke(this);

            if (currLevel >= skillSO.MaxLevel) 
            {
                OnSkillMaxed?.Invoke(this);
            }
            updateUI();
        }
    }

    public bool CanUnlockSkill() 
    {
        foreach (SkillSlot slot in prerequisiteSkillSlots) 
        {
            if (!slot.IsUnlocked || slot.currLevel < slot.MaxLevel) 
            {
                return false;
            }
        }

        return true;
    }

    public void Unlock()
    {
        isUnlocked = true;
        updateUI();
    }

    private void updateUI()
    {
        skillIcon.sprite = skillSO.SkillIcon;
        skillButton.interactable = isUnlocked;

        if (isUnlocked)
        {
            skillLevelText.text = currLevel.ToString() + " /" + skillSO.MaxLevel;
            skillIcon.color = Color.white;
        }
        else 
        {
            skillLevelText.text = "Locked";
            skillIcon.color = Color.gray;
        }
    }
}
