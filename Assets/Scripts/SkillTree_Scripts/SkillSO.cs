using UnityEngine;

// Skill Scriptable Object
[CreateAssetMenu(fileName ="NewSkill", menuName = "SkillTree/Skill")]
public class SkillSO : ScriptableObject
{
    

    [SerializeField] private string skillName;
    [SerializeField] private int maxLevel;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private bool isActiveSkill;
    [SerializeField] private SkillAbilitySO skill;
    [SerializeField] private skillType typeOfSkill;
    public Sprite SkillIcon { get { return skillIcon; } }

    public int MaxLevel { get { return maxLevel; } }
    public string SkillName { get { return skillName; } }
    public bool IsActiveSkill { get { return isActiveSkill; } }
    public SkillAbilitySO Skill { get { return skill; } }
    public skillType TypeOfSkill { get { return typeOfSkill; } }
}
