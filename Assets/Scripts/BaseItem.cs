using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseItem", menuName = "Scriptable Objects/BaseItem")]
public class BaseItem : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public Vector2Int Size;
    public Vector2Int SlotSize;
    public ItemType ItemType;
    public ItemSpecific ItemSpecific;
}
