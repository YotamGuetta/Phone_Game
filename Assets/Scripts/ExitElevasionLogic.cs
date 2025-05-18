using UnityEngine;

public class ExitElevasionLogic : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                mountain.enabled = true;
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = false;
            }
            //collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
            collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
        }

    }
}
