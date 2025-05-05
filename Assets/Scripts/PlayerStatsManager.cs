using UnityEngine;

[System.Serializable]
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStats playerStats;
    void Start()
    {
        playerStats = new PlayerStats();
        playerStats.Stats[StatType.Strength].AddConversion(playerStats.Stats[StatType.Intelligence], 100f);
        playerStats.Stats[StatType.Intelligence].AddConversion(playerStats.Stats[StatType.Wisdom], 100f);
        playerStats.Stats[StatType.Wisdom].AddConversion(playerStats.Stats[StatType.Constitution], 100f);
        playerStats.Stats[StatType.Constitution].AddConversion(playerStats.Stats[StatType.Dexterity], 100f);
        playerStats.Stats[StatType.Dexterity].AddConversion(playerStats.Stats[StatType.Charisma], 100f);
    }
}

