using UnityEngine;

public class CastAbility : MonoBehaviour
{
    AbilityAnimator animator;
    AbilityItem ability;
    void Start()
    {
        animator = GetComponent<AbilityAnimator>();
        animator.SetWeaponType(0);
        AbilityEvents.OnAbilityEquipped += OnAbilityEquipped;
    }

    void Update()
    {
        if (ability == null) return;
        if (Input.GetMouseButton(0))
        {
            //PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].Value - 50);
        }

        // Right Click (Reduce Mana)
        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].Value > ability.ManaCost)
            {
                animator.PlayAnimation(ability.ability.Stats[StatType.CastingSpeed].Item1.Value, AbilityType.Spell, ability);
                PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].Value - ability.ManaCost);
            }
            //PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].Value - 10);
        }
    }
    private void OnAbilityEquipped(AbilityItem a)
    {
        ability = a;
    }
}
