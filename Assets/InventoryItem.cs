using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    public Image itemIcon;
    public ItemData data { get; set; }
    public InventorySlot slot { get; set; }

    void Awake()
    {
        itemIcon = GetComponent<Image>();
    }
    public void Initialize(ItemData item, InventorySlot parent)
    {
        slot = parent;
        parent.Allocated = true;
        slot.Item = this;
        data = item;
        itemIcon.sprite = item.Icon;
        itemIcon.raycastTarget = false;
    }
}