using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour
{
    public Image itemIcon;
    public BaseItem data { get; set; }
    public ItemData item { get; set; }
    public AbilityItem ability { get; set; }
    public Rarity Rarity = Rarity.None;
    public int ItemLevel { get; set; }
    public string ItemName;
    public List<ItemModifier> Modifiers = new List<ItemModifier>();
    public ItemStatsManager Stats = new ItemStatsManager();

    void Awake()
    {
        itemIcon = GetComponent<Image>();
    }
    public void InitializeInInventory(ItemData item, int location)
    {
        this.data = item;
        this.item = item;
        itemIcon.sprite = item.Icon;
        itemIcon.raycastTarget = false;
        ItemRarityGenerator.GenerateRarity(this, 0f, LevelManager.level.CurrentLevel);
        ItemLevel = LevelManager.level.CurrentLevel;
        Inventory.Singleton.PlaceItem(this, location);
        foreach (ItemTemplateStats stat in this.item.ItemStats)
        {
            Stats.List.Add(stat.StatType,new Stat(stat.BaseValue, stat.StatType));
        }
        if (data.ItemType!= ItemType.Stackable)
            Modifiers = ModifierGenerator.GenerateModifiers(this);
        if (Rarity <= Rarity.Magic)
        {
            NameGenerator.GenerateName(this);
        }
        if (Modifiers == null) return;
        foreach (ItemModifier modifier in Modifiers)
        {
            if (modifier.Scope == ModifierScope.Local)
            {
                if(!Stats.List.ContainsKey(modifier.AffectedStat)) Stats.List.Add(modifier.AffectedStat, new Stat(0, modifier.AffectedStat));
                if (modifier.OperationType == OperationType.Add)
                    Stats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue);
                else if (modifier.OperationType == OperationType.Extra)
                    Stats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue / 100, modifier.Extra);
                else
                    Stats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue / 100);
            }
        }
    }
    public void InitializeInInventory(AbilityItem ability, int location)
    {
        this.data = ability;
        this.ability = ability.Clone();
        itemIcon.sprite = ability.Icon;
        itemIcon.raycastTarget = false;
        Inventory.Singleton.PlaceItem(this, location);
    }
    public bool Equals(InventoryItem other)
    {
        if (data == other.data 
            && Stats == other.Stats
            && ItemName == other.ItemName
            && Rarity == other.Rarity
            && ItemLevel == other.ItemLevel)
            return true;
        return false;
    }
}