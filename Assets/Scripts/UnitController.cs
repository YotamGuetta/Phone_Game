using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Transform upAttackPoint;
    [SerializeField] private Transform upForwardAttackPoint;
    [SerializeField] private Transform forwardAttackPoint;
    [SerializeField] private Transform downForwardAttackPoint;
    [SerializeField] private Transform downAttackPoint;

    private Transform activeAttackPoint;

    public Vector3 AttackPossition()
    {
        return activeAttackPoint.position;
    }
    public Quaternion AttackRotation()
    {
        return activeAttackPoint.rotation;
    }
    protected void flipPlayerSprite()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void SetAttackDirection(eightDirection attackDirection) 
    {
        setActivaAttackPoint(attackDirection);
        setDirectionFacing(attackDirection);
    }
    private void setActivaAttackPoint(eightDirection attackDirection)
    {
        switch (attackDirection)
        {
            case eightDirection.up:
                activeAttackPoint = upAttackPoint;
                break;
            case eightDirection.down:
                activeAttackPoint = downAttackPoint;
                break;
            case eightDirection.downLeft:
            case eightDirection.downRight:
                activeAttackPoint = downForwardAttackPoint;
                break;
            case eightDirection.upLeft:
            case eightDirection.upRight:
                activeAttackPoint = upForwardAttackPoint;
                break;
            default:
                activeAttackPoint = forwardAttackPoint;
                break;
        }
    }
    //The direction the player and his sprite is facing
    private void setDirectionFacing(eightDirection attackDirection)
    {
        switch (attackDirection)
        {
            case eightDirection.upLeft:
            case eightDirection.left:
            case eightDirection.downLeft:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                break;
            case eightDirection.upRight:
            case eightDirection.right:
            case eightDirection.downRight:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
        }
    }
}
