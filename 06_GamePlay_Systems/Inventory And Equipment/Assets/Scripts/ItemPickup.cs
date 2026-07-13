using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{
    public int itemID; // set this in Inspector
    private ItemDatabase database;

    void Start()
    {
        // Load database (assuming you put it in a "Resources" folder)
        database = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    public Items GetItem()
    {
        return database.GetItemByID(itemID);
    }
}
