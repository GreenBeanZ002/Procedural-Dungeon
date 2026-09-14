using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickupTrigger : MonoBehaviour
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            var inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                item.Pickup(inventory);
            }
        }
    }
}