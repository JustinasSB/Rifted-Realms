using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat : ISerializationCallbackReceiver
{
    public string Name { get; }
    public string Description { get; }
    public float BaseValue { get; private set; }
    [NonSerialized] private List<float> baseAdded = new List<float>();
    [NonSerialized] private List<float> baseIncrease = new List<float>();
    [NonSerialized] private List<float> baseMultiplier = new List<float>();
    public StatType StatType;

    //Invokable event for value changes, listeners can update labels on change instead of reading values every frame
    public event Action<float> OnValueChanged;

    //Stores Stat and the percentage of this.Value converted into held Stat
    [NonSerialized] private Dictionary<Stat, float> conversion = new Dictionary<Stat, float>();

    //Stores Stat type and the amount of this.Value converted into held Stat type
    [NonSerialized] private Dictionary<StatType, float> scaledConversion = new Dictionary<StatType, float>();
    [NonSerialized] private Dictionary<Stat, List<float>> asExtra = new Dictionary<Stat, List<float>>();
    public float Value;
    private float valueBeforeConversion;
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
        OnValueChanged?.Invoke(Value);
    }
    /// <summary>
    /// For use in regeneration, sets stat value directly to avoid unnecessary calculations
    /// </summary>
    /// <param name="value"></param>
    public void DirectValueSet(float value) 
    {
        this.Value = value;
        OnValueChanged?.Invoke(this.Value);
    }
    public void RecalculateValue()
    {
        float baseInc = (1f + baseIncrease.Sum());
        float baseMul = baseMultiplier.Aggregate(1f, (total, next) => total * next);
        valueBeforeConversion = (BaseValue + baseAdded.Sum()) * baseInc * baseMul;
        Value = valueBeforeConversion;
        //OnValueChanged?.Invoke(this.Value);
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
        if (asExtra.Count != 0)
        {
            recalculateAsExtra();
        }
        OnValueChanged?.Invoke(Value);
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
        //if (conversion[stat] == null) return;

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
    private void recalculateAsExtra()
    {
        foreach (var item in asExtra)
        {
            float percentage = item.Value[0];
            RemoveAsExtra(item.Key, percentage, false);
            AddAsExtra(item.Key, percentage, false);
            item.Value[2] = this.valueBeforeConversion;
        }
    }
    public void AddAsExtra(Stat stat, float value, bool allowModifyingDict = true)
    {
        if (allowModifyingDict && !asExtra.ContainsKey(stat))
        {
            asExtra.Add(stat, new List<float>() { value, this.valueBeforeConversion * value, this.valueBeforeConversion });
        }
        else
        {
            asExtra[stat][0] += value;
            asExtra[stat][1] += this.valueBeforeConversion * value;
        }
        stat.AddBaseAdded(this.valueBeforeConversion * value);
    }
    public void RemoveAsExtra(Stat stat, float value, bool allowModifyingDict = true) 
    {
        float toRemove = this.asExtra[stat][2] * value;
        stat.RemoveBaseAdded(toRemove);
        this.asExtra[stat][1] -= toRemove;
        this.asExtra[stat][0] -= value;
        if (allowModifyingDict && this.asExtra[stat][0] == 0) 
        {
            asExtra.Remove(stat);
        }
    }
    public float GetTotalIncrease()
    {
        return baseIncrease.Sum();
    }
    public float GetTotalMultiplier()
    {
        return baseMultiplier.Aggregate(1f, (total, next) => total * next);
    }
    public float GetTotalAdd()
    {
        return BaseValue + baseAdded.Sum();
    }
    public void PurgeModifierLists()
    {
        this.baseAdded.Clear();
        this.baseIncrease.Clear();
        this.baseMultiplier.Clear();
    }
    public void PurgeMultiplierList()
    {
        this.baseMultiplier.Clear();
    }
    public void ModifyStat(OperationType operation, float value)
    {
        switch (operation)
        {
            case OperationType.Add:
                AddBaseAdded(value);
                break;
            case OperationType.Increase:
                AddIncrease(value);
                break;
            case OperationType.Multiply:
                AddMultiplier(value);
                break;
            case OperationType.AddRemove:
                RemoveBaseAdded(value);
                break;
            case OperationType.IncreaseRemove:
                RemoveIncrease(value);
                break;
            case OperationType.MultiplyRemove:
                RemoveMultiplier(value);
                break;
            case OperationType.SetBase:
                SetBaseValue(value);
                break;
        }
    }
    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        if (BaseValue == 0f && Value != 0f)
            BaseValue = Value;
        baseAdded = new List<float>();
        baseIncrease = new List<float>();
        baseMultiplier = new List<float>();
        conversion = new Dictionary<Stat, float>();
        scaledConversion = new Dictionary<StatType, float>();
        asExtra = new Dictionary<Stat, List<float>>();
        RecalculateValue();
    }
    public Stat Clone()
    {
        Stat clone = new Stat(this.BaseValue, this.StatType);
        clone.baseAdded = new List<float>(this.baseAdded);
        clone.baseIncrease = new List<float>(this.baseIncrease);
        clone.baseMultiplier = new List<float>(this.baseMultiplier);
        clone.conversion = new Dictionary<Stat, float>(this.conversion);
        clone.scaledConversion = new Dictionary<StatType, float>(this.scaledConversion);
        clone.asExtra = new Dictionary<Stat, List<float>>(asExtra);
        clone.RefreshStat();
        return clone;
    }
}
