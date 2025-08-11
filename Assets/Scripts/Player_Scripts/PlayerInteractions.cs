using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private static GameObject lastEnemyHit;
    private static GameObject enemyHealthSliderContainer;
    [SerializeField] private  GameObject enemyHealthSlider;
    private void Start()
    {
        enemyHealthSliderContainer = enemyHealthSlider;
    }
    public static void ShowEnemyHealth(GameObject enemy)
    {
        if (lastEnemyHit != null)
        {
            lastEnemyHit.GetComponent<EnemyHealthPoints>().FreeHealthInSlider();
        }
        lastEnemyHit = enemy;
        lastEnemyHit.GetComponent<EnemyHealthPoints>().ShowHealthInSlider(enemyHealthSliderContainer);
    }
}
