using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
        resources[0].OnValueChanged += value => UpdateHealth();
        resources[1].OnValueChanged += value => UpdateMana();
        resources[2].OnValueChanged += value => UpdateEnergy();
        resources[3].OnValueChanged += value => UpdateHealth();
        resources[4].OnValueChanged += value => UpdateMana();
        resources[5].OnValueChanged += value => UpdateEnergy();
        UpdateHealth();
        UpdateMana();
        UpdateEnergy();
    }
    private void UpdateHealth()
    {
        if (resources[0].Value != 0)
        {
            if (resources[3].Value > resources[0].Value) resources[3].DirectValueSet(resources[0].Value);
            Health.value = resources[3].Value / resources[0].Value;
            HealthLabel.text = $"{Math.Round(resources[3].Value)} / {Math.Round(resources[0].Value)}";
        }
        else
        {
            Health.value = 0;
            HealthLabel.text = string.Format("{0} / {1}", 0, 0);
        }
    }
    private void UpdateMana()
    {
        if (resources[1].Value != 0)
        {
            if (resources[4].Value > resources[1].Value) resources[4].DirectValueSet(resources[1].Value);
            Mana.value = resources[4].Value / resources[1].Value;
            ManaLabel.text = string.Format("{0} / {1}", Math.Round(resources[4].Value), Math.Round(resources[1].Value));
        }
        else
        {
            Mana.value = 0;
            ManaLabel.text = string.Format("{0} / {1}", 0, 0);
        }
    }
    private void UpdateEnergy()
    {
        if (resources[2].Value != 0)
        {
            if (resources[5].Value > resources[2].Value) resources[5].DirectValueSet(resources[2].Value);
            Energy.value = resources[5].Value / resources[2].Value;
            EnergyLabel.text = string.Format("{0} / {1}", Math.Round(resources[5].Value), Math.Round(resources[2].Value));
        }
        else
        {
            Energy.value = 0;
            EnergyLabel.text = string.Format("{0} / {1}", 0, 0);
        }
    }
}
