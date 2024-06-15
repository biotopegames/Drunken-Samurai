using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public Item GetItemByID(int id)
    {
        foreach (Item item in items)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
}
