using NUnit.Framework;
using System.Collections.Generic;
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

    public void Initialize(Dictionary<StatType, Stat> damage, LayerMask targetLayer, Vector3 target, float pierce, float duration, float speed)
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
        isUsed = true;
    }
    void Update()
    {
        if (!isUsed) return;
        Elapsed += Time.deltaTime;
        if (Elapsed > Duration)
        {
            isUsed = false;
            return;
        }
        transform.Rotate(20 * Speed * Time.deltaTime * TargetDirection);
        transform.position += Speed * Time.deltaTime * TargetDirection;
        Ground();
        TimeUntilChange -= Time.deltaTime;
        if (TimeUntilChange > 0) return;
        TimeUntilChange = 1f;
        Speed = Random.Range(BaseSpeed/2,BaseSpeed*2);
        TargetDirection = Quaternion.Euler(0, Random.Range(0, 359), 0) * TargetDirection;
    }
    private void Ground()
    {
        Ray ray = new(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, groundLayer))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + 1.5f;
            transform.position = newPosition;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & WallLayer.value) != 0)
        {
            TimeUntilChange = 1f;
            Speed = Random.Range(BaseSpeed / 2f, BaseSpeed * 2f);
            TargetDirection.x *= -1;
            TargetDirection.z *= -1;
        }
    }
}
