using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Stat[] resources;
    void Start()
    {
        PlayerStatsManager playerStatsManager = GetComponent<PlayerStatsManager>();
        resources = new[]
        {
            playerStatsManager.playerStats.GetStat(StatType.Life),                          //0
            playerStatsManager.playerStats.GetStat(StatType.Mana),                          //1
            playerStatsManager.playerStats.GetStat(StatType.Energy),                        //2
            playerStatsManager.playerStats.GetStat(StatType.CurrentLife),                   //3
            playerStatsManager.playerStats.GetStat(StatType.CurrentMana),                   //4
            playerStatsManager.playerStats.GetStat(StatType.CurrentEnergy),                 //5
            playerStatsManager.playerStats.GetStat(StatType.RegenerationPercentage),        //6
            playerStatsManager.playerStats.GetStat(StatType.RegenerationFlat),              //7
            playerStatsManager.playerStats.GetStat(StatType.ManaRegenerationPercentage),    //8
            playerStatsManager.playerStats.GetStat(StatType.ManaRegenerationFlat),          //9
            playerStatsManager.playerStats.GetStat(StatType.EnergyRecharge),                //10
            playerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationPercentage),  //11
            playerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationFlat)         //11
        };
    }

    void Update()
    {
        RegenerateLife();
        RegenerateMana();
        RegenerateEnergy();
    }
    private void RegenerateLife() 
    {
        float total = resources[0].Value;
        float current = resources[3].Value;
        if (total > current) 
        {
            current += resources
            if (current > total) 
            {
                resources[3].DirectValueSet(total);
            }
        }
    }
    private void RegenerateMana()
    {
        
    }
    private void RegenerateEnergy()
    {
        
    }
}
