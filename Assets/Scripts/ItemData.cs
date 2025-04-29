using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : BaseItem
{
    public float BaseLevel;
    public List<ItemTemplateStats> ItemStats;
    public GameObject ItemPrefab;
}