using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class ItemModifier
{
    public ModifierType Type { get; set; }
    public OperationType OperationType { get; set; }
    public ModifierScope Scope { get; set; }
    public StatType AffectedStat { get; set; }
    public float Tier { get; set; }
    public float Group { get; set; }
    public float RollRangeMin { get; set; }
    public float RollRangeMax { get; set; }
    public float Weight { get; set; }
    public float LevelRequirement { get; set; }
    public string Text { get; private set; }
    public StatType Extra { get; set; }
    public float RolledValue { get; private set; }

    public ItemModifier(OperationType operationType, ModifierType type, ModifierScope scope, StatType affectedStat, float tier, float group, float min, float max, float weight, float levelRequirement)
    {
        OperationType = operationType;
        Type = type;
        Scope = scope;
        AffectedStat = affectedStat;
        Tier = tier;
        Group = group;
        RollRangeMin = min;
        RollRangeMax = max;
        Weight = weight;
        LevelRequirement = levelRequirement;
        RollValue();
        Text = $"+{RolledValue} {AffectedStat}";
    }
    public ItemModifier(OperationType operationType, ModifierType type, ModifierScope scope, StatType to, float tier, float group, float min, float max, float weight, float levelRequirement, StatType from)
    {
        OperationType = operationType;
        Type = type;
        Scope = scope;
        AffectedStat = to;
        Tier = tier;
        Group = group;
        RollRangeMin = min;
        RollRangeMax = max;
        Weight = weight;
        LevelRequirement = levelRequirement;
        Extra = from;
        RollValue();
        Text = $"+{RolledValue} {AffectedStat}";
    }
    public ItemModifier Clone()
    {
        if (Extra != default)
        {
            return new ItemModifier(
                OperationType,
                Type,
                Scope,
                AffectedStat,
                Tier,
                Group,
                RollRangeMin,
                RollRangeMax,
                Weight,
                LevelRequirement,
                Extra
            );
        }
        else
        {
            return new ItemModifier(
                OperationType,
                Type,
                Scope,
                AffectedStat,
                Tier,
                Group,
                RollRangeMin,
                RollRangeMax,
                Weight,
                LevelRequirement
            );
        }
    }
    public void RollValue()
    {
        RolledValue = (float)Math.Round(UnityEngine.Random.Range(RollRangeMin, RollRangeMax));
    }
}