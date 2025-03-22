using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Level
{
    public event Action<int> OnLevelUp;
    private ulong experience;
    private ulong experienceForLevel;
    private ulong experienceForPreviousLevel;
    public float percentageFilled;
    public int CurrentLevel { get; private set; }
    private uint penalty;
    public Level() 
    {
        percentageFilled = 0;
        experienceForPreviousLevel = 0;
        experienceForLevel = 1024;
        experience = 0;
    }
    public void IncreaseExperience(ulong exp, int sourceLevel) 
    {
        experience += exp;
        if (experience >= experienceForLevel) 
        {
            LevelUp();
        }
        CalculateFill();
    }
    private void setPenalty()
    {
        if (penalty % 5 == 0)
        {
            penalty = (uint)(CurrentLevel / 5);
        }
    }
    public void DecreaseExperience(float penalty) 
    {
        ulong exp = (ulong)Math.Floor((experienceForLevel - experienceForPreviousLevel) / penalty);
        experience -= exp;
        if (experience < experienceForPreviousLevel)
        {
            experience = experienceForPreviousLevel;
        }
        CalculateFill();
    }
    private void CalculateFill() 
    {
        float pexp = experience - experienceForPreviousLevel;
        float rexp = experienceForLevel - experienceForPreviousLevel;
        if (pexp > 0)
        {
            percentageFilled = pexp/rexp;
        }
    }
    private void LevelUp() 
    {
        this.CurrentLevel += 1;
        experienceForPreviousLevel = experienceForLevel;
        experienceForLevel = (ulong)(experienceForLevel * 1.5);
        setPenalty();

        OnLevelUp?.Invoke(this.CurrentLevel);
    }

}