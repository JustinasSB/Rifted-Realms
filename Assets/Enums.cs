using System;
using System.ComponentModel;
using System.Reflection;
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}
public enum StatType
{
    [Description("Strength")] Strength,
    [Description("Intelligence")] Intelligence,
    [Description("Wisdom")] Wisdom,
    [Description("Constitution")] Constitution,
    [Description("Dexterity")] Dexterity,
    [Description("Charisma")] Charisma,
    [Description("Movement Speed")] MovementSpeed,
    [Description("Evasion")] Evasion,
    [Description("Armour")] Armour,
    [Description("Energy")] Energy,
    [Description("Current Energy")] CurrentEnergy,
    [Description("Life")] Life,
    [Description("Current Life")] CurrentLife,
    [Description("Mana")] Mana,
    [Description("Current Mana")] CurrentMana,
    [Description("Regeneration Percentage")] RegenerationPercentage,
    [Description("Regeneration Flat")] RegenerationFlat,
    [Description("Mana Regeneration Percentage")] ManaRegenerationPercentage,
    [Description("Mana Regeneration Flat")] ManaRegenerationFlat,
    [Description("Energy Recharge")] EnergyRecharge,
    [Description("Energy Regeneration Percentage")] EnergyRegenerationPercentage,
    [Description("Energy Regeneration Flat")] EnergyRegenerationFlat,
    [Description("Fire Resistance")] FireResistance,
    [Description("Water Resistance")] WaterResistance,
    [Description("Air Resistance")] AirResistance,
    [Description("Poison Resistance")] PoisonResistance,
    [Description("Radiant Resistance")] RadiantResistance,
    [Description("Attack Speed")] AttackSpeed,
    [Description("Casting Speed")] CastingSpeed,
    [Description("Animation Speed")] AnimationSpeed,
    [Description("Physical Damage")] PhysicalDamage,
    [Description("Fire Damage")] FireDamage,
    [Description("Water Damage")] WaterDamage,
    [Description("Air Damage")] AirDamage,
    [Description("Poison Damage")] PoisonDamage,
    [Description("Radiant Damage")] RadiantDamage,
    [Description("Effect Duration")] EffectDuration,
    [Description("Damage")] Damage,
    [Description("Attack Time")] Interval,
}
public enum OperationType
{
    Add,
    Increase,
    Multiply,
    Convert,
    Extra,
    AddRemove,
    IncreaseRemove,
    MultiplyRemove,
    ConvertRemove,
    ExtraRemove,
    SetBase
}
public enum AbilityType 
{
    Spell = 1,
    SelfSpell,
    MeleeAttack,
    RangedAttack
}
public enum WeaponType
{
    Unarmed,
    Dagger,
    Sword,
    Axe,
    Mace,
    Staff,
    Bow,
    Wand,
    Book,
    Pole
}
public enum ItemType
{
    None,
    Mainhand,
    Offhand,
    Helmet,
    Bodyarmour,
    Pants,
    Belt,
    Gloves,
    Boots,
    Ring,
    Amulet,
    Elixir,
    Stackable
}
public enum ItemSpecific
{
    None,
    Evasion,
    Armor,
    Energy,
    Life,
    Mana,
    Utility,
    Dagger,
    Sword,
    Axe,
    Mace,
    Staff,
    Bow,
    Wand,
    Book,
    Quiver,
    Shield,
    Focus,
    Trinket,
    Sigil,
    Fist,
    Pole,

    Helmet,
    Bodyarmour,
    Pants,
    Belt,
    Gloves,
    Boots,
    Ring,
    Amulet,
    Elixir
}
public enum ModifiableItemType
{
    Dagger,
    Sword,
    Axe,
    Mace,
    Staff,
    Bow,
    Wand,
    Book,
    Helmet,
    Bodyarmour,
    Pants,
    Belt,
    Gloves,
    Boots,
    Ring,
    Amulet,
    Elixir
}
public enum ModifierType
{
    Enchant,
    Implicit,
    Prefix,
    Suffix
}
public enum ModifierScope
{
    Local,
    Global
}
public enum Rarity
{
    World,
    Divine,
    Legendary,
    Rare,
    Magic,
    Common,
    None
}