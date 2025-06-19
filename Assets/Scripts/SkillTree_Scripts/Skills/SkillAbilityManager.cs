
using System.Collections.Generic;
using UnityEngine;


public class SkillAbilityManager : MonoBehaviour
{

    [SerializeField] private SkillAbilitySO skill;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerCombat playerCombat;

    private SkillAnimationManager SkillAnimationManager;
    private float skillTimer = 0;
    private List<GameObject> enemiesIncountered;
    private void Start()
    {
        SkillAnimationManager = GetComponentInChildren<SkillAnimationManager>();
        enemiesIncountered = new List<GameObject>();
    }
    private void OnEnable()
    {
        SkillAnimationManager.OnAnimationEnded += endSkill;
        //SkillAnimationManager.OnAnimationApex += DealDamage;
    }
    private void OnDisable()
    {
        SkillAnimationManager.OnAnimationEnded -= endSkill;
        //SkillAnimationManager.OnAnimationApex -= DealDamage;

    }
    private void Update()
    {
        if (skillTimer > 0) 
        {
            skillTimer -= Time.deltaTime;
        }
    }
    private void endSkill()
    {
        enemiesIncountered.Clear();
        Destroy(GetComponent<Collider2D>());
    }
    public void ActivateSkill() 
    {
        ActivateSkill(skill);
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
            HealthPointsTracker health = collision.GetComponent<HealthPointsTracker>();
            EnemyKnockback knockback = collision.GetComponent<EnemyKnockback>();
            if (health != null)
            {
                if (playerCombat != null)
                {
                    playerCombat.ShowEnemyHealth(collision.gameObject);
                }
                health.CurrentHealth -= skill.Damage;
            }
            if (knockback != null)
            {
                knockback.Knockback(transform, skill.KnockbackForce, skill.KnockbackDuration, skill.StunTime);
            }
        } 
    }
    public void ActivateSkill(SkillAbilitySO newSkill)
    {
        skill = newSkill;
        if (skillTimer <= 0) 
        {
            switch (skill.AreaShape)
            {
                case shape.Cone:
                    skill.CreateConeColliderForSkill(gameObject); break;
                case shape.Square:
                    skill.CreateBoxColliderForSkill(gameObject); break;
                case shape.Circle:
                    skill.CreateCircleColliderForSkill(gameObject); break;
            }
            playAnimation();
            skillTimer = skill.SkillCooldown;
        }
    }
    private void playAnimation()
    {
        if (SkillAnimationManager != null && skill.AnimationClip != null)
        {

            SkillAnimationManager.StartAnimation(skill.AnimationClip.name);

        }
    }
    //for dealing damage once in a single frame
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
                    HealthPointsTracker health = collider.GetComponent<HealthPointsTracker>();
                    EnemyKnockback knockback = collider.GetComponent<EnemyKnockback>();
                    if (health != null)
                    {
                        if (playerCombat != null) 
                        {
                            playerCombat.ShowEnemyHealth(collider.gameObject);
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
    }
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
        Vector3 position = transform.position + new Vector3(skill.Range, 0, 0);
        Gizmos.DrawWireCube(position, new Vector3(skill.Size, transform.localScale.y, 0));
    }
    private void drawGizmosCone()
    {

        Vector3 position = transform.position + new Vector3(skill.Range, 0, 0);
        Vector3 origin = position;
        Vector3 forward = direction; // or transform.up, depending on desired cone direction

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
