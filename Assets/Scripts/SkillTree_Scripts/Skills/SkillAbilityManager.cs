
using System.Collections.Generic;
using UnityEngine;
using System;



public class SkillAbilityManager : MonoBehaviour
{

    public static Action SkillActivated;
    public static Action SkillFinished;

    [SerializeField] private SkillsDictionary typeToSkill;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private LayerMask enemyLayer;
    private PlayerInteractions playerInteractions;
    public GameObject skillExecutioner;

    private float skillTimer = 0;
    private UnitController unitController;
    Dictionary<skillType, SkillAbillityExecutioner> skillsToTypeDictionary;
    private bool skillIsActive = false;

    private void Start()
    {
        playerInteractions = GetComponentInParent<PlayerInteractions>();
        unitController = GetComponentInParent<UnitController>();
        skillsToTypeDictionary = InitializeDictionary();
    }
    private Dictionary<skillType, SkillAbillityExecutioner> InitializeDictionary()
    {
        Dictionary<skillType, SkillAbillityExecutioner> skillsDictionaryItems = new Dictionary<skillType, SkillAbillityExecutioner>();

        foreach (var item in typeToSkill.skillsItems)
        {
            SkillAbillityExecutioner skillAbillityExecutioner = Instantiate(skillExecutioner,transform).GetComponent<SkillAbillityExecutioner>();
            skillAbillityExecutioner.Initialize(item.skillAbilitySO, unitController, enemyLayer, playerInteractions);
            skillsDictionaryItems.Add(item.type, skillAbillityExecutioner);
        }

        return skillsDictionaryItems;
    }
    private void Update()
    {
        if (skillTimer > 0)
        {
            skillTimer -= Time.deltaTime;
        }
        else 
        {
            if (skillIsActive) 
            {
                SkillFinished?.Invoke();
                skillIsActive = false;
            }
        }
    }

    public void ActivateSkill(eightDirection attackDirection, skillType type)
    {
        if (skillsToTypeDictionary.TryGetValue(type, out SkillAbillityExecutioner skillAbillity))
        {
            if (skillAbillity.ActivateSkill(attackDirection)) 
            {
                skillTimer = Mathf.Max(skillTimer, skillAbillity.SkillTimer);
                if (!skillIsActive)
                {
                    SkillActivated?.Invoke();
                    skillIsActive = true;
                }
            }
        }
        else
        {
            Debug.Log("No Skill " + type.ToString() + " In Dictionary.");
        }
    }
}
public enum skillType 
{
    Jab,
    Arc,
    HalfCircle,
    Circle
}

[Serializable]
public class SkillsDictionary
{
    [SerializeField]
    public SkillsDictionaryItem[] skillsItems;

}

[Serializable]
public class SkillsDictionaryItem
{
    [SerializeField]
    public skillType type;
    [SerializeField]
    public SkillAbilitySO skillAbilitySO;
}