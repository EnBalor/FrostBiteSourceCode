using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equip,
    Consum,
    Resourece
}

public enum ConsumType
{
    Health,
    Hunger,
    Stamina,
    Frozen
}
[System.Serializable]
public class ItemDataConsum
{
    public ConsumType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consum")]
    public ItemDataConsum[] consumable;

    [Header("Equip")]
    public GameObject equipPrefab;
}
