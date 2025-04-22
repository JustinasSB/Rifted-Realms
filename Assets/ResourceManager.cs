using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<StatType, Stat> stats;
    private bool canRecharge = false;
    private void Start()
    {
        stats = PlayerStatsManager.playerStats.Stats;
    }

    void Update()
    {
        Regenerate(stats[StatType.Life], stats[StatType.CurrentLife], stats[StatType.RegenerationPercentage], stats[StatType.RegenerationFlat]);
        Regenerate(stats[StatType.Mana], stats[StatType.CurrentMana], stats[StatType.ManaRegenerationPercentage], stats[StatType.ManaRegenerationFlat]);
        Regenerate(stats[StatType.Energy], stats[StatType.CurrentEnergy], stats[StatType.EnergyRegenerationPercentage], stats[StatType.EnergyRegenerationFlat]);
        if (canRecharge)
            Regenerate(stats[StatType.Energy], stats[StatType.CurrentEnergy], stats[StatType.EnergyRecharge]);
    }
    private void Regenerate(Stat Total, Stat Current, Stat Percentage, Stat Flat) 
    {
        float total = Total.Value;
        float current = Current.Value;
        if (total == current) return;
        if (total > current) 
        {
            current += (total * Percentage.Value + Flat.Value) * Time.deltaTime;
            if (current > total)
            {
                Current.DirectValueSet(total);
            }
            else 
            {
                Current.DirectValueSet(current);
            }
        }
    }
    private void Regenerate(Stat Total, Stat Current, Stat Percentage)
    {
        float total = Total.Value;
        float current = Current.Value;
        if (total == current) return;
        if (total > current)
        {
            current += total * Percentage.Value;
            if (current > total)
            {
                Current.DirectValueSet(total);
            }
            else
            {
                Current.DirectValueSet(current);
            }
        }
    }
}
