using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(menuName = "EquipableItems/BaseItem")]
public class Items : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public ItemRarity Rarity;
    public float Attack;
    public float Defense;
    public float Health;
}
