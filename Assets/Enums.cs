using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum StatType
{
    Strength,
    Dexterity,
    Intelligence,
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
    RegenerationFlat
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