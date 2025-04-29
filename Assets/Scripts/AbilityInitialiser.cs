using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using System;

internal class AbilityInitialiser : MonoBehaviour
{
    public static void TriggerProjectile(GameObject prefab, Ability data, float damageMultiplier, Transform origin, Vector3 target, Dictionary<StatType, Stat> caster, LayerMask targetLayer)
    {
        int projectileCount = (int)RetrieveStatAdd(StatType.Projectile, data, caster);
        if (projectileCount == 0) return;
        float pierce = RetrieveStatAdd(StatType.Pierce, data, caster);
        float duration = RetrieveStatMul(StatType.EffectDuration, data, caster);
        float speed = RetrieveStatMul(StatType.ProjectileSpeed, data, caster);
        if (pierce == 0 || duration == 0) return;
        Dictionary<StatType, Stat> damage = CalculateDamage(data, caster, damageMultiplier);
        //additional projectiles are offset by 15 degrees to the left or right of original target
        //offset scales depending on projectile number so no projectiles overlap
        float spreadAngle = 15;
        //distance from projectile origin position to cursor target, used to create new targets for offset projectiles
        float distance = Vector3.Distance(origin.position, target);
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject instance = PoolManager.Instance.getGameObject(prefab, origin.position, quaternion.identity, data.pool);
            IProjectile projectile = instance.GetComponent<IProjectile>();
            if (projectile != null)
            {
                Vector3 direction = (target - origin.position);
                direction = new Vector3(direction.x, 0f, direction.z).normalized;
                if (i > 0)
                {
                    // Calculate angle offset
                    int side = (i % 2 == 0) ? -1 : 1; // alternate left/right
                    int index = (i + 1) / 2;          // 1,1,2,2,3,3
                    float angle = side * index * spreadAngle;

                    direction = Quaternion.Euler(0, angle, 0) * direction;
                }
                projectile.Initialize(damage, targetLayer, direction, pierce, duration, speed, data.pool);
            }
        }
    }
    private static Dictionary<StatType, Stat> CalculateDamage(Ability data, Dictionary<StatType, Stat> caster, float damageMultiplier)
    {
        Dictionary<StatType, Stat> damage = new();
        damage = RetrieveBaseDamageValues(damage, data, caster);
        float genericIncrease = caster[StatType.Damage].GetTotalIncrease();
        float genericMultiplier = caster[StatType.Damage].GetTotalMultiplier() * (damageMultiplier/100);
        foreach (AbilityTag tag in data.tags)
        {
            Stat value;
            switch (tag)
            {
                case AbilityTag.Projectile:
                    if (caster.TryGetValue(StatType.ProjectileDamage, out value))
                        (genericIncrease, genericMultiplier) = ScaleGeneric(genericIncrease, genericMultiplier, value);
                    break;
                case AbilityTag.Attack:
                    if (caster.TryGetValue(StatType.AttackDamage, out value))
                        (genericIncrease, genericMultiplier) = ScaleGeneric(genericIncrease, genericMultiplier, value);
                    break;
                case AbilityTag.Spell:
                    if (caster.TryGetValue(StatType.SpellDamage, out value))
                        (genericIncrease, genericMultiplier) = ScaleGeneric(genericIncrease, genericMultiplier, value);
                    break;
            }
        }
        foreach (Stat stat in damage.Values)
        {
            stat.AddIncrease(genericIncrease);
            stat.AddMultiplier(genericMultiplier);
        }
        return damage;
    }
    private static (float, float) ScaleGeneric(float Increase, float Multiplier, Stat scale)
    {
        Increase += scale.GetTotalIncrease();
        Multiplier *= scale.GetTotalMultiplier();
        return (Increase, Multiplier);
    }
    private static Dictionary<StatType, Stat> RetrieveBaseDamageValues(Dictionary<StatType, Stat> damage, Ability data, Dictionary<StatType, Stat> caster)
    {
        foreach (var type in new[] { StatType.AirDamage, StatType.FireDamage, StatType.PhysicalDamage, StatType.PoisonDamage, StatType.RadiantDamage, StatType.WaterDamage })
            if (RetrieveStat(type, data, caster) is { } stat)
                damage.TryAdd(type, stat);
        return damage;
    }
    private static Stat RetrieveStat(StatType statType, Ability ability, Dictionary<StatType, Stat> caster)
    {
        Stat toAdd = null;
        if (ability.Stats.TryGetValue(statType, out var value))
        {
            toAdd = value.Item1.Clone();
            if (value.Item2 && caster.TryGetValue(statType, out var add))
            {
                toAdd.AddBaseAdded(add.GetTotalAdd());
                toAdd.AddIncrease(add.GetTotalIncrease());
                toAdd.AddMultiplier(add.GetTotalMultiplier());
            }
        }
        return toAdd;
    }
    private static float RetrieveStatMul(StatType statType, Ability ability, Dictionary<StatType, Stat> caster)
    {
        Stat toAdd = null;
        if (ability.Stats.TryGetValue(statType, out var value))
        {
            toAdd = value.Item1.Clone();
            if (value.Item2 && caster.TryGetValue(statType, out var add))
            {
                toAdd.AddIncrease(add.GetTotalIncrease());
                toAdd.AddMultiplier(add.GetTotalMultiplier());
            }
        }
        return toAdd.Value;
    }
    private static float RetrieveStatAdd(StatType statType, Ability ability, Dictionary<StatType, Stat> caster)
    {
        if (!ability.Stats.TryGetValue(statType, out var stat)) return 0;
        int value = (int)stat.Item1.Value;
        if (stat.Item2 && caster.TryGetValue(statType, out var casterStat))
            value += (int)casterStat.Value;
        return value;
    }
}
public interface IProjectile
{
    event Action<GameObject, ProjectilePool> OnExpired;
    void Initialize(Dictionary<StatType, Stat> damage, LayerMask targetLayer, Vector3 target, float pierce, float duration, float speed, ProjectilePool pool);
}