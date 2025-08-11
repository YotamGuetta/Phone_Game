using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipedSkillsPanel : UIPanel
{
    [SerializeField] private SkillAbilityManager playerAbilityManager;
    [SerializeField] private TMP_Dropdown jabsDropDown;
    [SerializeField] private TMP_Dropdown arcsDropDown;
    [SerializeField] private TMP_Dropdown circleDropDown;

    private SkillsDictionary typeToSkill;
    private void Start()
    {
        typeToSkill = playerAbilityManager.ActiveTypeToSkills;
        updateDropBoxes();
    }
    //Updates the skill choices in the dropboxes
    private void updateDropBoxes()
    {
        jabsDropDown.ClearOptions();
        arcsDropDown.ClearOptions();
        circleDropDown.ClearOptions();

        foreach (var skillAndType in typeToSkill.skillsItems)
        {
            addSkillAbilityToDropBox(skillAndType.type, skillAndType.skillAbilitySO);
        }

        refreshDropBoxes();
    }

    public void EquipJabSkill() 
    {
        updateSkill(skillType.Jab);
    }
    public void EquipArcSkill()
    {
        updateSkill(skillType.Arc);
    }
    public void EquipCircleSkill()
    {
        updateSkill(skillType.Circle);
    }
    private void updateSkill(skillType type)
    {
        int selectedSkillIndex;
        string selectedSkillName;
        TMP_Dropdown dropDown;
        switch (type) 
        {
            case skillType.Jab: dropDown = jabsDropDown;
                break;
            case skillType.Arc: dropDown = arcsDropDown;
                break;
            //Defult is circle type
            default: dropDown = circleDropDown;
                break;
        }
        // Gets the selected name of skill in the dropbox
        selectedSkillIndex = dropDown.value;
        selectedSkillName = dropDown.options[selectedSkillIndex].text;

        SkillAbilitySO selectedSkill = FindSkillByName(selectedSkillName);
        if (selectedSkill == null)
        {
            Debug.Log("Skill " + selectedSkillName + " was not found in the skill list");
            return;
        }

        playerAbilityManager.ChangeSkill(type, selectedSkill);
    }
    private SkillAbilitySO FindSkillByName(string skillName) 
    {
        foreach (var item in typeToSkill.skillsItems)
        {
            if (item.skillAbilitySO.name.Equals(skillName)) 
            {
                return item.skillAbilitySO;
            }
        }
        return null;
    }
    public void AddSkillSlot(skillType type,SkillAbilitySO skill)
    {
        SkillsDictionaryItem newItem = new SkillsDictionaryItem();
        newItem.type = type;
        newItem.skillAbilitySO = skill;

        saveSkillInList(newItem);

        addSkillAbilityToDropBox(type, skill);
    }
    //Resizes the list and saves the new skill in it
    private void saveSkillInList(SkillsDictionaryItem newItem) 
    {
        SkillsDictionary newTypeToSkill = new SkillsDictionary();
        SkillsDictionaryItem[] newSkillDictionary = new SkillsDictionaryItem[typeToSkill.skillsItems.Length + 1];
        newTypeToSkill.skillsItems = newSkillDictionary;

        typeToSkill.skillsItems.CopyTo(newSkillDictionary, 0);
        newSkillDictionary[typeToSkill.skillsItems.Length] = newItem;
        
        typeToSkill = newTypeToSkill;
    }
    private void addSkillAbilityToDropBox(skillType type, SkillAbilitySO skill) 
    {
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(skill.name, skill.SkillIcon, Color.white);
        switch (type) 
        {
            case skillType.Jab: jabsDropDown.options.Add(option);
                break;
            case skillType.Arc: arcsDropDown.options.Add(option);
                break;
            case skillType.Circle: circleDropDown.options.Add(option);
                break;
        }
        refreshDropBoxes();
    }
    private void refreshDropBoxes() 
    {
        jabsDropDown.RefreshShownValue();
        arcsDropDown.RefreshShownValue();
        circleDropDown.RefreshShownValue();
    }
}
