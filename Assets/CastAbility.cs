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
        if (Input.GetMouseButtonDown(0))
        {
            playerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(playerStatsManager.playerStats.Stats[StatType.CurrentLife].Value - 50);
        }

        // Right Click (Reduce Mana)
        if (Input.GetMouseButtonDown(1))
        {
            animator.PlayAnimation((float)0.67, AbilityType.Spell);
            playerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(playerStatsManager.playerStats.Stats[StatType.CurrentMana].Value - 10);
            playerStatsManager.playerStats.Stats[StatType.CurrentEnergy].DirectValueSet(playerStatsManager.playerStats.Stats[StatType.CurrentEnergy].Value - 10);
        }
    }
}
