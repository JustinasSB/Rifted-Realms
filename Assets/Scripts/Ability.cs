using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public List<AbilityStatEntry> serializedStats;
    public List<AbilityTag> tags;
    public AbilityType type;
    public AbilityBehaviourTag behaviour;
    public ProjectilePool pool;
    public bool support;
    public List<StatModifier> modifiers;

    private Dictionary<StatType, (Stat stat, bool scalable)> _runtimeStats;

    public Dictionary<StatType, (Stat, bool)> Stats
    {
        get
        {
            if (_runtimeStats == null)
            {
                _runtimeStats = new();
                foreach (var entry in serializedStats)
                {
                    _runtimeStats[entry.statType] = (entry.stat, entry.scalable);
                }
            }
            return _runtimeStats;
        }
    }
}
[System.Serializable]
public class AbilityStatEntry
{
    public StatType statType;
    public Stat stat;
    public bool scalable;
}
