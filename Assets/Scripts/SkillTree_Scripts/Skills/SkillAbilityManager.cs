using System.Collections.Generic;
using UnityEngine;
using System;


public class SkillAbilityManager : MonoBehaviour
{

    public Action<skillType> SkillActivated;
    public Action SkillFinished;
    public Action SkillUpdated;

    public static Action RunAftherInitialize;

    [SerializeField] private SkillsDictionary typeToSkill;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private LayerMask enemyLayer;
    private PlayerInteractions playerInteractions;
    public GameObject skillExecutioner;

    private UnitController unitController;
    Dictionary<skillType, SkillAbillityExecutioner> skillsToTypeDictionary;

    private int activeAnimationsCount = 0;

    private int numberOfSkills = 0;

    public SkillsDictionary ActiveTypeToSkills { get { return typeToSkill; } }
    private void Start()
    {
        playerInteractions = GetComponentInParent<PlayerInteractions>();
        unitController = GetComponentInParent<UnitController>();
        skillsToTypeDictionary = InitializeDictionary();
        if (gameObjectIsPlayer())
        {
            RunAftherInitialize?.Invoke();
        }
    }
    private bool gameObjectIsPlayer() 
    {
        return gameObject.tag == "Player";
    }
    private Dictionary<skillType, SkillAbillityExecutioner> InitializeDictionary()
    {
        Dictionary<skillType, SkillAbillityExecutioner> skillsDictionaryItems = new Dictionary<skillType, SkillAbillityExecutioner>();
        numberOfSkills = typeToSkill.skillsItems.Length;

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

            skillsDictionaryItems.Add(item.type, skillAbillityExecutioner);

            if (tag == "Player")
            {
                //Adds the tracker to when the animation of the skill is active / finished
                skillAbillityExecutioner.SkillActivated += () => addAnimationsToActiveCount(item.type);

                //The tracker for the end only needs to check for the end of the animation in the animator once (the animator is shared between all skills) 
                //if (!skillfinishedAnimationAssighned)
                //{
                    skillAbillityExecutioner.SkillFinished += removeAnimationsToActiveCount;
                //    skillfinishedAnimationAssighned = true;
                //}
            }
        }


        return skillsDictionaryItems;
    }

    private bool tryGetRandomSkillType(out skillType type) 
    { 
        int randomNumber = UnityEngine.Random.Range(0, numberOfSkills);
        try
        {
            type = typeToSkill.skillsItems[randomNumber].type;
            return true;
        }
        catch 
        {
            type = skillType.MonsterSkill1;
            return false;
        }
    }
    public void ActivateRandomSkill(eightDirection attackDirection) 
    {
        if (tryGetRandomSkillType(out skillType type))
        {
            if (skillsToTypeDictionary.TryGetValue(type, out SkillAbillityExecutioner skillAbillity))
            {
                skillAbillity.ActivateSkill(attackDirection);
            }
            else
            {
                Debug.Log("No Skill " + type.ToString() + " In Dictionary.");
            }
        }
        else 
        {
            Debug.Log("Failed to get a random skill type");
        }
       
    }
    public void ActivateSkill(eightDirection attackDirection, skillType type)
    {
        if (skillsToTypeDictionary.TryGetValue(type, out SkillAbillityExecutioner skillAbillity))
        {
            skillAbillity.ActivateSkill(attackDirection);
        }
        else 
        {
            Debug.Log("No Skill " + type.ToString() + " In Dictionary.");
        }
    }
    /// <summary>
    /// Changes the equiped skill by the player
    /// </summary>
    /// <param name="type">The type of skill being changed</param>
    /// <param name="newSkill">The new skill being added</param>
    public void ChangeSkill(skillType type, SkillAbilitySO newSkill) 
    {
        skillsToTypeDictionary[type].ChangeSkill(newSkill);
        SkillUpdated?.Invoke();
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
        if (gameObjectIsPlayer())
        {
            SkillActivated?.Invoke(type);
        }
    }
    private void removeAnimationsToActiveCount()
    {
        //Debug.Log(" -1 Active Count ");
        if (activeAnimationsCount == 0) 
        {
            return;
        }
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
    Circle,
    MonsterSkill1,
    MonsterSkill2,
    MonsterSkill3,
    MonsterSkill4
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