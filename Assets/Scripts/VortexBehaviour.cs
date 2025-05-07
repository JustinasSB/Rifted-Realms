using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VortexBehaviour : MonoBehaviour, ICenterAOE
{
    Dictionary<StatType, Stat> Damage;
    [SerializeField] LayerMask TargetLayer;
    public Ability abilityData;
    public bool isUsed;
    private float Duration;
    private float Elapsed;
    private ProjectilePool pool;
    public event Action<GameObject, ProjectilePool> OnExpired;
    private Dictionary<EnemyHealthManager, float> enemiesInside = new Dictionary<EnemyHealthManager, float>();
    private const float DamageInterval = 0.5f;
    public void Initialize(Dictionary<StatType, Stat> damage, LayerMask targetLayer, float duration, float AreaOfEffect, ProjectilePool pool)
    {
        this.Damage = damage;
        this.TargetLayer = targetLayer;
        this.Duration = duration;
        this.pool = pool;
        this.Elapsed = 0;
        isUsed = true;
        this.transform.localScale = this.transform.localScale * AreaOfEffect;
        this.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    void Update()
    {
        if (!isUsed) return;
        Elapsed += Time.deltaTime;
        if (Elapsed > Duration)
        {
            Expire();
            return;
        }
        this.transform.rotation *= Quaternion.Euler(0, 0, 180 * Time.deltaTime);
        var keys = enemiesInside.Keys.ToList();
        List<EnemyHealthManager> toRemove = new List<EnemyHealthManager>();
        foreach (var enemy in keys)
        {
            enemiesInside[enemy] += Time.deltaTime;
            if (enemiesInside[enemy] >= DamageInterval)
            {
                foreach (var damage in Damage)
                {
                    enemy.TakeDamage(damage.Value.Value);
                }
                enemiesInside[enemy] = 0f;
                if (!enemy.isAlive) toRemove.Add(enemy);
            }
        }
        foreach (var enemy in toRemove)
        {
            enemiesInside.Remove(enemy);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isUsed) return;
        if (((1 << other.gameObject.layer) & TargetLayer.value) != 0)
        {
            EnemyHealthManager enemy = other.GetComponent<EnemyHealthManager>();
            if (enemy != null)
            {
                foreach (var damage in Damage)
                {
                    enemy.TakeDamage(damage.Value.Value);
                }
                if (enemy.isAlive)
                {
                    if (!enemiesInside.ContainsKey(enemy))
                        enemiesInside.Add(enemy, 0f);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        EnemyHealthManager enemy = other.GetComponent<EnemyHealthManager>();
        if (enemy != null && enemiesInside.ContainsKey(enemy))
        {
            enemiesInside.Remove(enemy);
        }
    }
    public void Expire()
    {
        isUsed = false;
        enemiesInside.Clear();
        OnExpired?.Invoke(this.gameObject, pool);
    }
}
