using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillAbillityExecutioner: MonoBehaviour
{
    public Action SkillActivated;
    public Action SkillFinished;

    [SerializeField] private SkillAbilitySO skill;
     private LayerMask enemyLayer;
     private PlayerInteractions playerInteractions;

    private SkillAnimationManager skillAnimationManager;
    private float skillTimer = 0;
    private List<GameObject> enemiesIncountered;
    private UnitController unitController;
    private Collider2D thisSkillCollider2D;
    private bool skillISActive = false;
    public float SkillTimer { get { return skill.SkillCooldown; } }
    public void Initialize(SkillAbilitySO skillAbilitySO, UnitController unitController, LayerMask enemyLayer, PlayerInteractions playerInteractions)
    {
        skill = skillAbilitySO;
        this.unitController = unitController;
        //SkillAnimationManager.OnAnimationEnded += endSkill;
        this.enemyLayer = enemyLayer;
        this.playerInteractions = playerInteractions;
    }
    private void Start()
    {
        skillAnimationManager = GetComponentInChildren<SkillAnimationManager>();
        enemiesIncountered = new List<GameObject>();
    }

    private void endSkill()
    {
        //Debug.Log("skill finished");
        skill.skillEnded();
        enemiesIncountered.Clear();
        Destroy(GetComponent<Collider2D>());
        if (skillAnimationManager != null)
        {
            skillAnimationManager.OnAnimationEnded -= SkillFinished;
            skillAnimationManager.OnAnimationEnded -= endSkill;
        }
    }

    //for dealing damage once while collider is active
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //prevents the same enemy to get damaged multible times
        if (enemiesIncountered.Contains(collision.gameObject))
        {
            return;
        }
        enemiesIncountered.Add(collision.gameObject);

        // checks if the object is an enemy
        if (collision.tag == LayerMask.LayerToName((int)Mathf.Log(enemyLayer.value, 2)))
        {
            HealthPointsTrackerAbs health = collision.GetComponent<HealthPointsTrackerAbs>();
            EnemyKnockback knockback = collision.GetComponent<EnemyKnockback>();
            if (health != null)
            {
                if (playerInteractions != null)
                {
                    PlayerInteractions.ShowEnemyHealth(collision.gameObject);
                }
                health.CurrentHealth -= skill.Damage;
            }
            if (knockback != null)
            {
                knockback.Knockback(transform, skill.KnockbackForce, skill.KnockbackDuration, skill.StunTime);
            }
        }
    }
    public bool ActivateSkill(eightDirection attackDirection)
    {
        //Prevents the skill from being activated if it's cooldown isn't ready
        if (skillTimer > 0)
        {
            return false;
        }

        SkillActivated?.Invoke();
        unitController.SetAttackDirection(attackDirection);

        //Makes a 2d collider based on skill shape
        make2DColliderForSkill();

        thisSkillCollider2D = GetComponent<Collider2D>();
        skillAnimationManager.OnAnimationApex += () => TurnColliderOnOrOff(true);

        skillAnimationManager.OnAnimationEnded += SkillFinished;
        skillAnimationManager.OnAnimationEnded += endSkill;

        playAnimation();

        skillTimer = skill.SkillCooldown;
        return true;
    }
    private void make2DColliderForSkill() 
    {

        LayerMask thisLayer  =  unitController.gameObject.layer;
        skill.InstantiateSkill(gameObject, unitController.AttackPossition(), unitController.AttackRotation(), unitController, enemyLayer, thisLayer);
        //TurnColliderOnOrOff(false);
    }
    private void Update()
    {
        if(skillTimer > 0)
            skillTimer -= Time.deltaTime;
    }
    private void playAnimation()
    {
        if (skillAnimationManager != null && skill.AnimationClip != null)
        {
            skillAnimationManager.transform.position = Vector3.right;
            skillAnimationManager.StartAnimation(skill.AnimationClip.name, this);
        }
    }
    public void ChangeSkill(SkillAbilitySO newSkill) 
    {
        skill = newSkill;
    }
    public void TurnColliderOnOrOff(bool turnOn)
    {
        if (thisSkillCollider2D != null)
        {
            thisSkillCollider2D.enabled = turnOn;
        }
    }
    //For dealing damage once in a single frame to every enemy in enemy layer
    /*
    public void DealDamage()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.layerMask = enemyLayer;
            List<Collider2D> results = new List<Collider2D>();
            int numberOfEnemies = collider2D.Overlap(filter, results);
            if (numberOfEnemies > 0)
            {
                foreach (var collider in results)
                {
                    HealthPointsTrackerAbs health = collider.GetComponent<HealthPointsTrackerAbs>();
                    EnemyKnockback knockback = collider.GetComponent<EnemyKnockback>();
                    if (health != null)
                    {
                        if (playerInteractions != null)
                        {
                            playerInteractions.ShowEnemyHealth(collider.gameObject);
                        }
                        health.CurrentHealth -= skill.Damage;
                    }
                    if (knockback != null)
                    {
                        knockback.Knockback(transform, skill.KnockbackForce, skill.KnockbackDuration, skill.StunTime);
                    }
                }
            }
        }
    }*/

    public float GetSkillCooldown() 
    {
        return skill.SkillCooldown;
    }
    public Sprite GetSkillIcon()
    {
        return skill.SkillIcon;
    }

    //For visual presentation in unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        switch (skill.AreaShape)
        {
            case shape.Cone:
                drawGizmosCone(); break;
            case shape.Square:
                drawGizmosSquaree(); break;
            case shape.Circle:
                drawGizmosCircle(); break;

        }
    }
    private void drawGizmosCircle()
    {
        Vector3 position = transform.position + new Vector3(skill.Range, 0, 0);
        Gizmos.DrawWireSphere(position, skill.Size);
    }
    private void drawGizmosSquaree()
    {
        Vector3 position = transform.position + new Vector3(skill.Range * Vector3.right.x, Vector3.right.y, 0);
        Gizmos.DrawWireCube(position, new Vector3(skill.Size, transform.localScale.y, 0));
    }
    private void drawGizmosCone()
    {

        Vector3 position = transform.position + new Vector3(skill.Range, 0, 0);
        Vector3 origin = position;
        Vector3 forward = Vector3.right; // or transform.up, depending on desired cone direction

        // Draw center line
        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, 0, -skill.Angle / 2f) * forward * skill.Size);
        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, 0, skill.Angle / 2f) * forward * skill.Size);

        Vector3 previousPoint = origin + Quaternion.Euler(0, 0, -skill.Angle / 2f) * forward * skill.Size;

        for (int i = 1; i <= skill.Segments; i++)
        {
            float step = skill.Angle / skill.Segments;
            float currentAngle = -skill.Angle / 2f + step * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, 0, currentAngle) * forward * skill.Size;

            // Draw arc segment
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        // Optional: fill in the sides of the cone
        for (int i = 0; i <= skill.Segments; i++)
        {
            float step = skill.Angle / skill.Segments;
            float currentAngle = -skill.Angle / 2f + step * i;
            Vector3 point = origin + Quaternion.Euler(0, 0, currentAngle) * forward * skill.Size;

            Gizmos.DrawLine(origin, point);
        }
    }
}