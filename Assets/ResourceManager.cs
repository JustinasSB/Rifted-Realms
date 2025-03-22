using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;
    private Stat[] resources;
    private bool canRecharge = false;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GetComponent<PlayerStatsManager>()?.playerStats != null);
        playerStatsManager = GetComponent<PlayerStatsManager>();
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
            playerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationFlat)         //12
        };
    }

    void Update()
    {
        // PlayerStatsManager doesn't initialize immediately, the updates have to be delayed until the component populates
        if (playerStatsManager == null) return;
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
