using UnityEngine;
using System;

public class Loot : MonoBehaviour
{
    [SerializeField] private ItemAbs_SO itemSO;
    [SerializeField] private int quantity;
    [SerializeField] private SpriteRenderer sr;
    private Animator anim;

    public bool CanBePickedup = true;
    public static event Action<ItemAbs_SO, int> OnItemLooted;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnValidate()
    {
        if (itemSO == null) 
        {
            return;
        }
        updateApparence();
    }
    public void Initialize(ItemAbs_SO itemSO, int quantity) 
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        CanBePickedup = false;
        updateApparence();
    }

    private void updateApparence() 
    {
        sr.sprite = itemSO.ItemIcon;
        this.name = itemSO.ItemName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CanBePickedup) 
        {
            anim.Play("LootPickup");

            OnItemLooted?.Invoke(itemSO, quantity);

            Destroy(gameObject, 0.5f);

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanBePickedup = true;
        }
    }
}
