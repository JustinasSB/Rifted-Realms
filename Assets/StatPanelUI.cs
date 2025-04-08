using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelUI : MonoBehaviour
{
    [SerializeField] private Button offense;
    [SerializeField] private Button defense;
    [SerializeField] private Button utility;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Transform labelContainer;
    [SerializeField] private GameObject labelPrefab;
    [SerializeField] private GameObject[] AttributeNumerics;
    private PlayerStatsManager playerStatsManager;
    private List<Stat> statsToUse;
    private List<Stat> Attributes = new List<Stat>();
    private List<(string, float, bool)> valuesToDisplay = new List<(string, float, bool)>();
    private bool isVisible = false;
    private float labelUpdatePeriod = 2f;
    private float actionTime = 0f;
    private int pageToLoad = 0;
    void Start()
    {
        playerStatsManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>();
        labelPrefab.SetActive(false);
        loadStats();
        loadAttributes();
        loadDisplayValues();
        offense.onClick.AddListener(() => SetValue(0));
        defense.onClick.AddListener(() => SetValue(1));
        utility.onClick.AddListener(() => SetValue(2));
    }
    void Update()
    {
        if (Time.time > actionTime && isVisible) 
        {
            actionTime = Time.time+labelUpdatePeriod;
            updateLabels();
            updateAttributes();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            togglePanel();
        }
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
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Strength));
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Intelligence));
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Wisdom));
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Constitution));
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Dexterity));
        Attributes.Add(PlayerStatsManager.playerStats.GetStat(StatType.Charisma));
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
    private void togglePanel()
    {
        isVisible = !isVisible;
        infoPanel.SetActive(isVisible);

        if (isVisible)
        {
            updateLabels();
        }
    }
}
