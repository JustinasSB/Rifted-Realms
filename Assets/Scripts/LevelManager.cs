﻿using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour 
{
    public static Level level;
    void Start() 
    {
        level = new Level();
        level.OnLevelUp += HandleLevelUp;
    }
    private void HandleLevelUp(int level)
    {
        Stat Life = PlayerStatsManager.playerStats.Stats[StatType.Life];
        Stat Mana = PlayerStatsManager.playerStats.Stats[StatType.Mana];
        Life.AddBaseAdded((float)10 * (level / 5));
        Mana.AddBaseAdded(3f);
        LevelManager.level.IncrementPassivePoints();
        //fully recover player resources
        PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(Life.Value);
        PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(Mana.Value);
        PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.Energy].Value);
    }
}
