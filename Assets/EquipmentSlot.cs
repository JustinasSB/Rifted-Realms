using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Image Border;
    InventoryItem Item { get; set; }
    bool Allocated;
    [SerializeField] ItemType itemType;
    public static event Action<InventoryItem> OnMainHandEquipmentChanged;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Inventory.carriedItem == null)
        {
            if (this.Item == null) return;
            adjustEffect(true);
            deallocate();
            PublishUpdate();
            SetTooltip();
        }
        else if (Inventory.carriedItem != null)
        {
            if (Inventory.carriedItem.data.ItemType != itemType) return;
            if (Item != null)
            {
                adjustEffect(true);
                allocateItem(Inventory.carriedItem, Item);
            }
            else
            {
                allocateItem(Inventory.carriedItem, null);
            }
            adjustEffect(false);
            PublishUpdate();
            SetTooltip();
        }
        ResetTooltip();
    }
    private void adjustEffect(bool remove)
    {
        if (remove)
        {
            if (this.Item.Modifiers != null) RemoveModifiers();
            if (this.Item.Stats != null) RemoveStats();
            return;
        }
        if (this.Item.Modifiers != null) ApplyModifiers();
        if (this.Item.Stats != null) AddStats();
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
    private void PublishUpdate()
    {
        if (itemType == ItemType.Mainhand)
        {
            OnMainHandEquipmentChanged?.Invoke(Item);
        }
    }
    private void SetTooltip()
    {
        TooltipTrigger trigger = this.GetComponent<TooltipTrigger>();
        if (trigger != null)
        {
            trigger.rt = this.GetComponent<RectTransform>();
            trigger.item = this.Item;
        }
    }
    private void deallocate()
    {
        Inventory.Singleton.SetCarriedItem(Item, false);
        this.Item = null;
    }
    private void AddStats()
    {
        if (this.itemType != ItemType.Mainhand
           && this.itemType != ItemType.Offhand)
        {
            foreach (var value in Item.Stats.List)
            {
                PlayerStatsManager.playerStats.ModifyStat(value.Key, OperationType.Add, value.Value.Value);
            }
        }
    }
    private void RemoveStats()
    {
        if (this.itemType != ItemType.Mainhand
           && this.itemType != ItemType.Offhand)
        {
            foreach (var value in Item.Stats.List)
            {
                PlayerStatsManager.playerStats.ModifyStat(value.Key, OperationType.AddRemove, value.Value.Value);
            }
        }
    }
    private void RemoveModifiers()
    {
        foreach (ItemModifier modifier in Item.Modifiers)
        {
            if (modifier.Scope != ModifierScope.Global) continue;
            switch (modifier.OperationType)
            {
                case OperationType.Add:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.AddRemove, modifier.RolledValue);
                    break;
                case OperationType.Increase:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.IncreaseRemove, modifier.RolledValue);
                    break;
                case OperationType.Multiply:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.MultiplyRemove, modifier.RolledValue);
                    break;
                case OperationType.Convert:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.ConvertRemove, modifier.RolledValue, modifier.Extra);
                    break;
                case OperationType.Extra:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.ExtraRemove, modifier.RolledValue, modifier.Extra);
                    break;
                case OperationType.SetBase:
                    PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, OperationType.SetBase, modifier.RolledValue);
                    break;
                default:
                    break;
            }
        }
    }
    private void ApplyModifiers()
    {
        foreach (ItemModifier modifier in Item.Modifiers)
        {
            if (modifier.Scope != ModifierScope.Global) continue;
            if (modifier.OperationType == OperationType.Convert
                || modifier.OperationType == OperationType.Extra)
            {
                PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue, modifier.Extra);
                continue;
            }
            PlayerStatsManager.playerStats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue);
        }
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
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
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
