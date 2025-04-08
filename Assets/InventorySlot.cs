using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    public InventoryItem Item { get; set; }
    public bool Allocated;
    public InventorySlot AllocatedBy;
    public bool Allocating;
    public List<InventorySlot> AllocatingTo = new();
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Inventory.carriedItem == null)
        {
            if (!Allocated) return;
            if (Allocated)
            {
                if (!Allocating && AllocatedBy != null)
                {
                    Inventory.Singleton.SetCarriedItem(AllocatedBy.Item, true);
                    Deallocate(AllocatedBy);
                }
                else if (Allocating)
                {
                    Inventory.Singleton.SetCarriedItem(Item, true);
                    Deallocate(this);
                }
                else 
                {
                    Inventory.Singleton.SetCarriedItem(Item, true);
                    this.Allocated = false;
                }
            }
        }
        else if (Inventory.carriedItem != null)
        {
            (int overlap, InventoryItem t) = canAllocate(Inventory.carriedItem);
            if (overlap == 2) return;
            else
            {
                allocateItem(Inventory.carriedItem, t);
            }
        }
    }
    private void allocateItem(InventoryItem item, InventoryItem reallocate)
    {
        Inventory.carriedItem = null;
        if (reallocate != null) Inventory.Singleton.SetCarriedItem(reallocate, true);
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        int index = transform.GetSiblingIndex();
        InventorySlot Allocator = Inventory.Singleton.GetSlot(index + (x - 1) + (y - 1) * 15);
        Allocator.Allocated = true;
        Allocator.Item = item;
        Allocator.Item.slot = Allocator;
        Allocator.Item.transform.SetParent(Allocator.transform);
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                InventorySlot temp = Inventory.Singleton.GetSlot(j * 15 + i);
                if (Allocator == temp) continue;
                temp.Allocated = true;
                temp.AllocatedBy = Allocator;
                Allocator.AllocatingTo.Add(temp);
                
            }
        }
        if (Allocator.AllocatingTo != null) Allocator.Allocating = true;
        RectTransform rt = Allocator.Item.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(item.data.SlotSize.x * item.data.Size.x, item.data.SlotSize.y * item.data.Size.y);
        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;
        if (item.data.SlotSize.x > 1 && item.data.SlotSize.y > 1)
        {
            rt.anchoredPosition += new Vector2(-item.data.Size.x / 2, item.data.SlotSize.y * item.data.Size.y / 2 - item.data.Size.y / 2);
        }
        else if (item.data.SlotSize.y > 1)
        {
            rt.anchoredPosition += new Vector2(0, item.data.SlotSize.y * item.data.Size.y / 2 - item.data.Size.y / 2);
        }
        else if (item.data.SlotSize.x > 1)
        {
            rt.anchoredPosition += new Vector2(item.data.Size.x / 2, 0);
        }
    }
    public List<InventorySlot> GetTargetSlots(InventoryItem item)
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        int index = transform.GetSiblingIndex();
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                slots.Add(Inventory.Singleton.GetSlot(j * 15 + i));
            }
        }
        return slots;
    }
    private (int, InventoryItem) canAllocate(InventoryItem item)
    {
        InventoryItem TempItem = null;
        int index = transform.GetSiblingIndex();
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        int itemIndex = 0;
        if (index + (x - 1) + (y - 1) * 15 >= transform.parent.childCount) return (2, null);
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                InventorySlot check = Inventory.Singleton.GetSlot(j * 15 + i);
                if (!check.Allocated) continue;
                if (!check.Allocating) check = check.AllocatedBy;
                if (TempItem == null)
                {
                    itemIndex = j * 15 + i;
                    TempItem = check.Item;
                    continue;
                }
                if (TempItem == check.Item) continue;
                else return (2, null);
            }
        }
        if (TempItem != null) {
            Deallocate(itemIndex);
            return (1, TempItem);
        }
        return (0, null);
    }
    public int CanAllocate(InventoryItem item) 
    {
        InventoryItem TempItem = null;
        int index = transform.GetSiblingIndex();
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        int itemIndex = 0;
        if (index + (x - 1) + (y - 1) * 15 >= transform.parent.childCount) return 2;
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                InventorySlot check = Inventory.Singleton.GetSlot(j * 15 + i);
                if (!check.Allocated) continue;
                if (!check.Allocating) check = check.AllocatedBy;
                if (TempItem == null)
                {
                    itemIndex = j * 15 + i;
                    TempItem = check.Item;
                    continue;
                }
                if (TempItem == check.Item) continue;
                else return 2;
            }
        }
        if (TempItem != null)
        {
            return 1;
        }
        return 0;
    }
    private void Deallocate(int index)
    {
        InventorySlot slot = Inventory.Singleton.GetSlot(index);
        if (slot.Allocating)
        {
            Deallocate(slot);
        }
        else
        {
            Deallocate(slot.AllocatedBy);
        }
    }
    private void Deallocate(InventorySlot slot)
    {
        foreach (InventorySlot s in slot.AllocatingTo)
        {
            s.Allocated = false;
            s.AllocatedBy = null;
            s.Item = null;
        }
        slot.Allocated = false;
        slot.Item = null;
        slot.Allocating = false;
        slot.AllocatingTo = new();
    }
    public void Highlight(Color color)
    {
        image.color = color;
    }

    public void ResetHighlight()
    {
        image.color = Color.clear;
    }
}
