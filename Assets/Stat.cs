using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Stat
{
    public string Name { get; }
    public string Description { get; }
    public float BaseValue { get; private set; }
    private List<float> baseAdded = new List<float>();
    private List<float> baseIncrease = new List<float>();
    private List<float> baseMultiplier = new List<float>();
    public StatType StatType;

    //Stores Stat and the percentage of this.Value converted into held Stat
    private Dictionary<Stat, float> conversion = new Dictionary<Stat, float>();

    //Stores Stat type and the amount of this.Value converted into held Stat type
    private Dictionary<StatType, float> scaledConversion = new Dictionary<StatType, float>();
    private Dictionary<Stat, List<float>> asExtra = new Dictionary<Stat, List<float>>();
    public float Value;
    private float valueBeforeConversion;
    //public event Action<string, float> OnStatChanged;
    public Stat(string name, string description, float baseValue, StatType statType)
    {
        this.Name = name;
        this.Description = description;
        this.BaseValue = baseValue;
        this.StatType = statType;
        RecalculateValue();
    }
    /// <summary>
    /// For use with items, minimal initialization
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="statType"></param>
    public Stat(float baseValue, StatType statType)
    {
        this.Name = statType.ToString();
        this.BaseValue = baseValue;
        this.StatType = statType;
        RecalculateValue();
    }
    /// <summary>
    /// For use in regeneration, sets stat value directly to avoid unnecessary calculations
    /// </summary>
    /// <param name="value"></param>
    public void DirectValueSet(float value) 
    {
        this.Value = value;
    }
    public void RecalculateValue()
    {
        float baseInc = baseIncrease.Sum();
        if (baseInc == 0) 
        {
            baseInc = 1f;
        }
        float baseMul = baseMultiplier.Aggregate(1f, (total, next) => total * next);
        valueBeforeConversion = (BaseValue + baseAdded.Sum()) * baseInc * baseMul;
        Value = valueBeforeConversion;
    }
    public void AddBaseAdded(float value)
    {
        baseAdded.Add(value);
        RefreshStat();
    }
    public void RemoveBaseAdded(float value)
    {
        baseAdded.Remove(value);
        RefreshStat();
    }
    public void AddIncrease(float value)
    {
        baseIncrease.Add(value);
        RefreshStat();
    }
    public void RemoveIncrease(float value)
    {
        baseIncrease.Remove(value);
        RefreshStat();
    }
    public void AddMultiplier(float value)
    {
        baseMultiplier.Add(value);
        RefreshStat();
    }
    public void RemoveMultiplier(float value)
    {
        baseMultiplier.Remove(value);
        RefreshStat();
    }
    public void SetBaseValue(float value)
    {
        BaseValue = value;
        RefreshStat();
    }
    private void RefreshStat() 
    {
        if(conversion.Count != 0)
        {
            purgeScaledConversionList();
            RecalculateValue();
            updateScaledConversions();
            updateConversionDependants();
            deductConversionFromValue();
        }
        else 
        {
            RecalculateValue();
        }
    }
    private void deductConversionFromValue() 
    {
        foreach(var item in conversion)
        {
            float value = scaledConversion[item.Key.StatType];
            Value -= value;
        }
    }
    public void AddConversion(Stat stat, float percentage)
    {
        //Add new conversion
        conversion.Add(stat, percentage);
        //Update Scaled Conversions list to populate it with values for Update()
        updateScaledConversions();
        RefreshStat();

    }
    private void updateConversionDependants()
    {
        foreach (var item in conversion)
        {
            float value = scaledConversion[item.Key.StatType];
            item.Key.AddBaseAdded(value);
        }
    }
    private void updateScaledConversions() 
    {
        //If more than 100% of stat is converted, GetScale to normalize to 100%
        float scale = getConversionScale();

        //Fill new Scaled Conversions list
        Dictionary<StatType, float> newConversions = new Dictionary<StatType, float>();
        foreach (var item in conversion)
        {
            newConversions.Add(item.Key.StatType, this.valueBeforeConversion * (item.Value * scale));
        }
        //Set new Scaled Conversions list as main list
        this.scaledConversion = newConversions;
    }
    private void purgeScaledConversionList() 
    {
        if (conversion.Count == 0) return;
        foreach (var item in conversion)
        {
            item.Key.RemoveBaseAdded(this.scaledConversion[item.Key.StatType]);
            this.scaledConversion[item.Key.StatType] = 0;
        }
    }
    public void RemoveConversion(Stat stat, float value)
    {
        if (conversion[stat].IsUnityNull()) return;

        //Remove values added by conversion for other stats
        purgeScaledConversionList();

        //Update conversion value
        this.conversion[stat] -= value;
        if (this.conversion[stat] == 0) 
        {
            this.conversion.Remove(stat);
        }
        RefreshStat();
    }
    private float getConversionScale()
    {
        float totalPercentage = conversion.Values.Sum();
        return totalPercentage > 1.0f ? 1.0f / totalPercentage : 1;
    }
    public void AddAsExtra(Stat stat, float value)
    {
        asExtra.Add(stat, new List<float>() { value, this.valueBeforeConversion*value});
        stat.AddBaseAdded(this.valueBeforeConversion * value);
        RefreshStat();
    }
    public void RemoveAsExtra(Stat stat, float value) 
    {
        stat.RemoveBaseAdded(this.asExtra[stat][1]);
        this.asExtra[stat][0] -= value;
        if (this.asExtra[stat][0] == 0) 
        {
            asExtra.Remove(stat);
        }
        RefreshStat();
    }
}
