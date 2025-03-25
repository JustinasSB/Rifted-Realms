using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelUI : MonoBehaviour
{
    public GameObject infoPanel;
    public Transform labelContainer;
    public GameObject labelPrefab;
    private bool isVisible = false;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] Scrollbar scrollbar;
    private float labelUpdatePeriod = 2f;
    private float actionTime = 0f;
    void Start()
    {
        playerStatsManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>();
    }
    void Update()
    {
        if (Time.time > actionTime && isVisible) 
        {
            actionTime = Time.time+labelUpdatePeriod;
            UpdateLabels();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogglePanel();
        }
    }
    public void UpdateLabels()
    {
        if (labelContainer.childCount > 1)
        {
            for (int i = 1; i < labelContainer.childCount; i++)
            {
                Destroy(labelContainer.GetChild(i).gameObject);
            }
        }
        labelPrefab.SetActive(false);
        scrollbar.gameObject.SetActive(true);


        foreach (var label in playerStatsManager.playerStats.Stats)
        {
            GameObject newLabel = Instantiate(labelPrefab, labelContainer);
            newLabel.GetComponent<TMP_Text>().text = label.Key + ": " + label.Value.Value;
            newLabel.SetActive(true);
        }
    }
    public void TogglePanel()
    {
        isVisible = !isVisible;
        infoPanel.SetActive(isVisible);

        if (isVisible)
        {
            UpdateLabels();
        }
    }
}
