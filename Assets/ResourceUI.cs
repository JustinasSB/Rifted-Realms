using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] Slider Health;
    [SerializeField] Slider Mana;
    [SerializeField] Slider Energy;
    [SerializeField] TextMeshProUGUI HealthLabel;
    [SerializeField] TextMeshProUGUI ManaLabel;
    [SerializeField] TextMeshProUGUI EnergyLabel;
    private Stat[] resources;

    private void Start()
    {
        resources = new[]
        {
            PlayerStatsManager.playerStats.GetStat(StatType.Life),
            PlayerStatsManager.playerStats.GetStat(StatType.Mana),
            PlayerStatsManager.playerStats.GetStat(StatType.Energy),
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentLife),
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentMana),
            PlayerStatsManager.playerStats.GetStat(StatType.CurrentEnergy),
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
