using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private GameObject lastEnemyHit;
    [SerializeField] private GameObject enemyHealthSlider;
    public void ShowEnemyHealth(GameObject enemy)
    {
        if (lastEnemyHit != null)
        {
            lastEnemyHit.GetComponent<EnemyHealthPoints>().FreeHealthInSlider();
        }
        lastEnemyHit = enemy;
        lastEnemyHit.GetComponent<EnemyHealthPoints>().ShowHealthInSlider(enemyHealthSlider);
    }
}
