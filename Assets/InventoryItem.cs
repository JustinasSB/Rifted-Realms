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
    public List<ItemModifier> modifiers { get; set; }

    void Awake()
    {
        itemIcon = GetComponent<Image>();
    }
    public void InitializeInInventory(ItemData item, int location)
    {
        data = item;
        itemIcon.sprite = item.Icon;
        itemIcon.raycastTarget = false;
        Inventory.Singleton.PlaceItem(this, location);
    }
}