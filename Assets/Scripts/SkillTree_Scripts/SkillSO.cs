using UnityEngine;

// Skill Scriptable Object
[CreateAssetMenu(fileName ="NewSkill", menuName = "SkillTree/Skill")]
public class SkillSO : ScriptableObject
{
    

    [SerializeField] private string skillName;
    [SerializeField] private int maxLevel;
    [SerializeField] private Sprite skillIcon;

    public Sprite SkillIcon { get { return skillIcon; } }

    public int MaxLevel { get { return maxLevel; } }
    public string SkillName { get { return skillName; } }
}
