using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TempestBehaviour : MonoBehaviour
{
    public Ability abilityData;
    public Dictionary<StatType, Stat> Caster;
    private float speed;
    private float damage;
    private float pierce;
    public void Initialize(Ability data, float damageMultiplier)
    {
        speed = data.Stats[StatType.MovementSpeed].Item1.Value;
        pierce = data.Stats[StatType.Pierce].Item1.Value;
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {

    }
}
