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
        playerStatsManager.playerStats.Stats[StatType.Life].AddBaseAdded((float)10 * (level / 5));
        playerStatsManager.playerStats.Stats[StatType.Mana].AddBaseAdded(3f);
    }
}
