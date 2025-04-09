using System;
using System.Collections.Generic;
using System.Linq;

public static class ModifierGenerator
{
    public static List<ItemModifier> GenerateModifiers(InventoryItem item)
    {
        ItemModifierRules constraints = ModifierRules.RarityLimits[item.Rarity];
        List<ItemModifier> result = new List<ItemModifier>();
        int itemLevel = item.ItemLevel;
        ItemType type = item.data.ItemType;
        ItemSpecific specificType = item.data.ItemSpecific;
        List<ItemModifier> possibleModifiers = ModifierDictionary.ModifierPools[(type, specificType)]
            .Where(level => level.LevelRequirement < itemLevel)
            .ToList();

        if (constraints.Enchants > 0)
        {
            List<ItemModifier> temp = GenerateModifiers(possibleModifiers, constraints.Implicits, ModifierType.Enchant);
            foreach (ItemModifier modifier in temp)
            {
                result.Add(modifier);
            }
        }
        if (constraints.Implicits > 0) {
            List<ItemModifier> temp = GenerateModifiers(possibleModifiers, constraints.Implicits, ModifierType.Implicit);
            foreach (ItemModifier modifier in temp) 
            {
                result.Add(modifier);
            }
        }
        if (constraints.Prefixes > 0)
        {
            List<ItemModifier> temp = GenerateModifiers(possibleModifiers, constraints.Implicits, ModifierType.Prefix);
            foreach (ItemModifier modifier in temp)
            {
                result.Add(modifier);
            }
        }
        if (constraints.Suffixes > 0)
        {
            List<ItemModifier> temp = GenerateModifiers(possibleModifiers, constraints.Implicits, ModifierType.Suffix);
            foreach (ItemModifier modifier in temp)
            {
                result.Add(modifier);
            }
        }
        return result;
    }
    private static List<ItemModifier> GenerateModifiers(List<ItemModifier> possibleModifiers, int count, ModifierType type)
    {
        List<ItemModifier> result = new List<ItemModifier>();

        List<ItemModifier> filtered = possibleModifiers
        .Where(mod => mod.Type == type)
        .ToList();
        if (filtered.Count < 1) return null;
        List<IGrouping<float, ItemModifier>> groupBuckets = filtered
        .GroupBy(mod => mod.Group)
        .ToList();
        if (groupBuckets.Count < 1) return null;

        var groupSampler = new VoseAliasSampler<IGrouping<float, ItemModifier>>(groupBuckets, g => g.Sum(m => m.Weight));
        HashSet<float> selectedGroups = new();
        while (result.Count < count && selectedGroups.Count < groupBuckets.Count)
        {
            var group = groupSampler.Sample();
            if (selectedGroups.Contains(group.Key)) continue;

            var modSampler = new VoseAliasSampler<ItemModifier>(group.ToList(), m => m.Weight);
            result.Add(modSampler.Sample());
            selectedGroups.Add(group.Key);
        }
        return result;
    }

    //https://www.keithschwarz.com/darts-dice-coins/
    public class VoseAliasSampler<T>
    {
        private readonly T[] outcomes;
        private readonly int[] alias;
        private readonly double[] probabilities;
        private readonly Random random = new Random();

        public VoseAliasSampler(List<T> items, Func<T, double> weightSelector)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("Item list must be non-null and non-empty.");

            int n = items.Count;

            outcomes = new T[n];
            probabilities = new double[n];
            alias = new int[n];

            // Dividing selection weight by total weight and scaling by n ensures the average q=1.
            double[] scaled = items.Select(weightSelector).ToArray();
            double total = scaled.Sum();
            for (int i = 0; i < n; i++)
            {
                outcomes[i] = items[i];
                scaled[i] = (scaled[i] / total) * n;
            }

            // Split scaled probabilities into small and large buckets O(n) time complexity initialization
            var small = new Queue<int>();
            var large = new Queue<int>();

            for (int i = 0; i < n; i++)
            {
                if (scaled[i] < 1.0)
                    small.Enqueue(i);
                else
                    large.Enqueue(i);
            }

            // Construct the alias and probability tables
            while (small.Count > 0 && large.Count > 0)
            {
                int s = small.Dequeue();
                int l = large.Dequeue();

                probabilities[s] = scaled[s];
                alias[s] = l;

                // Adjust the large bucket after using some of its weight
                scaled[l] = (scaled[l] + scaled[s]) - 1.0;

                if (scaled[l] < 1.0)
                    small.Enqueue(l);
                else
                    large.Enqueue(l);
            }

            // all remaining probabilities should be 1
            while (large.Count > 0)
            {
                int i = large.Dequeue();
                probabilities[i] = 1.0;
            }

            while (small.Count > 0)
            {
                int i = small.Dequeue();
                probabilities[i] = 1.0;
            }
        }
        public T Sample()
        {
            int i = random.Next(outcomes.Length);
            return random.NextDouble() < probabilities[i] ? outcomes[i] : outcomes[alias[i]];
        }
    }
}

