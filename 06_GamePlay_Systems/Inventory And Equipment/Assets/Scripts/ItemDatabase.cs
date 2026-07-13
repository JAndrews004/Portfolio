using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Database/Items")]
public class ItemDatabase : ScriptableObject
{
    public List<Items> items;

    public Items GetItemByID(int id)
    {
        return items.Find(item => item.Id == id);
    }
}
