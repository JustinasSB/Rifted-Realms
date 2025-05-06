using System;
using UnityEngine;
using static Codice.Client.BaseCommands.Import.Commit;
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

        int negative = 0;
        if (OperationType == OperationType.Multiply)
        {
            negative = RolledValue < 100 ? 1 : 0;
        }
        else negative = RolledValue < 0 ? 1 : 0;

        switch ((int)(this.OperationType + negative * 10 + (int)this.Scope*100)) {
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
            case 100:
                Text = $"+{RolledValue} To {AffectedStat.GetDisplayName()}";
                break;
            case 101:
                Text = $"+{RolledValue}% To {AffectedStat.GetDisplayName()}";
                break;
            case 102:
                Text = $"+{RolledValue - 100}% Multiplier To {AffectedStat.GetDisplayName()}";
                break;
            case 103:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} added as {AffectedStat.GetDisplayName()}";
                break;
            case 104:
                Text = $"{RolledValue}% of {Extra.GetDisplayName()} added as extra {AffectedStat.GetDisplayName()}";
                break;
            case 110:
                Text += $"{RolledValue} To {AffectedStat.GetDisplayName()}";
                break;
            case 111:
                Text += $"{Math.Abs(RolledValue)}% Reduction To {AffectedStat.GetDisplayName()}";
                break;
            case 112:
                Text += $"{Math.Abs(100 - RolledValue)}% Less {AffectedStat.GetDisplayName()}";
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