using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public Vector2Int Size;
    public Vector2Int SlotSize;
    public ItemType ItemType;
    public ItemSpecific ItemSpecific;
    public float BaseLevel;
    public List<ItemTemplateStats> ItemStats;
    public GameObject ItemPrefab;
}