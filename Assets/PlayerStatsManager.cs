using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStats playerStats;
    void Start()
    {
        playerStats = new PlayerStats();

        playerStats.AddStatListener("Dexterity", (statName, newValue) =>
        {
            Debug.Log(statName.ToString() + " adjusted to " + newValue.ToString());
        });
        playerStats.AddStatListener("MovementSpeed", (statName, newValue) =>
        {
            Debug.Log(statName.ToString() + " adjusted to " + newValue.ToString());
        });

        playerStats.Stats[StatType.Dexterity].SetBaseValue(300);
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " adjusted to " + playerStats.Stats[StatType.Dexterity].Value.ToString());
        playerStats.Stats[StatType.MovementSpeed].SetBaseValue(20);
        Debug.Log(playerStats.Stats[StatType.MovementSpeed].Name + " adjusted to " + playerStats.Stats[StatType.MovementSpeed].Value.ToString());
        playerStats.Stats[StatType.MovementSpeed].AddConversion(playerStats.GetStat(StatType.Dexterity), 1.75f);
        Debug.Log(playerStats.Stats[StatType.MovementSpeed].Name + " adjusted to " + playerStats.Stats[StatType.MovementSpeed].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " adjusted to " + playerStats.Stats[StatType.Dexterity].Value.ToString());

        playerStats.Stats[StatType.MovementSpeed].RemoveConversion(playerStats.GetStat(StatType.Dexterity), 1.0f);
        Debug.Log(playerStats.Stats[StatType.MovementSpeed].Name + " adjusted to " + playerStats.Stats[StatType.MovementSpeed].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " adjusted to " + playerStats.Stats[StatType.Dexterity].Value.ToString());

        //Debug.Log(playerStats.Stats[StatType.Strength].Name + " is " + playerStats.Stats[StatType.Strength].Value.ToString());
        //Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " is " + playerStats.Stats[StatType.Intelligence].Value.ToString());
        //playerStats.Stats[StatType.Strength].AddConversion(playerStats.GetStat(StatType.Intelligence), 1.0f);
        //Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());
        //playerStats.Stats[StatType.Strength].SetBaseValue(300);
        //Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());

        Debug.Log(playerStats.Stats[StatType.Strength].Name + " is " + playerStats.Stats[StatType.Strength].Value.ToString());
        playerStats.Stats[StatType.Dexterity].AddConversion(playerStats.GetStat(StatType.Strength), 1.0f);
        Debug.Log(playerStats.Stats[StatType.Strength].Name + " adjusted to " + playerStats.Stats[StatType.Strength].Value.ToString());
        playerStats.Stats[StatType.Strength].AddConversion(playerStats.GetStat(StatType.Intelligence), 1.0f);
        Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());
        playerStats.Stats[StatType.Dexterity].RemoveConversion(playerStats.GetStat(StatType.Strength), 1.0f);
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " is " + playerStats.Stats[StatType.Dexterity].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Strength].Name + " is " + playerStats.Stats[StatType.Strength].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());
        playerStats.Stats[StatType.Dexterity].AddAsExtra(playerStats.GetStat(StatType.Strength), 10f);
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " is " + playerStats.Stats[StatType.Dexterity].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Strength].Name + " is " + playerStats.Stats[StatType.Strength].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());
        playerStats.Stats[StatType.Dexterity].RemoveAsExtra(playerStats.GetStat(StatType.Strength), 10f);
        Debug.Log(playerStats.Stats[StatType.Dexterity].Name + " is " + playerStats.Stats[StatType.Dexterity].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Strength].Name + " is " + playerStats.Stats[StatType.Strength].Value.ToString());
        Debug.Log(playerStats.Stats[StatType.Intelligence].Name + " adjusted to " + playerStats.Stats[StatType.Intelligence].Value.ToString());
    }
}
