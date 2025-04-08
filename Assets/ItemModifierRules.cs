using System.Collections.Generic;
using UnityEngine;

public class ItemModifierRules
{
    public int Implicits;
    public int Prefixes;
    public int Suffixes;
    public int Enchants;

    public ItemModifierRules(int implicits, int prefixes, int suffixes, int enchants = 0)
    {
        Implicits = implicits;
        Prefixes = prefixes;
        Suffixes = suffixes;
        Enchants = enchants;
    }
}
public static class ModifierRules
{
    public static readonly Dictionary<Rarity, ItemModifierRules> RarityLimits = new()
    {
        { Rarity.Common,    new(1, 0, 0) },
        { Rarity.Magic,     new(1, 1, 1) },
        { Rarity.Rare,      new(1, 2, 2) },
        { Rarity.Legendary, new(1, 3, 2) },
        { Rarity.Divine,    new(1, 3, 3) },
        { Rarity.World,     new(1, 3, 3, 1) },
    };
}