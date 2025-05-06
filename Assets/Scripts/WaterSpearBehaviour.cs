using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WaterSpearBehaviour : MonoBehaviour, IProjectile
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask WallLayer;
    Dictionary<StatType, Stat> Damage;
    LayerMask TargetLayer;
    public Ability abilityData;
    public bool isUsed;
    private float Duration;
    private float Elapsed;
    private float Speed;
    private Vector3 TargetDirection;
    private float Pierce;
    private ProjectilePool pool;
    public event Action<GameObject, ProjectilePool> OnExpired;
    private List<GameObject> hitTargets = new();
    bool changed = false;

    public void Initialize(Dictionary<StatType, Stat> damage, LayerMask targetLayer, Vector3 target, float pierce, float duration, float speed, ProjectilePool pool)
    {
        Damage = damage;
        TargetLayer = targetLayer;
        TargetDirection = target;
        Pierce = pierce;
        Duration = duration;
        Speed = speed;
        Elapsed = 0;
        this.pool = pool;
        isUsed = true;
        changed = false;
        Quaternion offset = Quaternion.Euler(0, -90, 0);
        transform.rotation = Quaternion.LookRotation(target) * offset;
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
        Ground();
        Move();
        if (changed || Elapsed < 1) return;
        changed = true;
        Speed = Speed * 10;
    }
    private void Move()
    {
        transform.position += Speed * Time.deltaTime * TargetDirection;
    }
    private void Ground()
    {
        Ray ray = new(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.9f, groundLayer))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + 1.5f;
            transform.position = newPosition;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isUsed) return;
        if (((1 << other.gameObject.layer) & WallLayer.value) != 0)
        {
            Expire();
        }
        if (((1 << other.gameObject.layer) & TargetLayer.value) != 0)
        {
            if (hitTargets.Contains(other.gameObject)) return;
            EnemyHealthManager enemy = other.gameObject.GetComponent<EnemyHealthManager>();
            foreach (var damage in Damage)
            {
                enemy.TakeDamage(damage.Value.Value);
            }
            hitTargets.Add(other.gameObject);
            Pierce--;
            if (Pierce == 0)
            {
                Expire();
            }
        }
    }
    public void Expire()
    {
        hitTargets.Clear();
        isUsed = false;
        OnExpired?.Invoke(this.gameObject, pool);
    }
}
