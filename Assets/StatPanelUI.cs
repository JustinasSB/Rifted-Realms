using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class StatPanelUI : MonoBehaviour, IUIToggleable
{
    [SerializeField] private Button offense;
    [SerializeField] private Button defense;
    [SerializeField] private Button utility;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Transform labelContainer;
    [SerializeField] private GameObject labelPrefab;
    [SerializeField] private GameObject[] AttributeNumerics;
    private List<Stat> statsToUse;
    private List<Stat> Attributes = new List<Stat>();
    private List<Stat> subscribedStats = new();
    private List<(string, float, bool)> valuesToDisplay = new List<(string, float, bool)>();
    private int pageToLoad = 0;

    void Start()
    {
        labelPrefab.SetActive(false);
        loadStats();
        loadAttributes();
        loadDisplayValues();
        offense.onClick.AddListener(() => SetValue(0));
        defense.onClick.AddListener(() => SetValue(1));
        utility.onClick.AddListener(() => SetValue(2));
        updateAttributes();
        updateLabels();
    }
    private void SetValue(int value)
    {
        pageToLoad = value;
        destroyLabels();
        loadStats();
        loadDisplayValues();
        updateLabels();
    }
    private void loadAttributes()
    {
        Stat[] attributeStats = {
            PlayerStatsManager.playerStats.GetStat(StatType.Strength),
            PlayerStatsManager.playerStats.GetStat(StatType.Intelligence),
            PlayerStatsManager.playerStats.GetStat(StatType.Wisdom),
            PlayerStatsManager.playerStats.GetStat(StatType.Constitution),
            PlayerStatsManager.playerStats.GetStat(StatType.Dexterity),
            PlayerStatsManager.playerStats.GetStat(StatType.Charisma)
        };
        for (int i = 0; i < attributeStats.Length; i++)
        {
            var stat = attributeStats[i];
            Attributes.Add(stat);
            SubscribeToStat(stat, (float value) => OnAttributeChanged(i, value), false);
        }
    }
    private void SubscribeToStat(Stat stat, Action<float> callback, bool addtoList = true)
    {
        stat.OnValueChanged += value => callback.Invoke(value);
        if (!addtoList) return;
        subscribedStats.Add(stat);
    }
    private void OnAttributeChanged(int i, float value)
    {
        AttributeNumerics[i].gameObject.GetComponent<TMP_Text>().text = value.ToString();
    }
    private void updateAttributes()
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            AttributeNumerics[i].gameObject.GetComponent<TMP_Text>().text = Attributes[i].Value.ToString();
        }
    }
    private void loadStats() 
    {
        foreach (var stat in subscribedStats)
        {
            stat.OnValueChanged -= value => OnStatChanged();
        }
        subscribedStats.Clear();
        List<Stat> newStatList = new List<Stat>();
        switch (pageToLoad)
        {
            case 0:
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.AttackSpeed));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.CastingSpeed));
                break;
            case 1:
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.Armour));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.Evasion));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.RegenerationPercentage));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.RegenerationFlat));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.Life));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.EnergyRecharge));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationPercentage));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationFlat));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.Energy));
                break;
            case 2:
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.MovementSpeed));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.AnimationSpeed));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.ManaRegenerationPercentage));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.ManaRegenerationFlat));
                newStatList.Add(PlayerStatsManager.playerStats.GetStat(StatType.Mana));
                break;
        }
        statsToUse = newStatList;
        foreach (var stat in statsToUse)
        {
            SubscribeToStat(stat, (float f) => OnStatChanged());
        }
    }
    private void OnStatChanged()
    {
        loadDisplayValues();
        updateLabels();
    }
    private void loadDisplayValues()
    {
        List<(string, float, bool)> newDisplayList = new List<(string, float, bool)>();
        switch (pageToLoad)
        {
            case 0:
                newDisplayList.Add((statsToUse[0].Name, statsToUse[0].Value*100, true));
                newDisplayList.Add((statsToUse[1].Name, statsToUse[1].Value*100, true));
                break;
            case 1:
                newDisplayList.Add((statsToUse[0].Name, statsToUse[0].Value, false));
                newDisplayList.Add((statsToUse[1].Name, statsToUse[1].Value, false));
                newDisplayList.Add(("Life Regeneration", statsToUse[4].Value * statsToUse[2].Value + statsToUse[3].Value, false));
                newDisplayList.Add(("Energy Regeneration", statsToUse[8].Value * statsToUse[6].Value + statsToUse[7].Value, false));
                newDisplayList.Add(("Energy Recharge", statsToUse[8].Value * statsToUse[5].Value, false));
                break;
            case 2:
                newDisplayList.Add((statsToUse[0].Name, (statsToUse[0].Value/5)*100, true));
                newDisplayList.Add((statsToUse[1].Name, (statsToUse[1].Value) * 100, true));
                newDisplayList.Add(("Mana Regeneration", statsToUse[4].Value * statsToUse[2].Value + statsToUse[3].Value, false));
                break;
        }
        valuesToDisplay = newDisplayList;
    }
    private void destroyLabels()
    {
        while (labelContainer.childCount > 1)
        {
            DestroyImmediate(labelContainer.GetChild(1).gameObject);
        }
    }
    private void createLabels()
    {
        for (int i = 0; i < valuesToDisplay.Count; i++)
        {
            GameObject newLabel = Instantiate(labelPrefab, labelContainer);
            newLabel.SetActive(true);
        }
    }
    private void updateLabels()
    {
        labelContainer.gameObject.SetActive(true);
        if (labelContainer.childCount < 2)
        {
            createLabels();
        }
        int i = 1;
        foreach ((string, float, bool) tuple in valuesToDisplay)
        {
            GameObject prefab = labelContainer.GetChild(i).gameObject;
            GameObject numeric = labelContainer.GetChild(i).GetChild(0).gameObject;
            prefab.gameObject.GetComponent<TMP_Text>().text = tuple.Item1;
            if (tuple.Item3)
            {
                numeric.GetComponent<TMP_Text>().text = tuple.Item2 + "%";
            }
            else
            {
                numeric.GetComponent<TMP_Text>().text = tuple.Item2.ToString();
            }
            i++;
        }
    }
    public void Toggle()
    {
        bool toggle = !infoPanel.activeSelf;
        infoPanel.SetActive(toggle);
        if (toggle)
        {
            updateLabels();
        }
    }
    public bool IsOpen => infoPanel.activeSelf;
}
