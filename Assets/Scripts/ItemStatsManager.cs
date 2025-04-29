using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ItemStatsManager
{
    public Dictionary<StatType, Stat> List = new();
    public void ModifyStat(StatType statType, OperationType operation, float value)
    {
        if (!List.ContainsKey(statType)) return;
        switch (operation)
        {
            case OperationType.Add:
                List[statType].AddBaseAdded(value);
                break;
            case OperationType.Increase:
                List[statType].AddIncrease(value);
                break;
            case OperationType.Multiply:
                List[statType].AddMultiplier(value);
                break;
            case OperationType.AddRemove:
                List[statType].RemoveBaseAdded(value);
                break;
            case OperationType.IncreaseRemove:
                List[statType].RemoveIncrease(value);
                break;
            case OperationType.MultiplyRemove:
                List[statType].RemoveMultiplier(value);
                break;
            case OperationType.SetBase:
                List[statType].SetBaseValue(value);
                break;
        }
    }
    public void ModifyStat(StatType statType, OperationType operation, float value, StatType from)
    {
        if (!List.ContainsKey(statType)) return;
        switch (operation)
        {
            case OperationType.Convert:
                List[from].AddConversion(List[statType], value);
                break;
            case OperationType.ConvertRemove:
                List[from].RemoveConversion(List[statType], value);
                break;
            case OperationType.Extra:
                List[from].AddAsExtra(List[statType], value);
                break;
            case OperationType.ExtraRemove:
                List[from].RemoveAsExtra(List[statType], value);
                break;
        }
    }
}
