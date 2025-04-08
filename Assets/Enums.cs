public enum StatType
{
    Strength,
    Intelligence,
    Wisdom,
    Constitution,
    Dexterity,
    Charisma,
    MovementSpeed,
    Armour,
    Evasion,
    Energy,
    CurrentEnergy,
    Life,
    CurrentLife,
    Mana,
    CurrentMana,
    RegenerationPercentage,
    RegenerationFlat,
    ManaRegenerationPercentage,
    ManaRegenerationFlat,
    EnergyRecharge,
    EnergyRegenerationPercentage,
    EnergyRegenerationFlat,
    AttackSpeed,
    CastingSpeed,
    AnimationSpeed,
    PhysicalDamage

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
    Book
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
    Magic
}