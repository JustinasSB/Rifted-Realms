using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
    AnimationSpeed

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