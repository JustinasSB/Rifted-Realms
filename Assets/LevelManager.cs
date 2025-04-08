using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour 
{
    public Level level;
    private PlayerStatsManager playerStatsManager;
    void Start() 
    {
        level = new Level();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        level.OnLevelUp += HandleLevelUp;
    }
    private void HandleLevelUp(int level)
    {
        Stat Life = PlayerStatsManager.playerStats.Stats[StatType.Life];
        Stat Mana = PlayerStatsManager.playerStats.Stats[StatType.Mana];
        Life.AddBaseAdded((float)10 * (level / 5));
        Mana.AddBaseAdded(3f);

        //fully recover player resources
        PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(Life.Value);
        PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(Mana.Value);
        PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.Energy].Value);
    }
}
