using UnityEngine;

public class CastAbility : MonoBehaviour
{
    AbilityAnimator animator;
    PlayerStatsManager playerStatsManager;
    void Start()
    {
        animator = GetComponent<AbilityAnimator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        animator.SetWeaponType(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentLife].Value - 50);
        }

        // Right Click (Reduce Mana)
        if (Input.GetMouseButton(1))
        {
            animator.PlayAnimation((float)0.2, AbilityType.Spell);
            //PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentMana].Value - 10);
            //PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].DirectValueSet(PlayerStatsManager.playerStats.Stats[StatType.CurrentEnergy].Value - 10);
        }
    }
}
