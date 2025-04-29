using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


[TestFixture]
public class StatTests
{
    private const float Tolerance = 0.0001f; // For float comparisons

    private Stat CreateDefaultStat()
    {
        // Creating a stat with a base value of 100 and a dummy type (using its ToString as name)
        return new Stat("Test", "A test stat", 100f, StatType.Life);
    }

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Constructor_SetsInitialValues()
    {
        var stat = CreateDefaultStat();
        // When constructed, RecalculateValue is called in the constructor.
        Assert.AreEqual(100f, stat.Value, Tolerance);
        Assert.AreEqual("Test", stat.Name);
    }

    [Test]
    public void DirectValueSet_SetsValueAndFiresEvent()
    {
        var stat = CreateDefaultStat();
        float reportedValue = 0f;
        stat.OnValueChanged += (newValue) => reportedValue = newValue;

        stat.DirectValueSet(250f);

        Assert.AreEqual(250f, stat.Value, Tolerance);
        Assert.AreEqual(250f, reportedValue, Tolerance);
    }

    [Test]
    public void RecalculateValue_CorrectlyAppliesModifiers()
    {
        // Value = (BaseValue + sum(baseAdded)) * (1 + sum(baseIncrease)) * (product of baseMultiplier)
        var stat = CreateDefaultStat();
        float initial = stat.Value;

        // Apply a base addition of 20
        stat.AddBaseAdded(20f);
        float expectedAfterAdd = (100f + 20f) * 1f * 1f;
        Assert.AreEqual(expectedAfterAdd, stat.Value, Tolerance);

        // Apply an increase of 0.1 (i.e., 10% increase)
        stat.AddIncrease(0.1f);
        float expectedAfterIncrease = (100f + 20f) * (1f + 0.1f) * 1f;
        Assert.AreEqual(expectedAfterIncrease, stat.Value, Tolerance);

        // Apply a multiplier of 1.2
        stat.AddMultiplier(1.2f);
        float expectedAfterMultiplier = (100f + 20f) * (1f + 0.1f) * 1.2f;
        Assert.AreEqual(expectedAfterMultiplier, stat.Value, Tolerance);
    }

    [Test]
    public void RemoveModifiers_UpdatesValueCorrectly()
    {
        var stat = CreateDefaultStat();
        stat.AddBaseAdded(30f);
        stat.AddIncrease(0.2f);
        stat.AddMultiplier(1.5f);

        float valueWithMods = stat.Value;

        // Now remove the modifiers one by one and check value trends toward original
        stat.RemoveBaseAdded(30f);
        float afterRemoveAdd = stat.Value;
        Assert.Less(afterRemoveAdd, valueWithMods);

        stat.RemoveIncrease(0.2f);
        float afterRemoveIncrease = stat.Value;
        Assert.Less(afterRemoveIncrease, afterRemoveAdd);

        stat.RemoveMultiplier(1.5f);
        // Value should now be recalculated as BaseValue only.
        Assert.AreEqual(100f, stat.Value, Tolerance);
    }

    [Test]
    public void ModifyStat_Method_OperatesCorrectly()
    {
        var stat = CreateDefaultStat();

        // Using ModifyStat to add a base addition.
        stat.ModifyStat(OperationType.Add, 50f);
        float expected = (100f + 50f);
        Assert.AreEqual(expected, stat.GetTotalAdd(), Tolerance);

        // Then remove that addition.
        stat.ModifyStat(OperationType.AddRemove, 50f);
        expected = 100f;
        Assert.AreEqual(expected, stat.GetTotalAdd(), Tolerance);

        // Test Increase and Multiply as well
        stat.ModifyStat(OperationType.Increase, 0.25f); // +25%
        Assert.AreEqual(0.25f, stat.GetTotalIncrease(), Tolerance);
        stat.ModifyStat(OperationType.Multiply, 1.1f); // Multiply by 1.1
        Assert.AreEqual(1.1f, stat.GetTotalMultiplier(), Tolerance);
    }

    [Test]
    public void Conversion_AddAndRemoveConversion_AdjustsStatValues()
    {
        // Create two stats to demonstrate conversion effects.
        var statA = new Stat("A", "Source stat", 200f, StatType.Life);
        var statB = new Stat("B", "Target stat", 100f, StatType.Mana);

        float aInitial = statA.Value;
        float bInitial = statB.Value;

        // Add a conversion: For instance, converting 20% of statA’s value to statB.
        statA.AddConversion(statB, 0.2f);
        // After conversion, statA's value should be reduced by an amount and statB increased by that amount.
        // We assume conversion value = statA.valueBeforeConversion * (0.2f * scale)
        // Here scale is likely 1 as total conversion is not more than 100%.
        Assert.Less(statA.Value, aInitial); // statA should lose some value due to conversion

        // Now remove the conversion partially.
        statA.RemoveConversion(statB, 0.1f);
        // statA value should be recalculated.
        Assert.Less(statA.Value, aInitial);
    }

    [Test]
    public void AsExtra_AddAndRemoveExtra_AdjustsDependantStat()
    {
        var statA = new Stat("A", "Primary stat", 150f, StatType.Strength);
        var statB = new Stat("B", "Extra stat", 50f, StatType.Dexterity);
        float aInitial = statA.Value;

        // Add extra: statA provides some extra value to statB.
        statA.AddAsExtra(statB, 0.1f);
        // statB should have its base added modified by the conversion (valueBeforeConversion * percentage)
        // Since valueBeforeConversion is recalculated within RecalculateValue, we test that statB's value increases.
        Assert.Greater(statB.Value, 50f);

        // Now remove the extra.
        statA.RemoveAsExtra(statB, 0.1f);
        // statB's value should revert closer to initial.
        Assert.AreEqual(50f, statB.Value, Tolerance);
    }

    [Test]
    public void OnAfterDeserialize_ResetsNonSerializedFields()
    {
        var stat = CreateDefaultStat();
        // Simulate a deserialization round-trip that resets nonserialized lists.
        stat.AddBaseAdded(20f);
        stat.AddIncrease(0.2f);
        stat.AddMultiplier(1.5f);
        // Change the value so it no longer equals BaseValue
        stat.DirectValueSet(300f);

        // Now call OnAfterDeserialize to simulate Unity’s deserialization.
        stat.OnAfterDeserialize();

        // The nonserialized lists should be reset, so modifiers no longer affect Value.
        // Also, if Value was nonzero and BaseValue was 0, it corrects itself.
        Assert.AreEqual(stat.BaseValue, stat.Value, Tolerance);
    }

    [Test]
    public void Clone_CreatesIndependentButEquivalentCopy()
    {
        var stat = CreateDefaultStat();
        stat.AddBaseAdded(10f);
        stat.AddIncrease(0.1f);
        stat.AddMultiplier(1.2f);

        var clone = stat.Clone();
        // The cloned stat should have the same recalculated value.
        Assert.AreEqual(stat.Value, clone.Value, Tolerance);

        // Now change the original and check that clone remains unchanged.
        stat.AddBaseAdded(50f);
        Assert.AreNotEqual(stat.Value, clone.Value);
    }
}
