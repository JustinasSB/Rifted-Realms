using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public Vector2Int Size;
    public ItemType ItemType;
    public GameObject ItemPrefab;
    public Vector2Int SlotSize;
}