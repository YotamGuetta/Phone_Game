using UnityEngine;

public class EnemyRangeDetector : MonoBehaviour
{
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerDetectionRange = 5;

    private GameObject player;

    public GameObject checkForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer);

        //If player detected by the enemy
        if (hits.Length > 0)
        {
            player = hits[0].gameObject;
        }
        else
        {
            player = null;
        }
        return player;
    }
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
        }
        else 
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectionRange);
    }
    
}