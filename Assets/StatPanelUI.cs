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
    private PlayerStatsManager playerStatsManager;
    private List<Stat> statsToDisplay;
    private bool isVisible = false;
    private float labelUpdatePeriod = 2f;
    private float actionTime = 0f;
    private int pageToLoad = 0;
    void Start()
    {
        playerStatsManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>();
        labelPrefab.SetActive(false);
        loadStats();
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
        updateLabels();
    }
    private void loadStats() 
    {
        List<Stat> newStatList = new List<Stat>();
        switch (pageToLoad)
        {
            case 0:
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.AttackSpeed));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.CastingSpeed));
                break;
            case 1:
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.Armour));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.Evasion));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.RegenerationPercentage));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.RegenerationFlat));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.EnergyRecharge));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationPercentage));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.EnergyRegenerationFlat));
                break;
            case 2:
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.MovementSpeed));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.AnimationSpeed));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.ManaRegenerationPercentage));
                newStatList.Add(playerStatsManager.playerStats.GetStat(StatType.ManaRegenerationFlat));
                break;
        }
        statsToDisplay = newStatList;
    }
    private void destroyLabels()
    {
        while (labelContainer.childCount > 1)
        {
            DestroyImmediate(labelContainer.GetChild(1).gameObject);
        }
    }
    private void updateLabels()
    {
        labelContainer.gameObject.SetActive(true);
        if (labelContainer.childCount > 1)
        {
            int i = 1;
            foreach (Stat stat in statsToDisplay)
            {
                labelContainer.GetChild(i).gameObject.GetComponent<TMP_Text>().text = stat.Name + ": " + stat.Value;
                i++;
            }
        }
        else
        {
            foreach (Stat stat in statsToDisplay)
            {
                GameObject newLabel = Instantiate(labelPrefab, labelContainer);
                newLabel.GetComponent<TMP_Text>().text = stat.Name + ": " + stat.Value;
                newLabel.SetActive(true);
            }
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
