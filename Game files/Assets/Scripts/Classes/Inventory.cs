using System.Collections.Generic;
using UnityEngine;

public class  Inventory : MonoBehaviour
{
    [SerializeField]
    private int capacity = 15;

    private List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory is full!");
            return false;
        }
        items.Add(item);
        Debug.Log($"Added {item.ItemName} to inventory.");
        return true;
    }

    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }
}
