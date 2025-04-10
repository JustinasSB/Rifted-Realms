using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour 
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Slider fill;

    private void Start()
    {
        levelManager = GameObject.FindWithTag("Player")?.GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found");
        }
    }
    private void Update()
    {
        LevelManager.level.IncreaseExperience((uint)10, 1);
        level.text = LevelManager.level.CurrentLevel.ToString();
        fill.value = (float)LevelManager.level.percentageFilled;
    }
}
