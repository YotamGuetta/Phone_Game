using UnityEngine;

public class ExitElevasionLogic : MonoBehaviour
{

 
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    [SerializeField] private GameObject arrows;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {

            foreach (Collider2D mountain in mountainColliders)
            {
                ElevasionLogic.ignoreAllColliders(collision, mountain,false);

                //mountain.enabled = true;
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                ElevasionLogic.ignoreAllColliders(collision, boundary, true);
                //boundary.enabled = false;
            }
            //collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
            //collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
            foreach (SpriteRenderer spriteRenderer in collision.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingOrder = 5;
            }

            //Arrows Elevasion
            if (collision.gameObject.tag == "Player")
            {
                arrows.GetComponent<Collider2D>().excludeLayers &= ~(LayerMask.GetMask("Obstacle"));
                arrows.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
            }
        }
    }
}
