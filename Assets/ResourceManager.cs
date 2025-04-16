using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Stat[] resources;
    private bool canRecharge = false;
    private void Start()
    {
        resources = new[]
        {
            PlayerStatsManager.playerStats.GetStat(StatType.Life),                          //0
            PlayerStatsManager.playerStats.GetStat(StatType.Mana),                          //1
            PlayerStatsManager.playerStats.GetStat(StatType.Energy),                        //2
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentLife),                   //3
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentMana),                   //4
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentEnergy),                 //5
            PlayerStatsManager.playerStats.GetStat(StatType.RegenerationPercentage),        //6
            PlayerStatsManager.playerStats.GetStat(StatType.RegenerationFlat),              //7
            PlayerStatsManager.playerStats.GetStat(StatType.ManaRegenerationPercentage),    //8
            PlayerStatsManager.playerStats.GetStat(StatType.ManaRegenerationFlat),          //9
            PlayerStatsManager.playerStats.GetStat(StatType.EnergyRecharge),                //10
            PlayerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationPercentage),  //11
            PlayerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationFlat)         //12
        };
    }

    void Update()
    {
        Regenerate(resources[0], resources[3], resources[6], resources[7]);
        Regenerate(resources[1], resources[4], resources[8], resources[9]);
        Regenerate(resources[2], resources[5], resources[11], resources[12]);
        if (canRecharge)
            Regenerate(resources[2], resources[5], resources[10]);
    }
    private void Regenerate(Stat Total, Stat Current, Stat Percentage, Stat Flat) 
    {
        float total = Total.Value;
        float current = Current.Value;
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
