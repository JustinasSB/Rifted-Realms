using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerStats
{
    public Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();

    private Dictionary<string, List<Action<string, float>>> statListeners = new Dictionary<string, List<Action<string, float>>>();
    public PlayerStats()
    {
        Stats = new Dictionary<StatType, Stat>
        {
            { StatType.Strength, new Stat("Strength", "Undescribed", 10f) },
            { StatType.Dexterity, new Stat("Dexterity", "Undescribed", 10f) },
            { StatType.Intelligence, new Stat("Intelligence", "Undescribed", 10f) },
            { StatType.MovementSpeed, new Stat("MovementSpeed", "Undescribed", 5f) },
            { StatType.Armour, new Stat("Armour", "Undescribed", 0f) },
            { StatType.Evasion, new Stat("Evasion", "Undescribed", 0f) },
            { StatType.Energy, new Stat("Energy", "Undescribed", 100f) },
            { StatType.CurrentEnergy, new Stat("CurrentEnergy", "Undescribed", 100f) },
            { StatType.Life, new Stat("Life", "Undescribed", 100f) },
            { StatType.CurrentLife, new Stat("CurrentLife", "Undescribed", 100f) },
            { StatType.Mana, new Stat("Mana", "Undescribed", 50f) },
            { StatType.CurrentMana, new Stat("CurrentMana", "Undescribed", 50f) },
            { StatType.RegenerationPercentage, new Stat("RegenerationPercentage", "Undescribed", 1f) },
            { StatType.RegenerationFlat, new Stat("RegenerationFlat", "Undescribed", 5f) },
            { StatType.ManaRegenerationPercentage, new Stat("ManaRegenerationPercentage", "Undescribed", 1f) },
            { StatType.ManaRegenerationFlat, new Stat("ManaRegenerationFlat", "Undescribed", 1f) },
            { StatType.EnergyRecharge, new Stat("EnergyRecharge", "Undescribed", 30f) },
            { StatType.EnergyRegenerationPercentage, new Stat("EnergyRegenerationPercentage", "Undescribed", 0f) },
            { StatType.EnergyRegenerationFlat, new Stat("EnergyRegenerationFlat", "Undescribed", 0f) }
        };
    }
    public void ModifyStat(StatType statType, OperationType operation, float value) {
        if (!Stats.ContainsKey(statType)) return;

        switch (operation) {
            case OperationType.Add:
                Stats[statType].AddBaseAdded(value);
                break;
            case OperationType.Increase:
                Stats[statType].AddIncrease(value);
                break;
            case OperationType.Multiply:
                Stats[statType].AddMultiplier(value);
                break;
            case OperationType.AddRemove:
                Stats[statType].RemoveBaseAdded(value);
                break;
            case OperationType.IncreaseRemove:
                Stats[statType].RemoveIncrease(value);
                break;
            case OperationType.MultiplyRemove:
                Stats[statType].RemoveMultiplier(value);
                break;
            case OperationType.SetBase:
                Stats[statType].SetBaseValue(value);
                break;

        }

    }
    public Stat GetStat(StatType statType)
    {
        return Stats.TryGetValue(statType, out Stat stat) ? stat : null;
    }

    public void AddStatListener(string statType, Action<string, float> listener)
    {
        if (!statListeners.ContainsKey(statType))
        {
            statListeners[statType] = new List<Action<string, float>>();
        }
        statListeners[statType].Add(listener);
    }
    private void NotifyListeners(string statType, float newValue)
    {
        if (statListeners.ContainsKey(statType))
        {
            foreach (var listener in statListeners[statType])
            {
                listener.Invoke(statType, newValue);
            }
        }
    }
    private void OnDexterityChanged(object sender, EventArgs e)
    {

    }
}
