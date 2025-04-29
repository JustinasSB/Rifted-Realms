using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PassiveTreeAbilityNode : PassiveTreeNode, IPointerClickHandler
{
    Guid id;
    InventoryItem item;
    AbilityItem abilityItem;
    [SerializeField] List<PassiveTreeAbilityNode> relatedNodes = new();
    private event Action OnEquip;
    [SerializeField] int levelToUnhide;
    private List<StatModifier> activeModifiers = new();
    public void Start()
    {
        id = Guid.NewGuid();
        Inventory.Singleton.OnCarriedItemChange += item => carriedItemChanged(item);
        foreach (PassiveTreeAbilityNode node in relatedNodes)
        {
            node.OnEquip += relationsChanged;
        }
        if (levelToUnhide == 0) return;
        LevelManager.level.OnLevelUp += UnlockNode;
        this.gameObject.SetActive(false);
    }
    private void UnlockNode(int level)
    {
        if (level != levelToUnhide) return;
        this.gameObject.SetActive(true);
        LevelManager.level.OnLevelUp -= UnlockNode;
    }
    private void relationsChanged()
    {
        if (item == null || abilityItem.ability.support) return;
        activeGem();
    }
    private void activeGem()
    {
        removeModifiers();
        activeModifiers.Clear();
        abilityItem.ManaCost = abilityItem.BaseManaCost;
        foreach (PassiveTreeAbilityNode node in relatedNodes) 
        {
            if (node.abilityItem == null || !node.abilityItem.ability.support) continue;
            foreach (var modifier in node.abilityItem.ability.modifiers)
            {
                if (!abilityItem.ability.Stats.TryGetValue(modifier.To, out var value)) continue;
                value.Item1.ModifyStat(modifier.OperationType, modifier.Value);
                activeModifiers.Add(modifier);
                abilityItem.ManaCost += abilityItem.BaseManaCost * (node.abilityItem.Multiplier/100);
            }
        }
    }
    private void removeModifiers()
    {
        abilityItem.ManaCost = abilityItem.BaseManaCost;
        foreach (StatModifier modifier in activeModifiers)
        {
            if (!this.abilityItem.ability.Stats.TryGetValue(modifier.To, out var value)) continue;
            switch (modifier.OperationType)
            {
                case OperationType.Add:
                    value.Item1.RemoveBaseAdded(modifier.Value);
                    break;
                case OperationType.Increase:
                    value.Item1.RemoveIncrease(modifier.Value);
                    break;
                case OperationType.Multiply:
                    value.Item1.RemoveMultiplier(modifier.Value);
                    break;
            }
        }
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
            SetTooltip();
        }
        else if (item != null)
        {
            Deallocate();
            SetTooltip();
        }
        else
        {
            if (this.special) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (this.Allocated) { deallocateNode(); return; }
            if (LevelManager.level.SkillPoints <= 0) return;
            else allocateNode();
        }
        ResetTooltip();
    }
    private void Allocate(InventoryItem carried, InventoryItem slotted)
    {
        if (Inventory.carriedItem.ability == null) return;
        Inventory.Singleton.RemoveCarriedItem();
        if (slotted != null) Inventory.Singleton.SetCarriedItem(slotted, true);
        item = carried;
        abilityItem = item.ability;
        if (!abilityItem.ability.support)
        {
            AbilityEvents.TriggerAbilityEquipped(abilityItem, id);
            activeGem();
        }
        item.transform.SetParent(transform);
        RectTransform rt = item.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(item.data.SlotSize.x * item.data.Size.x, item.data.SlotSize.y * item.data.Size.y);
        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        OnEquip?.Invoke();
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
    public void DestroyTooltip()
    {
        TooltipTrigger trigger = this.GetComponent<TooltipTrigger>();
        if (trigger != null)
        {
            trigger.OnPointerExit();
        }
    }
    private void Deallocate()
    {
        Inventory.Singleton.SetCarriedItem(item, false);
        removeModifiers();
        item = null;
        if (!abilityItem.ability.support)
        {
            AbilityEvents.TriggerAbilityEquipped(null, id);
        }
        abilityItem = null;
        OnEquip?.Invoke();
    }
}