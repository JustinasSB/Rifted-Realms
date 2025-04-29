using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemModifierTests
{
    [Test]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var modifier = new ItemModifier(
            OperationType.Add,
            ModifierType.Suffix,
            ModifierScope.Local,
            StatType.Life,
            tier: 1,
            group: 1,
            min: 10f,
            max: 20f,
            weight: 1f,
            levelRequirement: 1f
        );

        Assert.AreEqual(OperationType.Add, modifier.OperationType);
        Assert.AreEqual(ModifierType.Suffix, modifier.Type);
        Assert.AreEqual(ModifierScope.Local, modifier.Scope);
        Assert.AreEqual(StatType.Life, modifier.AffectedStat);
        Assert.IsTrue(modifier.RolledValue >= 10f && modifier.RolledValue <= 20f);
    }
    [Test]
    public void RollValue_GeneratesValueWithinRange()
    {
        var modifier = new ItemModifier(
            OperationType.Add,
            ModifierType.Suffix,
            ModifierScope.Local,
            StatType.Life,
            tier: 1,
            group: 1,
            min: 10f,
            max: 20f,
            weight: 1f,
            levelRequirement: 1f
        );

        // Act
        modifier.RollValue();
        Assert.IsTrue(modifier.RolledValue >= 10f && modifier.RolledValue <= 20f);
    }
    [Test]
    public void SetText_GeneratesCorrectText()
    {
        var modifier = new ItemModifier(
            OperationType.Add,
            ModifierType.Suffix,
            ModifierScope.Local,
            StatType.Life,
            tier: 1,
            group: 1,
            min: 10f,
            max: 20f,
            weight: 1f,
            levelRequirement: 1f
        );

        // Act
        modifier.AddToRolledValue(5f); // Adjusting the rolled value

        Assert.AreEqual($"Adds {modifier.RolledValue} {StatType.Life.GetDisplayName()}", modifier.Text);
    }
    [Test]
    public void Clone_CreatesIdenticalInstance()
    {
        var original = new ItemModifier(
            OperationType.Add,
            ModifierType.Suffix,
            ModifierScope.Local,
            StatType.Life,
            tier: 1,
            group: 1,
            min: 10f,
            max: 20f,
            weight: 1f,
            levelRequirement: 1f
        );

        // Act
        var clone = original.Clone();

        Assert.AreEqual(original.OperationType, clone.OperationType);
        Assert.AreEqual(original.Type, clone.Type);
        Assert.AreEqual(original.Scope, clone.Scope);
        Assert.AreEqual(original.AffectedStat, clone.AffectedStat);
        Assert.AreEqual(original.Tier, clone.Tier);
        Assert.AreEqual(original.Group, clone.Group);
        Assert.AreEqual(original.RollRangeMin, clone.RollRangeMin);
        Assert.AreEqual(original.RollRangeMax, clone.RollRangeMax);
        Assert.AreEqual(original.Weight, clone.Weight);
        Assert.AreEqual(original.LevelRequirement, clone.LevelRequirement);
    }
}
