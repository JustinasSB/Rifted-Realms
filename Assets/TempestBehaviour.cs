using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TempestBehaviour : MonoBehaviour, IProjectile
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
    private float BaseSpeed;
    private float TimeUntilChange;
    private ProjectilePool pool;
    public event Action<GameObject, ProjectilePool> OnExpired;

    public void Initialize(Dictionary<StatType, Stat> damage, LayerMask targetLayer, Vector3 target, float pierce, float duration, float speed, ProjectilePool pool)
    {
        Damage = damage;
        TargetLayer = targetLayer;
        TargetDirection = target;
        Pierce = pierce;
        Duration = duration;
        Speed = speed;
        BaseSpeed = speed;
        Elapsed = 0;
        TimeUntilChange = 1f;
        this.pool = pool;
        this.gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("_Speed", UnityEngine.Random.Range(1, 5));
        isUsed = true;
    }
    void Update()
    {
        if (!isUsed) return;
        Elapsed += Time.deltaTime;
        if (Elapsed > Duration)
        {
            isUsed = false;
            Expire();
            return;
        }
        Move();
        Ground();
        TimeUntilChange -= Time.deltaTime;
        if (TimeUntilChange > 0) return;
        TimeUntilChange = 1f;
        Speed = UnityEngine.Random.Range(BaseSpeed/2,BaseSpeed*2);
        TargetDirection = Quaternion.Euler(0, UnityEngine.Random.Range(0, 359), 0) * TargetDirection;
    }
    private void Move()
    {
        if (IsLedgeAhead(TargetDirection))
        {
            TimeUntilChange = 1f;
            Speed = UnityEngine.Random.Range(BaseSpeed / 2f, BaseSpeed * 2f);
            TargetDirection.x *= -1;
            TargetDirection.z *= -1;
        }
        transform.position += Speed * Time.deltaTime * TargetDirection;
    }
    private bool IsLedgeAhead(Vector3 direction)
    {
        Vector3 rayOrigin = transform.position;
        Ray ray = new(rayOrigin + 1.5f * direction, Vector3.down);
        return !Physics.Raycast(ray, 3f, groundLayer);
    }
    private void Ground()
    {
        Ray ray = new(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f, groundLayer))
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
            TimeUntilChange = 1f;
            Speed = UnityEngine.Random.Range(BaseSpeed / 2f, BaseSpeed * 2f);
            TargetDirection.x *= -1;
            TargetDirection.z *= -1;
        }
        if (((1 << other.gameObject.layer) & TargetLayer.value) != 0)
        {
            //do damage
        }
    }
    public void Expire()
    {
        OnExpired?.Invoke(this.gameObject, pool);
    }
}
