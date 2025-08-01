using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO/ New Skill")]
public class SkillAbilitySO : ScriptableObject
{
    [SerializeField] private float size = 3f;
    [SerializeField] private float range = 3f;
    [SerializeField] private int damage;
    [SerializeField] private shape areaShape;
    [SerializeField] private float angle = 90f;
    [SerializeField] private int segments = 10;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private AnimationClip animation;
    [SerializeField] private float skillCooldown = 1;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float stunTime;
    public float Size { get { return size; } }
    public float Range { get { return range; } }
    public int Damage { get { return damage; } }
    public shape AreaShape { get { return areaShape; } }
    public float Angle { get { return angle; } }
    public int Segments { get { return segments; } }
    public float SkillCooldown { get { return skillCooldown; } }
    public AnimationClip AnimationClip { get { return animation; } }
    public float KnockbackForce { get { return knockbackForce; } }
    public float KnockbackDuration { get { return knockbackDuration; } }
    public float StunTime { get { return stunTime; } }
    public Sprite SkillIcon { get { return skillIcon; } }
    public void skillEnded()
    {

    }
    public Collider2D CreateConeColliderForSkill(GameObject obj)
    {
        return CreateConeCollider(obj, size, angle, segments);
    }
    public Collider2D CreateConeCollider(GameObject obj, Vector3 possition, Quaternion rotation)
    {
        Collider2D collider = CreateConeCollider(obj, size, angle, segments);
        obj.transform.rotation = rotation;
        obj.transform.position = possition;
        return collider;
    }
    public Collider2D CreateConeCollider(GameObject obj, float radius, float angle, int segments)
    {
        PolygonCollider2D poly = obj.AddComponent<PolygonCollider2D>();

        poly.isTrigger = true;
        //poly.transform.position = obj.transform.position + new Vector3(range, 0, 0);
        //poly.transform.Rotate(-obj.transform.rotation.eulerAngles);
        poly.offset += new Vector2(range, 0);
        List<Vector2> points = new List<Vector2> { Vector2.zero }; // center point

        float halfAngle = angle / 2f;
        for (int i = 0; i <= segments; i++)
        {
            float theta = Mathf.Deg2Rad * (-halfAngle + (angle / segments) * i);
            Vector2 point = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * radius;
            points.Add(point);
        }

        poly.SetPath(0, points.ToArray());
        return poly;
    }
    public Collider2D CreateBoxColliderForSkill(GameObject obj)
    {
        BoxCollider2D boxCollider = obj.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(size * obj.transform.localScale.x, obj.transform.localScale.y);
        //boxCollider.transform.position = obj.transform.position + new Vector3(range, 0, 0);
        boxCollider.offset += new Vector2(range, 0);
        return boxCollider;
    }
    public Collider2D CreateBoxColliderForSkill(GameObject obj, Vector3 possition, Quaternion rotation)
    {
        BoxCollider2D boxCollider = obj.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(size * obj.transform.localScale.x, obj.transform.localScale.y);
        boxCollider.transform.rotation = rotation;
        boxCollider.transform.position = possition;
        //boxCollider.transform.position = obj.transform.position + new Vector3(range, 0, 0);
        boxCollider.offset += new Vector2(range, 0);
        return boxCollider;
    }
    public Collider2D CreateCircleColliderForSkill(GameObject obj)
    {
        CircleCollider2D circleCollider = obj.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = size * obj.transform.localScale.y;
        //circleCollider.transform.position = obj.transform.position + new Vector3(range, 0, 0);
        circleCollider.offset += new Vector2(range, 0);
        return circleCollider;
    }
}
public enum shape 
{
    Square,
    Circle,
    Cone
}
