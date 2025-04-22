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
            { StatType.Strength, new Stat("Strength", "Undescribed", 10f, StatType.Strength) },
            { StatType.Dexterity, new Stat("Dexterity", "Undescribed", 10f, StatType.Dexterity) },
            { StatType.Intelligence, new Stat("Intelligence", "Undescribed", 10f, StatType.Intelligence) },
            { StatType.MovementSpeed, new Stat("Movement Speed", "Undescribed", 5f, StatType.MovementSpeed) },
            { StatType.Armour, new Stat("Armour", "Undescribed", 0f, StatType.Armour) },
            { StatType.Evasion, new Stat("Evasion", "Undescribed", 0f, StatType.Evasion) },
            { StatType.Energy, new Stat("Energy", "Undescribed", 100f, StatType.Energy) },
            { StatType.CurrentEnergy, new Stat("Current Energy", "Undescribed", 100f, StatType.CurrentEnergy) },
            { StatType.Life, new Stat("Life", "Undescribed", 100f, StatType.Life) },
            { StatType.CurrentLife, new Stat("Current Life", "Undescribed", 100f, StatType.CurrentLife) },
            { StatType.Mana, new Stat("Mana", "Undescribed", 50f, StatType.Mana) },
            { StatType.CurrentMana, new Stat("Current Mana", "Undescribed", 50f, StatType.CurrentMana) },
            { StatType.RegenerationPercentage, new Stat("Regeneration Percentage", "Undescribed", 0.01f, StatType.RegenerationPercentage) },
            { StatType.RegenerationFlat, new Stat("Regeneration Flat", "Undescribed", 5f, StatType.RegenerationFlat) },
            { StatType.ManaRegenerationPercentage, new Stat("Mana Regeneration Percentage", "Undescribed", 0.01f, StatType.ManaRegenerationPercentage) },
            { StatType.ManaRegenerationFlat, new Stat("Mana Regeneration Flat", "Undescribed", 1f, StatType.ManaRegenerationFlat) },
            { StatType.EnergyRecharge, new Stat("Energy Recharge", "Undescribed", 0.3f, StatType.EnergyRecharge) },
            { StatType.EnergyRegenerationPercentage, new Stat("Energy Regeneration Percentage", "Undescribed", 0f, StatType.EnergyRegenerationPercentage) },
            { StatType.EnergyRegenerationFlat, new Stat("Energy Regeneration Flat", "Undescribed", 0f, StatType.EnergyRegenerationFlat) },
            { StatType.AttackSpeed, new Stat("Attack Speed", "Undescribed", 1f, StatType.AttackSpeed) },
            { StatType.CastingSpeed, new Stat("Casting Speed", "Undescribed", 1f, StatType.CastingSpeed) },
            { StatType.Wisdom, new Stat ("Wisdom", "Undescribed", 10f, StatType.Wisdom)},
            { StatType.Constitution, new Stat ("Constitution", "Undescribed", 10f, StatType.Constitution)},
            { StatType.Charisma, new Stat ("Charisma", "Undescribed", 10f, StatType.Charisma)},
            { StatType.AnimationSpeed, new Stat ("Animation Speed", "Undescribed", 1f, StatType.AnimationSpeed)},
            { StatType.FireResistance, new Stat ("Fire Resistance", "Undescribed", 0f, StatType.FireResistance)},
            { StatType.WaterResistance, new Stat ("Water Resistance", "Undescribed", 0f, StatType.WaterResistance)},
            { StatType.AirResistance, new Stat ("Air Resistance", "Undescribed", 0f, StatType.AirResistance)},
            { StatType.RadiantResistance, new Stat ("Radiant Resistance", "Undescribed", 0f, StatType.RadiantResistance)},
            { StatType.PoisonResistance, new Stat ("Poison Resistance", "Undescribed", 0f, StatType.PoisonResistance)},
            { StatType.PhysicalDamage, new Stat ("Physical Damage", "Undescribed", 1f, StatType.PhysicalDamage)},
            { StatType.Damage, new Stat ("Damage", "Undescribed", 1f, StatType.Damage)},
            { StatType.Interval, new Stat ("Interval", "Undescribed", 0f, StatType.Interval)}
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
    public void ModifyStat(StatType statType, OperationType operation, float value, StatType from)
    {
        if (!Stats.ContainsKey(statType)) return;
        switch (operation)
        {
            case OperationType.Convert:
                Stats[from].AddConversion(Stats[statType], value);
                break;
            case OperationType.ConvertRemove:
                Stats[from].RemoveConversion(Stats[statType], value);
                break;
            case OperationType.Extra:
                Stats[from].AddAsExtra(Stats[statType], value);
                break;
            case OperationType.ExtraRemove:
                Stats[from].RemoveAsExtra(Stats[statType], value);
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
