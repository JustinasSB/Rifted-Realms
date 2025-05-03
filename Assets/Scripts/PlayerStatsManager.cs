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
