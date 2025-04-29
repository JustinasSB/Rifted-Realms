[System.Serializable]
public class ItemTemplateStats
{
    public StatType StatType;
    public float BaseValue;
    public ItemTemplateStats(StatType statType, float baseValue)
    {
        this.StatType = statType;
        this.BaseValue = baseValue;
    }
    public Stat ToRuntimeStat()
    {
        return new Stat(BaseValue, StatType);
    }
}