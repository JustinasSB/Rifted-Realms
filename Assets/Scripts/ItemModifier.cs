using System;
using UnityEngine;
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
        setText();
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
        setText();
    }
    public ItemModifier(ItemModifier clone)
    {
        OperationType = clone.OperationType;
        AffectedStat = clone.AffectedStat;
        Type = clone.Type;
        Weight = clone.Weight;
        Text = clone.Text;
        Extra = clone.Extra;
        RolledValue = clone.RolledValue;
    }
    private void setText()
    {
        switch ((int)(this.OperationType + (int)this.Scope*10)) {
            case 0:
                Text = $"Adds {RolledValue} {AffectedStat.GetDisplayName()}";
                break;
            case 1:
                Text = $"{RolledValue}% increased {AffectedStat.GetDisplayName()}";
                break;
            case 2:
                Text = $"+{RolledValue}% more {AffectedStat.GetDisplayName()}";
                break;
            case 3:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} to {AffectedStat.GetDisplayName()}";
                break;
            case 4:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} as extra {AffectedStat.GetDisplayName()}";
                break;
            case 10:
                Text = $"+{RolledValue} To {AffectedStat.GetDisplayName()}";
                break;
            case 11:
                Text = $"+{RolledValue}% To {AffectedStat.GetDisplayName()}";
                break;
            case 12:
                Text = $"+{RolledValue}% Multiplier To {AffectedStat.GetDisplayName()}";
                break;
            case 13:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} added as {AffectedStat.GetDisplayName()}";
                break;
            case 14:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} added as extra {AffectedStat.GetDisplayName()}";
                break;
        }
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
    /// <summary>
    /// ONLY USED FOR TOOLTIP DISPLAY
    /// </summary>
    public void AddToRolledValue(float value)
    {
        RolledValue += value;
        setText();
    }
    public void RollValue()
    {
        RolledValue = (float)Math.Round(UnityEngine.Random.Range(RollRangeMin, RollRangeMax));
    }
}