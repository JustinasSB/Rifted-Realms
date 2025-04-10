using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour
{
    public Image itemIcon;
    public ItemData data { get; set; }
    public Rarity Rarity { get; set; }
    public int ItemLevel { get; set; }
    public List<ItemModifier> Modifiers { get; set; }
    public ItemStatsManager Stats = new ItemStatsManager();

    void Awake()
    {
        itemIcon = GetComponent<Image>();
    }
    public void InitializeInInventory(ItemData item, int location)
    {
        data = item;
        itemIcon.sprite = item.Icon;
        itemIcon.raycastTarget = false;
        ItemRarityGenerator.GenerateRarity(this, 0f, LevelManager.level.CurrentLevel);
        ItemLevel = LevelManager.level.CurrentLevel;
        Inventory.Singleton.PlaceItem(this, location);
        foreach (ItemTemplateStats stat in data.ItemStats)
        {
            Stats.List.Add(stat.StatType,new Stat(stat.BaseValue, stat.StatType));
        }
        if (data.ItemType!= ItemType.Stackable)
            Modifiers = ModifierGenerator.GenerateModifiers(this);
        foreach (ItemModifier modifier in Modifiers)
        {
            if (modifier.Scope == ModifierScope.Local)
            {
                if (modifier.OperationType == OperationType.Add)
                    Stats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue);
                else 
                    Stats.ModifyStat(modifier.AffectedStat, modifier.OperationType, modifier.RolledValue / 100);
            }
        }
    }
    public void AddValuesToPlayer()
    {
        
    }
    public void RemoveValuesFromPlayer() 
    {
        
    }
}