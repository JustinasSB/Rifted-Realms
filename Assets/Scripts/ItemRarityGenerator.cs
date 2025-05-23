using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRarityGenerator
{
    //most rare to least rare order to more easily apply weighting improvment with rarity stat when generating
    private static readonly List<(Rarity, int)> RarityWeights = new List<(Rarity, int)>()
    {
        new(Rarity.World, 8192),
        new(Rarity.Divine, 8192),
        new(Rarity.Legendary, 128),
        new(Rarity.Rare, 512),
        new(Rarity.Magic, 2048),
        new(Rarity.Common, 8192)
    };
    private static readonly Dictionary<Rarity, int> RarityLevelRequirements = new()
    {
        { Rarity.Common, 0 },
        { Rarity.Magic, 5 },
        { Rarity.Rare, 20 },
        { Rarity.Legendary, 40 },
        { Rarity.Divine, 0 },
        { Rarity.World, 0 }
    };
    public static void GenerateRarity(InventoryItem Item, float ImprovedRarity, int SourceLevel)
    {
        if (Item == null || Item.data.ItemType == ItemType.Stackable || Item.data.ItemType == ItemType.None) return;
        Rarity rarity = Rarity.Common;
        List<(Rarity, int)> modifiedWeights = new List<(Rarity, int)>();
        float count = (float)RarityWeights.Count;
        int totalWeight = 0;

        List<Rarity> availableRarities = RarityLevelRequirements
            .Where(requirement => requirement.Value <= SourceLevel)
            .Select(req => req.Key)
            .ToList();

        foreach ((Rarity, int) item in RarityWeights)
        {
            int weight = (int)(item.Item2 * (1f + ImprovedRarity*count--));
            if (availableRarities.Contains(item.Item1))
            {
                totalWeight += weight;
                modifiedWeights.Add((item.Item1, weight));
            }
        }
        int choice = Random.Range(0, totalWeight);
        int findRarity = 0;
        foreach ((Rarity, int) item in modifiedWeights)
        {
            findRarity += item.Item2;
            if (findRarity >= choice)
            {
                rarity = item.Item1;
                break;
            }
        }
        Item.Rarity = rarity;
    }
}
