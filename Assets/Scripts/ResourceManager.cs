using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<StatType, Stat> stats;
    private bool canRecharge = false;
    private float CummulativeEvasion = 0;
    [SerializeField] private GameObject GameOverPanel;
    private void Start()
    {
        stats = PlayerStatsManager.playerStats.Stats;
        CummulativeEvasion = stats[StatType.Evasion].Value;
    }

    public void Update()
    {
        if (DeathManager.Dead) return;
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
            current += total * Percentage.Value * Time.deltaTime;
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
    public void TakeDamage(float value, StatType DamageType)
    {
        switch(DamageType)
        {
            case StatType.PhysicalDamage:
                if (Evade(value)) return;
                float armour = stats[StatType.Armour].Value;
                float damageReduction = armour / (50 * value);
                // limit to 90% damage reduction
                damageReduction = Mathf.Clamp(damageReduction, 0f, 0.9f);
                value *= (1f - damageReduction);
                break;
        }
        Stat Energy = stats[StatType.CurrentEnergy];
        float energy = Energy.Value - value;
        if (energy > 0)
        {
            Energy.DirectValueSet(energy);
            return;
        }
        else
        {
            float toRemove = value + energy;
            value -= toRemove;
            Energy.DirectValueSet(0);
        }
        Stat Life = stats[StatType.CurrentLife];
        Life.DirectValueSet(Life.Value - value);
        if (Life.Value <= 0)
        {
            DeathManager.Dead = true;
            GameOverPanel.SetActive(true);
            GameOver panel = GameOverPanel.GetComponent<GameOver>();
            panel.isOpen = true;
        }
    }
    private bool Evade(float value)
    {
        CummulativeEvasion -= value*10;
        if (CummulativeEvasion < 0)
        {
            CummulativeEvasion = stats[StatType.Evasion].Value;
            return false;
        }
        return true;
    }
}
