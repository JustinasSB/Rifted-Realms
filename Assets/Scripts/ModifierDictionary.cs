using System.Collections.Generic;
using UnityEngine;

public static class ModifierDictionary
{
    // Format:
    // OperationType operationType | ModifierType type | ModifierScope scope | StatType affectedStat | float tier | float group | float min | float max | float weight | float levelRequirement
    public static Dictionary<(ItemType, ItemSpecific), List<ItemModifier>> ModifierPools = new()
    {
        {
            (ItemType.Mainhand, ItemSpecific.Axe),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 0, 0, 100, 120, 500, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 1, 0, 80, 99, 1000, 60),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 2, 0, 60, 79, 1000, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 3, 0, 40, 59, 1000, 20),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 4, 0, 20, 39, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 5, 0, 1, 19, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 0, 1, 50, 60, 500, 80),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 1, 1, 40, 49, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 2, 1, 30, 39, 1000, 40),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 3, 1, 20, 29, 1000, 20),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 4, 1, 10, 19, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.PhysicalDamage, 5, 1, 1, 9, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 0, 6, 100, 120, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 1, 6, 90, 99, 100, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 2, 6, 80, 89, 100, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 3, 6, 70, 79, 100, 40),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 4, 6, 30, 59, 100, 20),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Local, StatType.RadiantDamage, 5, 6, 10, 19, 100, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 0, 2, 28, 30, 500, 60),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 1, 2, 22, 27, 1000, 60),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 2, 2, 17, 21, 1000, 20),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 3, 2, 11, 16, 1000, 20),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 4, 2, 6, 10, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Local, StatType.AttackSpeed, 5, 2, 1, 5, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.AirResistance, 0, 3, 35, 40, 100, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.FireResistance, 0, 4, 35, 40, 100, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.WaterResistance, 0, 5, 35, 40, 100, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.PhysicalDamage, 0, 1000, 40, 50, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.PhysicalDamage, 0, 1001, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.AttackSpeed, 0, 1002, 5, 10, 1000, 0),

                new ItemModifier(OperationType.Extra, ModifierType.Enchant, ModifierScope.Local, StatType.FireDamage, 0, 100, 50, 100, 1000, 0, StatType.PhysicalDamage),
                new ItemModifier(OperationType.Extra, ModifierType.Enchant, ModifierScope.Local, StatType.WaterDamage, 0, 101, 50, 100, 1000, 0, StatType.PhysicalDamage),
                new ItemModifier(OperationType.Extra, ModifierType.Enchant, ModifierScope.Local, StatType.AirDamage, 0, 102, 50, 100, 1000, 0, StatType.PhysicalDamage),
                new ItemModifier(OperationType.Extra, ModifierType.Enchant, ModifierScope.Local, StatType.PoisonDamage, 0, 103, 50, 100, 1000, 0, StatType.PhysicalDamage),
                new ItemModifier(OperationType.Extra, ModifierType.Enchant, ModifierScope.Local, StatType.RadiantDamage, 0, 104, 50, 100, 1000, 0, StatType.PhysicalDamage)

            }
        },
        {
            (ItemType.Mainhand, ItemSpecific.Sword),
            new List<ItemModifier>
            {

            }
        },
        {
            (ItemType.Helmet, ItemSpecific.Armor),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1000, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1001, 10, 20, 1000, 0)
            }
        },
        {
            (ItemType.Bodyarmour, ItemSpecific.Armor),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 1, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 1, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 1, 40, 79, 1000, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1000, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1001, 10, 20, 1000, 0)
            }
        },
        {
            (ItemType.Pants, ItemSpecific.Armor),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 1, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 1, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 1, 40, 79, 1000, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1000, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1001, 10, 20, 1000, 0)
            }
        },
        {
            (ItemType.Gloves, ItemSpecific.Armor),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1000, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1001, 10, 20, 1000, 0)
            }
        },
        {
            (ItemType.Boots, ItemSpecific.Armor),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 0, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 0, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 0, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 1, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 1, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 1, 40, 79, 1000, 0),

                new ItemModifier(OperationType.Increase, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1000, 10, 20, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Implicit, ModifierScope.Local, StatType.Armour, 0, 1001, 10, 20, 1000, 0)
            }
        },
        {
            (ItemType.Amulet, ItemSpecific.Amulet),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0)
            }
        },
        {
            (ItemType.Belt, ItemSpecific.Belt),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0)
            }
        },
        {
            (ItemType.Ring, ItemSpecific.Ring),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0)
            }
        },
        {
            (ItemType.Core, ItemSpecific.Core),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Strength, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Dexterity, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 0, 4, 3, 5, 100, 80),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 1, 4, 2, 3, 500, 40),
                new ItemModifier(OperationType.Add, ModifierType.Suffix, ModifierScope.Global, StatType.Constitution, 2, 4, 1, 2, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 0, 1, 80, 100, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 1, 1, 60, 79, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Life, 2, 1, 40, 59, 1000, 0),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 0, 2, 120, 150, 1000, 60),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 1, 2, 80, 119, 1000, 30),
                new ItemModifier(OperationType.Add, ModifierType.Prefix, ModifierScope.Global, StatType.Mana, 2, 2, 40, 79, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 0, 5, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 1, 5, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 2, 5, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ProjectileDamage, 0, 6, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ProjectileDamage, 1, 6, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ProjectileDamage, 2, 6, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Convert, ModifierType.Enchant, ModifierScope.Global, StatType.Mana, 0, 7, 80, 99, 100, 80, StatType.Life),
                new ItemModifier(OperationType.Convert, ModifierType.Enchant, ModifierScope.Global, StatType.Mana, 1, 7, 50, 79, 500, 40, StatType.Life),
                new ItemModifier(OperationType.Convert, ModifierType.Enchant, ModifierScope.Global, StatType.Mana, 2, 7, 20, 49, 1000, 0, StatType.Life),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 0, 8, 150, 200, 100, 80),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 1, 8, 100, 149, 500, 40),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 2, 8, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 0, 9, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 1, 9, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 2, 9, 50, 99, 1000, 0),
            }
        },
        {
            (ItemType.Mainhand, ItemSpecific.Book),
            new List<ItemModifier>
            {
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 0, 0, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 1, 0, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.CastingSpeed, 2, 0, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 0, 3, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 1, 3, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.ManaRegenerationPercentage, 2, 3, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 0, 5, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 1, 5, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Suffix, ModifierScope.Global, StatType.SpellDamage, 2, 5, 50, 99, 1000, 0),
                new ItemModifier(OperationType.Convert, ModifierType.Enchant, ModifierScope.Global, StatType.Intelligence, 0,100, 20, 50, 1000, 0, StatType.Strength),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 0, 8, 126, 150, 100, 80),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 1, 8, 111, 125, 500, 40),
                new ItemModifier(OperationType.Multiply, ModifierType.Implicit, ModifierScope.Global, StatType.SpellDamage, 2, 8, 101, 110, 1000, 0),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 0, 9, 150, 200, 100, 80),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 1, 9, 100, 149, 500, 40),
                new ItemModifier(OperationType.Increase, ModifierType.Prefix, ModifierScope.Global, StatType.ProjectileDamage, 2, 9, 50, 99, 1000, 0),
            }
        },
        {
            (ItemType.Elixir, ItemSpecific.Life),
            new List<ItemModifier>
            {

            }
        },
        {
            (ItemType.Elixir, ItemSpecific.Mana),
            new List<ItemModifier>
            {

            }
        },
        {
            (ItemType.Elixir, ItemSpecific.Utility),
            new List<ItemModifier>
            {

            }
        },
    };
}
