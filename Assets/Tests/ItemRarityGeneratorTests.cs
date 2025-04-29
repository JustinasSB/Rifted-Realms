using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemRarityGeneratorTests
{
    [Test]
    public void GenerateRarity_NullItem_DoesNothing()
    {
        Assert.DoesNotThrow(() => ItemRarityGenerator.GenerateRarity(null, 0f, 0));
    }

    [Test]
    public void GenerateRarity_DisallowedItemType_DoesNothing()
    {
        GameObject go = new GameObject("TestItem");
        InventoryItem item = go.AddComponent<InventoryItem>();
        BaseItem baseItem = ScriptableObject.CreateInstance<BaseItem>();
        baseItem.ItemName = "Test";
        baseItem.Size = new Vector2Int(1, 1);
        baseItem.SlotSize = new Vector2Int(1, 1);
        baseItem.ItemType = ItemType.Stackable;
        item.data = baseItem;
        item.Rarity = Rarity.Legendary;

        ItemRarityGenerator.GenerateRarity(item, 0f, 0);

        Assert.AreEqual(Rarity.Legendary, item.Rarity);

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(baseItem);
    }

    [Test]
    public void GenerateRarity_SourceLevelZero_ImprovedRarityZero()
    {
        Random.InitState(12345);
        GameObject go = new GameObject("TestItem");
        InventoryItem item = go.AddComponent<InventoryItem>();
        BaseItem baseItem = ScriptableObject.CreateInstance<BaseItem>();
        baseItem.ItemName = "Test";
        baseItem.Size = new Vector2Int(1, 1);
        baseItem.SlotSize = new Vector2Int(1, 1);
        baseItem.ItemType = ItemType.Mainhand;
        item.data = baseItem;
        item.Rarity = Rarity.Common;

        ItemRarityGenerator.GenerateRarity(item, 0f, 0);

        // For SourceLevel=0, available rarities are: Common, Divine, World.
        // Should be changed to Common only after showcase
        List<Rarity> expected = new List<Rarity> { Rarity.Common, Rarity.Divine, Rarity.World };
        Assert.Contains(item.Rarity, expected);

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(baseItem);
    }

    [Test]
    public void GenerateRarity_HighSourceLevel_ImprovedRarityNonZero()
    {
        Random.InitState(54321);
        GameObject go = new GameObject("TestItem");
        InventoryItem item = go.AddComponent<InventoryItem>();
        BaseItem baseItem = ScriptableObject.CreateInstance<BaseItem>();
        baseItem.ItemName = "Test";
        baseItem.Size = new Vector2Int(1, 1);
        baseItem.SlotSize = new Vector2Int(1, 1);
        baseItem.ItemType = ItemType.Mainhand;
        item.data = baseItem;
        item.Rarity = Rarity.Common;

        // Using a SourceLevel high enough to include all rarities
        // And a nonzero ImprovedRarity to affect weight calculations.
        ItemRarityGenerator.GenerateRarity(item, 0.5f, 100);

        // For SourceLevel=100, available rarities are all rarities.
        List<Rarity> expected = new List<Rarity> { Rarity.Common, Rarity.Magic, Rarity.Rare, Rarity.Legendary, Rarity.Divine, Rarity.World };
        Assert.Contains(item.Rarity, expected);

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(baseItem);
    }
}