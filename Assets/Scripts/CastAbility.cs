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
        if (ability == null || DeathManager.Dead) return;
        // Right Click (Reduce Mana)
        if (Input.GetMouseButton(1))
        {
            if (PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].Value > ability.ManaCost)
            {
                float castTime = ability.ability.Stats[StatType.CastingSpeed].Item1.Value;// / PlayerStatsManager.playerStats.Stats[StatType.CastingSpeed].Value;
                animator.PlayAnimation(castTime, AbilityType.Spell, ability);
            }
        }
    }
    private void OnAbilityEquipped(AbilityItem a)
    {
        ability = a;
    }
}
