
using System.Collections.Generic;
using UnityEngine;
using System;



public class SkillAbilityManager : MonoBehaviour
{

    public static Action<skillType> SkillActivated;
    public static Action SkillFinished;

    public static Action RunAftherInitialize;

    [SerializeField] private SkillsDictionary typeToSkill;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private LayerMask enemyLayer;
    private PlayerInteractions playerInteractions;
    public GameObject skillExecutioner;

    //private float skillTimer = 0;
    //private bool skillOnCooldown = false;

    private UnitController unitController;
    Dictionary<skillType, SkillAbillityExecutioner> skillsToTypeDictionary;

    private int activeAnimationsCount = 0;


    private void Start()
    {
        playerInteractions = GetComponentInParent<PlayerInteractions>();
        unitController = GetComponentInParent<UnitController>();
        skillsToTypeDictionary = InitializeDictionary();
        RunAftherInitialize?.Invoke();
    }

    private Dictionary<skillType, SkillAbillityExecutioner> InitializeDictionary()
    {
        Dictionary<skillType, SkillAbillityExecutioner> skillsDictionaryItems = new Dictionary<skillType, SkillAbillityExecutioner>();
        bool skillfinishedAnimationAssighned = false;

        //Destroy exsisting skills
        foreach (Transform ChildTransform in this.transform)
        {
            Destroy(ChildTransform.gameObject);
        }

        //create all skills and add them to the dictionary
        foreach (var item in typeToSkill.skillsItems)
        {
            SkillAbillityExecutioner skillAbillityExecutioner = Instantiate(skillExecutioner, transform).GetComponent<SkillAbillityExecutioner>();
            skillAbillityExecutioner.Initialize(item.skillAbilitySO, unitController, enemyLayer, playerInteractions);

            //Adds the tracker to when the animation of the skill is active / finished
            skillAbillityExecutioner.SkillActivated += () => addAnimationsToActiveCount(item.type);
            //The tracker for the end only needs to check for the end of the animation in the animator once (the animator is shared between all skills) 
            if (!skillfinishedAnimationAssighned)
            {
                skillAbillityExecutioner.SkillFinished += removeAnimationsToActiveCount;
                skillfinishedAnimationAssighned = true;
            }
            skillsDictionaryItems.Add(item.type, skillAbillityExecutioner);

        }

        return skillsDictionaryItems;
    }
    /* // track total cooldown of skills
    private void Update()
    {
        if (skillTimer > 0)
        {
            skillTimer -= Time.deltaTime;
        }
        else 
        {
            if (skillOnCooldown) 
            {
                //SkillFinished?.Invoke();
                skillOnCooldown = false;
            }
        }
    }
    */
    public void ActivateSkill(eightDirection attackDirection, skillType type)
    {
        if (skillsToTypeDictionary.TryGetValue(type, out SkillAbillityExecutioner skillAbillity))
        {
            if (skillAbillity.ActivateSkill(attackDirection))
            {
                /*
                skillTimer = Mathf.Max(skillTimer, skillAbillity.SkillTimer);
                if (!skillOnCooldown)
                {
                    //SkillActivated?.Invoke();
                    skillOnCooldown = true;
                }
                */
            }
        }
        else
        {
            Debug.Log("No Skill " + type.ToString() + " In Dictionary.");
        }
    }
    public float GetASkillCooldownByType(skillType type)
    {
        return skillsToTypeDictionary[type].GetSkillCooldown();
    }
    public Sprite GetASkillIconByType(skillType type)
    {
        return skillsToTypeDictionary[type].GetSkillIcon();
    }
    private void addAnimationsToActiveCount(skillType type)
    {
        activeAnimationsCount++;
        SkillActivated?.Invoke(type);
    }
    private void removeAnimationsToActiveCount()
    {
        activeAnimationsCount--;
        if (activeAnimationsCount == 0)
            SkillFinished?.Invoke();
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