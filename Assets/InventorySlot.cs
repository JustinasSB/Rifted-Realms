using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        ResetTooltip();
    }
    private void ResetTooltip()
    {
        TooltipTrigger trigger = this.GetComponent<TooltipTrigger>();
        if (trigger != null)
        {
            trigger.ResetManager();
            trigger.OnPointerExit();
            trigger.OnPointerEnter();
        }
    }
    private void SetTooltip(InventorySlot slot, bool clear = false)
    {
        TooltipTrigger trigger = slot.GetComponent<TooltipTrigger>();
        if (trigger == null) return;
        if (clear)
        {
            trigger.rt = null;
            trigger.item = null;
            return;
        }
        if (slot.AllocatedBy != null) slot = slot.AllocatedBy;
        if (slot.AllocatingTo.Count != 0) slot = slot.AllocatingTo[0];
        trigger.rt = slot.GetComponent<RectTransform>();
        trigger.item = slot.Item;
        trigger.Initialize();
    }
    private void allocateItem(InventoryItem item, InventoryItem reallocate)
    {
        Inventory.Singleton.RemoveCarriedItem();
        if (reallocate != null) Inventory.Singleton.SetCarriedItem(reallocate, true);
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        int index = transform.GetSiblingIndex();
        InventorySlot Allocator = Inventory.Singleton.GetSlot(index + (x - 1) + (y - 1) * 15);
        Allocator.Allocated = true;
        Allocator.Item = item;
        Allocator.Item.transform.SetParent(Allocator.transform);
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                InventorySlot temp = Inventory.Singleton.GetSlot(j * 15 + i);
                if (Allocator == temp) continue;
                temp.Item = item;
                temp.Allocated = true;
                temp.AllocatedBy = Allocator;
                Allocator.AllocatingTo.Add(temp);
                SetTooltip(temp);
            }
        }
        SetTooltip(Allocator);
        if (Allocator.AllocatingTo != null) Allocator.Allocating = true;
        correctAnchor(Allocator.Item.GetComponent<RectTransform>(), item);
    }
    public void allocateItem(InventoryItem item)
    {
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        int index = transform.GetSiblingIndex();
        InventorySlot Allocator = Inventory.Singleton.GetSlot(index + (x - 1) + (y - 1) * 15);
        Allocator.Allocated = true;
        Allocator.Item = item;
        Allocator.Item.transform.SetParent(Allocator.transform);
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                InventorySlot temp = Inventory.Singleton.GetSlot(j * 15 + i);
                if (Allocator == temp) continue;
                temp.Item = item;
                temp.Allocated = true;
                temp.AllocatedBy = Allocator;
                Allocator.AllocatingTo.Add(temp);
                SetTooltip(temp);
            }
        }
        SetTooltip(Allocator);
        if (Allocator.AllocatingTo != null) Allocator.Allocating = true;
        correctAnchor(Allocator.Item.GetComponent<RectTransform>(), item);
        
    }
    private void correctAnchor(RectTransform rt, InventoryItem item)
    {
        rt.sizeDelta = new Vector2(item.data.SlotSize.x * item.data.Size.x, item.data.SlotSize.y * item.data.Size.y);
        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;
        if (item.data.SlotSize != Vector2.one)
        {
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1f, 0f);
        }
        else
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
        }
    }
    private bool checkValidity(int index, int x, int y)
    {
        if ((index % 15) + x > 15) return false;
        if ((index / 15) + y > 6) return false;
        return true;
    }
    public List<InventorySlot> GetTargetSlots(InventoryItem item)
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        int index = transform.GetSiblingIndex();
        int x = item.data.SlotSize.x;
        int y = item.data.SlotSize.y;
        //inverted for loops to avoid horizontal wrapping while keeping correct highlights
        for (int j = index / 15; j < index / 15 + y; j++)
        {
            for (int i = index % 15; i < index % 15 + x; i++)
            {
                if (i >= 15)
                    break;

                int flatIndex = j * 15 + i;
                if (flatIndex >= Inventory.Singleton.inventorySlots.Length)
                    continue;

                slots.Add(Inventory.Singleton.GetSlot(flatIndex));
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
        if (!checkValidity(index, x, y)) return (2, null);
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                if (j*15+i> Inventory.Singleton.inventorySlots.Length) return (2, null);
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
        if (!checkValidity(index, x, y)) return 2;
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                if (j * 15 + i > Inventory.Singleton.inventorySlots.Length) return 2;
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
    public bool CanAllocate(BaseItem item)
    {
        int index = transform.GetSiblingIndex();
        int x = item.SlotSize.x;
        int y = item.SlotSize.y;
        if (!checkValidity(index, x, y)) return false;
        for (int i = index % 15; i < index % 15 + x; i++)
        {
            for (int j = index / 15; j < index / 15 + y; j++)
            {
                if (j * 15 + i > Inventory.Singleton.inventorySlots.Length) return false;
                InventorySlot check = Inventory.Singleton.GetSlot(j * 15 + i);
                if (check.Allocated) return false;
                if (check.Allocating) return false;
            }
        }
        return true;
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
            SetTooltip(s, true);
            s.Allocated = false;
            s.AllocatedBy = null;
            s.Item = null;
        }
        SetTooltip(slot, true);
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
