using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;


public class PassiveTreeAbilityNode : PassiveTreeNode, IPointerClickHandler
{
    Guid id;
    InventoryItem item;
    AbilityItem ability;
    public void Start()
    {
        id = Guid.NewGuid();
        Inventory.Singleton.OnCarriedItemChange += item => carriedItemChanged(item);
    }
    private void carriedItemChanged(InventoryItem item)
    {
        if (item == null || item.ability == null)
        {
            highLightNode(false);
            return;
        }
        highLightNode(true);
    }
    public new void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.carriedItem != null)
        {
            Allocate(Inventory.carriedItem, this.item);
        }
        else if (item != null)
        {
            Deallocate();
        }
        else
        {
            if (this.special) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (this.Allocated) { deallocateNode(); return; }
            if (LevelManager.level.SkillPoints <= 0) return;
            else allocateNode();
        }
    }
    private void Allocate(InventoryItem carried, InventoryItem slotted)
    {
        if (Inventory.carriedItem.ability == null) return;
        Inventory.Singleton.RemoveCarriedItem();
        if (slotted != null) Inventory.Singleton.SetCarriedItem(slotted, true);
        item = carried;
        ability = item.ability;
        AbilityEvents.TriggerAbilityEquipped(ability, id);
        item.transform.SetParent(transform);
        RectTransform rt = item.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(item.data.SlotSize.x * item.data.Size.x, item.data.SlotSize.y * item.data.Size.y);
        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        SetTooltip();
    }
    private void SetTooltip()
    {
        TooltipTrigger trigger = this.GetComponent<TooltipTrigger>();
        if (trigger != null)
        {
            trigger.rt = this.GetComponent<RectTransform>();
            trigger.item = this.item;
        }
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
    private void Deallocate()
    {
        Inventory.Singleton.SetCarriedItem(item, false);
        item = null;
        ability = null;
        AbilityEvents.TriggerAbilityEquipped(null, id);
        ResetTooltip();
    }
}