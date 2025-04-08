using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Image Border;
    InventoryItem Item { get; set; }
    bool Allocated;
    [SerializeField] ItemType itemType;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Inventory.carriedItem == null)
        {
            if (this.Item == null) return;
            RemoveModifiers();
            deallocate();
        }
        else if (Inventory.carriedItem != null)
        {
            if (Inventory.carriedItem.data.ItemType != itemType) return;
            if (Item != null)
            {
                RemoveModifiers();
                allocateItem(Inventory.carriedItem, Item);
            }
            else
            {
                allocateItem(Inventory.carriedItem, null);
            }
            ApplyModifiers();
        }
    }
    private void Update()
    {
        if (Inventory.carriedItem == null || Inventory.carriedItem.data.ItemType != itemType)
        {
            Border.color = Color.white;
            return;
        }
        Border.color = Color.yellow;

    }
    private void deallocate()
    {
        Inventory.Singleton.SetCarriedItem(Item, false);
        this.Item = null;
    }
    private void RemoveModifiers()
    {
        
    }
    private void ApplyModifiers()
    {
        
    }
    private void allocateItem(InventoryItem item, InventoryItem reallocate)
    {
        Inventory.carriedItem = null;
        if (reallocate!=null)
        {
            Inventory.Singleton.SetCarriedItem(reallocate, false);
        }
        this.Item = item;
        Item.transform.SetParent(transform);
        RectTransform rt = this.Item.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(item.data.SlotSize.x * item.data.Size.x, item.data.SlotSize.y * item.data.Size.y);
        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;
    }
    public EquipmentSlot GetTargetSlot()
    {
        return this;
    }
    public int CanAllocate(InventoryItem item)
    {
        if (item.data.ItemType == this.itemType) return 0;
        return 2;
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
