using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityItem", menuName = "Scriptable Objects/AbilityItem")]
public class AbilityItem : BaseItem
{
    public GameObject effectPrefab;
    public Sprite abilityIcon;
    public Ability ability;
    public Level level;
    // multiplies mana cost if support and damage if active ability gem
    public float Multiplier;
    public float BaseManaCost;
    public float ManaCost;
    public string Description;
    public AbilityItem Clone()
    {
        AbilityItem clone = ScriptableObject.CreateInstance<AbilityItem>();
        clone.Icon = this.Icon;
        clone.ItemType = this.ItemType;
        clone.effectPrefab = this.effectPrefab;
        clone.abilityIcon = this.abilityIcon;
        clone.level = this.level;
        clone.Multiplier = this.Multiplier;
        clone.BaseManaCost = this.BaseManaCost;
        clone.ManaCost = this.ManaCost;
        clone.Description = this.Description;

        if (this.ability != null)
        {
            clone.ability = new Ability
            {
                serializedStats = new List<AbilityStatEntry>(this.ability.serializedStats.Select(entry => new AbilityStatEntry
                {
                    stat = new Stat(entry.stat.BaseValue, entry.stat.StatType),
                    scalable = entry.scalable,
                    statType = entry.statType
                })),
                tags = new List<AbilityTag>(this.ability.tags),
                type = this.ability.type,
                behaviour = this.ability.behaviour,
                pool = this.ability.pool,
                support = this.ability.support,
                modifiers = new List<StatModifier>(this.ability.modifiers.Select(mod => new StatModifier
                {
                    To = mod.To,
                    Value = mod.Value,
                    OperationType = mod.OperationType
                }))
            };
        }

        return clone;
    }
}
