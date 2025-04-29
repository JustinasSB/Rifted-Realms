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
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Slider fill;
    private void Start()
    {
        LevelManager.level.OnExperienceChange += experience => ExperienceChanged(experience);
        LevelManager.level.OnLevelUp += level => LevelChanged(level);
        LevelManager.level.IncreaseExperience(2000, 1);
    }
    private void ExperienceChanged(ulong experience)
    {
        fill.value = (float)LevelManager.level.percentageFilled;
    }
    private void LevelChanged(int lvl)
    {
        this.level.text = lvl.ToString();
    }
}
