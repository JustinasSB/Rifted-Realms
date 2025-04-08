
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

    public ItemModifier(OperationType operationType, ModifierType type, ModifierScope scope, StatType affectedStat, float tier, float group, float min, float max)
    {
        OperationType = operationType;
        Type = type;
        Scope = scope;
        AffectedStat = affectedStat;
        Tier = tier;
        Group = group;
        RollRangeMin = min;
        RollRangeMax = max;
    }
}