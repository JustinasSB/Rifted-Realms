using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] Slider Health;
    [SerializeField] Slider Mana;
    [SerializeField] Slider Energy;
    private Stat[] resources;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>()?.playerStats != null);
        playerStatsManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>();
        resources = new[]
        {
            playerStatsManager.playerStats.GetStat(StatType.Life),
            playerStatsManager.playerStats.GetStat(StatType.Mana),
            playerStatsManager.playerStats.GetStat(StatType.Energy),
            playerStatsManager.playerStats.GetStat(StatType.CurrentLife),
            playerStatsManager.playerStats.GetStat(StatType.CurrentMana),
            playerStatsManager.playerStats.GetStat(StatType.CurrentEnergy),
        };
    }
    private void Update()
    {
        if (resources == null) return;
        Health.value = resources[3].Value / resources[0].Value;
        Mana.value = resources[4].Value / resources[1].Value;
    }
}
