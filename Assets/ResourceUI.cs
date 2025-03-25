using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] Slider Health;
    [SerializeField] Slider Mana;
    [SerializeField] Slider Energy;
    [SerializeField] TextMeshProUGUI HealthLabel;
    [SerializeField] TextMeshProUGUI ManaLabel;
    [SerializeField] TextMeshProUGUI EnergyLabel;
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
        if (resources[0].Value != 0)
        {
            Health.value = resources[3].Value / resources[0].Value;
            HealthLabel.text = string.Format("{0} / {1}", Math.Round(resources[3].Value), resources[0].Value);
        }
        if (resources[1].Value != 0)
        {
            Mana.value = resources[4].Value / resources[1].Value;
            ManaLabel.text = string.Format("{0} / {1}", Math.Round(resources[4].Value), resources[1].Value);
        }
        if (resources[2].Value != 0)
        {
            Energy.value = resources[5].Value / resources[2].Value;
            EnergyLabel.text = string.Format("{0} / {1}", Math.Round(resources[5].Value), resources[2].Value);
        }
        else 
        {
            Energy.value = 0;
            EnergyLabel.text = string.Format("{0} / {1}", 0, 0);
        }
    }
}
