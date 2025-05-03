using NUnit.Framework;
public class LevelTests
{
    private Level level;

    [SetUp]
    public void Setup()
    {
        level = new Level();
    }

    [Test]
    public void InitialValues_AreCorrect()
    {
        Assert.AreEqual(0, level.CurrentLevel);
        Assert.AreEqual(0, level.SkillPoints);
        Assert.AreEqual(0, level.percentageFilled);
    }

    [Test]
    public void IncreaseExperience_TriggersExperienceChange()
    {
        ulong receivedExp = 0;
        level.OnExperienceChange += (exp) => receivedExp = exp;

        level.IncreaseExperience(1024, sourceLevel: 5);

        Assert.AreNotEqual(0, receivedExp);
    }

    [Test]
    public void IncreaseExperience_LevelUpOccurs_ExperienceUpdates()
    {
        bool levelUpCalled = false;
        int newLevel = -1;
        level.OnLevelUp += (lvl) => { levelUpCalled = true; newLevel = lvl; };

        level.IncreaseExperience(2000, sourceLevel: 5);

        Assert.IsTrue(levelUpCalled);
        Assert.AreEqual(1, newLevel);
        Assert.AreEqual(1, level.CurrentLevel);
    }

    [Test]
    public void DecreaseExperience_ClampsToPreviousLevel()
    {
        level.IncreaseExperience(2000, sourceLevel: 5);
        float expBefore = level.percentageFilled;

        level.DecreaseExperience(2);

        Assert.GreaterOrEqual(level.percentageFilled, 0);
    }

    [Test]
    public void PassivePoints_IncrementDecrementWork()
    {
        int lastPoints = -1;
        level.OnPassivePointChange += (points) => lastPoints = points;

        level.IncrementPassivePoints();
        Assert.AreEqual(1, lastPoints);
        Assert.AreEqual(1, level.SkillPoints);

        level.DecrementPassivePoints();
        Assert.AreEqual(0, lastPoints);
        Assert.AreEqual(0, level.SkillPoints);
    }

    [Test]
    public void IncreaseExperience_WithHighLevelDifference_IsPenalized()
    {
        level.IncreaseExperience(1000, sourceLevel: 50);
        float first = level.percentageFilled;

        level = new Level();
        level.IncreaseExperience(1000, sourceLevel: 1);
        float second = level.percentageFilled;

        Assert.Less(first, second);
    }

    [Test]
    public void PercentageFill_IsCorrectlyCalculated()
    {
        level.IncreaseExperience(512, sourceLevel: 0);
        Assert.IsTrue(level.percentageFilled > 0f && level.percentageFilled < 1f);
    }
}
