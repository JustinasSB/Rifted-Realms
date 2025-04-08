using System.Collections.Generic;
using UnityEngine;

public static class ModifierDictionary
{
    public static Dictionary<ModifiableItemType, List<ItemModifier>> ModifierPools = new()
    {
        {
            ModifiableItemType.Axe,
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 0, 0, 100, 120),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 1, 0, 80, 99),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 2, 0, 60, 79),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 3, 0, 40, 59),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 4, 0, 20, 39),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 5, 0, 1, 19),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 0, 1, 50, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 1, 1, 40, 49),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 2, 1, 30, 39),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 3, 1, 20, 29),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 4, 1, 10, 19),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 5, 1, 1, 9)
            }
        },
        {
            ModifiableItemType.Bodyarmour,
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 0, 3, 5),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 0, 2, 3),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 0, 1, 2),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 0, 3, 5),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 0, 2, 3),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 0, 1, 2),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 0, 3, 5),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 0, 2, 3),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 0, 1, 2),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59)
            }
        }
    };
}
