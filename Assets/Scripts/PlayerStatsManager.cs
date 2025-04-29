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
    }
}
