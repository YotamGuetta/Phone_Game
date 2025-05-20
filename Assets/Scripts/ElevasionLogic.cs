using UnityEngine;

public class NewEmptyCSharpScript : MonoBehaviour
{
    /// Potential Problem: 
    /// unit outside the elevation area that didn't interact with it is blocked by the mountain boarder if another unit interacted with the elevation
    ///
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                ignoreAllColliders(collision, mountain, true);
                //mountain.enabled = false;
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                ignoreAllColliders(collision, boundary, false);
                boundary.enabled = true;
            }
            //collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
            collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 15;
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
}

