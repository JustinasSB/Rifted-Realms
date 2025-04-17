using System;

public class Level
{
    public event Action<int> OnLevelUp;
    public event Action<int> OnPassivePointChange;
    private ulong experience;
    private ulong experienceForLevel;
    private ulong experienceForPreviousLevel;
    public float percentageFilled;
    public int SkillPoints;
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
        float gain = Math.Max(Math.Abs(sourceLevel - CurrentLevel) - (CurrentLevel/20),0);
        experience += (ulong)(exp * Math.Pow(0.9,gain));
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
    public void IncrementPassivePoints()
    {
        SkillPoints++;
        OnPassivePointChange?.Invoke(SkillPoints);
    }
    public void DecrementPassivePoints()
    {
        SkillPoints--;
        OnPassivePointChange?.Invoke(SkillPoints);
    }
}