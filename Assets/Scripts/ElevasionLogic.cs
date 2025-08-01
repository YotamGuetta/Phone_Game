using UnityEngine;

public class ElevasionLogic : MonoBehaviour
{
    /// Potential Problem: 
    /// unit outside the elevation area that didn't interact with it is blocked by the mountain boarder if another unit interacted with the elevation
    ///
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    [SerializeField] private GameObject arrows;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {

            //Makes the enemy or player able to ignore all colliders when elevating up
            foreach (Collider2D mountain in mountainColliders)
            {
                ignoreAllColliders(collision, mountain, true);

            }

            //Makes the enemy or player unable to move throw the mountain boarder colliders when elevating up
            foreach (Collider2D boundary in boundaryColliders)
            {
                ignoreAllColliders(collision, boundary, false);
                boundary.enabled = true;
            }

            //Makes all the target sprites render on top of the mountain 
            foreach (SpriteRenderer spriteRenderer in collision.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingOrder = 15;
            }

            //Arrows Elevasion
            if (collision.gameObject.tag == "Player")
            {
                arrows.GetComponent<Collider2D>().excludeLayers |= LayerMask.GetMask("Obstacle");
                arrows.GetComponentInChildren<SpriteRenderer>().sortingOrder = 15;
            }
            
        }
    
    }
    public static void ignoreAllColliders(Collider2D collider1, Collider2D collider2, bool ignore)
    {
        if (!collider2.enabled) 
        {
            collider2.enabled = true;
        }
        Physics2D.IgnoreCollision(collider1, collider2, ignore);
        foreach (Collider2D collider in collider1.GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(collider, collider2, ignore);

        }
    }
    private void OnDestroy()
    {
        arrows.GetComponent<Collider2D>().excludeLayers &= ~(LayerMask.GetMask("Obstacle"));
    }
}

