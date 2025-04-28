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
        Ground();
        Move();
        TimeUntilChange -= Time.deltaTime;
        if (TimeUntilChange > 0) return;
        TimeUntilChange = 1f;
        Speed = UnityEngine.Random.Range(BaseSpeed/2,BaseSpeed*2);
        TargetDirection = Quaternion.Euler(0, UnityEngine.Random.Range(0, 359), 0) * TargetDirection;
    }
    private void Move()
    {
        if (canMoveAhead(TargetDirection))
        {
            TimeUntilChange = 1f;
            Speed = UnityEngine.Random.Range(BaseSpeed / 2f, BaseSpeed * 2f);
            TargetDirection.x *= -1;
            TargetDirection.z *= -1;
        }
        transform.position += Speed * Time.deltaTime * TargetDirection;
    }
    private bool canMoveAhead(Vector3 direction)
    {
        Vector3 rayOrigin = transform.position + Vector3.up + direction.normalized * 1f;
        Ray ray = new(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f, groundLayer))
        {
            float heightDifference = rayOrigin.y - hit.point.y;
            // Check if ground is too far down (ledge) OR ground is too high (obstacle)
            if (heightDifference > 3f || heightDifference < 1.75f)
            {
                return true;
            }
            return false;
        }

        // No ground detected, treat it as a ledge
        return true;
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
