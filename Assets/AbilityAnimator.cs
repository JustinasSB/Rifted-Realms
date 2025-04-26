using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AbilityAnimator : MonoBehaviour
{
    private CharacterController body;
    [SerializeField] Transform Skeleton;
    [SerializeField] Transform LeftArmTarget;
    [SerializeField] Transform RightArmTarget;
    [SerializeField] Transform LeftArmHint;
    [SerializeField] Transform RightArmHint;
    [SerializeField] Transform Target;
    [SerializeField] LayerMask targetLayer;
    private Stat attackSpeed;
    private Stat castingSpeed;
    private Stat animationSpeed;
    private WeaponType weapon;
    private AbilityType ability;
    private AbilityItem abilityItem;
    private bool animationPlaying;
    private float animationTime;
    private float elapsedTime;
    private float bufferedAnimation;
    private AbilityType bufferedAbility;
    private AbilityItem bufferedAbilityItem;

    private Vector3 leftArmBasePosition;
    private Vector3 leftArmCurrentPosition;
    private Vector3 leftArmTargetPosition;
    private Vector3 leftArmHintBasePosition;
    private Vector3 leftArmHintCurrentPosition;
    private Vector3 leftArmHintTargetPosition;
    private quaternion leftArmBaseRotation;
    private quaternion leftArmCurrentRotation;
    private quaternion leftArmTargetRotation;

    private Vector3 rightArmBasePosition;
    private Vector3 rightArmCurrentPosition;
    private Vector3 rightArmTargetPosition;
    private Vector3 rightArmHintBasePosition;
    private Vector3 rightArmHintCurrentPosition;
    private Vector3 rightArmHintTargetPosition;
    private quaternion rightArmBaseRotation;
    private quaternion rightArmCurrentRotation;
    private quaternion rightArmTargetRotation;

    private bool settled = false;

    private bool Triggered;
    void Start()
    {
        body = GetComponent<CharacterController>();
        attackSpeed = PlayerStatsManager.playerStats.GetStat(StatType.AttackSpeed);
        castingSpeed = PlayerStatsManager.playerStats.GetStat(StatType.CastingSpeed);
        animationSpeed = PlayerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
        weapon = 0;
        leftArmBasePosition = LeftArmTarget.transform.localPosition;
        leftArmHintBasePosition = LeftArmHint.transform.localPosition;
        leftArmBaseRotation = LeftArmTarget.transform.localRotation;
        rightArmBasePosition = RightArmTarget.transform.localPosition;
        rightArmHintBasePosition = RightArmHint.transform.localPosition;
        rightArmBaseRotation = RightArmTarget.transform.localRotation;
        LoadIdleAnimation();
    }
    void Update()
    {
        if (settled) return;
        LeftArmTarget.transform.localPosition = leftArmCurrentPosition;
        LeftArmTarget.transform.localRotation = leftArmCurrentRotation;
        LeftArmHint.transform.localPosition = leftArmHintCurrentPosition;
        RightArmTarget.transform.localPosition = rightArmCurrentPosition;
        RightArmTarget.transform.localRotation = rightArmCurrentRotation;
        RightArmHint.transform.localPosition = rightArmHintCurrentPosition;
        
        if (animationPlaying)
        {
            elapsedTime += getScaledDeltaTime(ability);
            float NormalizedElapsedTime = Mathf.Clamp01(elapsedTime / animationTime);
            //lerp factor climbs from 0 to 1 during the first half of animation time, then from 1 to 0 to return to base position 
            float lerpFactor = (NormalizedElapsedTime < 0.5f) ? (NormalizedElapsedTime * 2f) : ((1f - NormalizedElapsedTime) * 2f);
            interpolateMovement(lerpFactor);

            if (!Triggered && NormalizedElapsedTime >= 0.5f)
            {
                Triggered = true;
                TriggerAbility(abilityItem, LeftArmTarget, PlayerStatsManager.playerStats.Stats);
            }
            if (NormalizedElapsedTime >= 1)
            {
                animationPlaying = false;
                Triggered = false;
                if (bufferedAnimation == 0 && bufferedAbility == 0)
                {
                    prepareReturnToIdle();
                }
                else
                {
                    PlayAnimation(bufferedAnimation, bufferedAbility, bufferedAbilityItem);
                    bufferedAnimation = 0;
                    bufferedAbility = 0;
                }
            }
        }
        else if (leftArmCurrentPosition != leftArmBasePosition)
        {
            elapsedTime += getScaledDeltaTime(0);
            float NormalizedElapsedTime = Mathf.Clamp01(elapsedTime / animationTime);
            float lerpFactor = (1f - NormalizedElapsedTime);
            interpolateMovement(lerpFactor);
            if (lerpFactor == 0)
            {
                LeftArmTarget.transform.localPosition = leftArmCurrentPosition;
                LeftArmTarget.transform.localRotation = leftArmCurrentRotation;
                LeftArmHint.transform.localPosition = leftArmHintCurrentPosition;
                RightArmTarget.transform.localPosition = rightArmCurrentPosition;
                RightArmTarget.transform.localRotation = rightArmCurrentRotation;
                RightArmHint.transform.localPosition = rightArmHintCurrentPosition;
                settled = true;
            }
        }
    }
    private void prepareReturnToIdle()
    {
        elapsedTime = 0;
        animationTime = 0.5f;
        leftArmTargetPosition = leftArmCurrentPosition;
        leftArmTargetRotation = leftArmCurrentRotation;
        leftArmHintTargetPosition = leftArmHintCurrentPosition;
        rightArmTargetPosition = rightArmCurrentPosition;
        rightArmTargetRotation = rightArmCurrentRotation;
        rightArmHintTargetPosition = rightArmHintCurrentPosition;
        LoadIdleAnimation();
    }
    private void interpolateMovement(float lerpFactor)
    {
        leftArmCurrentPosition = Vector3.Lerp(leftArmBasePosition, leftArmTargetPosition, lerpFactor);
        leftArmCurrentRotation = Quaternion.Slerp(leftArmBaseRotation, leftArmTargetRotation, lerpFactor);
        leftArmHintCurrentPosition = Vector3.Lerp(leftArmHintBasePosition, leftArmHintTargetPosition, lerpFactor);

        rightArmCurrentPosition = Vector3.Lerp(rightArmBasePosition, rightArmTargetPosition, lerpFactor);
        rightArmCurrentRotation = Quaternion.Slerp(rightArmBaseRotation, rightArmTargetRotation, lerpFactor);
        rightArmHintCurrentPosition = Vector3.Lerp(rightArmHintBasePosition, rightArmHintTargetPosition, lerpFactor);
    }
    private void OnEnable()
    {
        EquipmentSlot.OnMainHandEquipmentChanged += UpdateWeapon;
    }

    private void OnDisable()
    {
        EquipmentSlot.OnMainHandEquipmentChanged -= UpdateWeapon;
    }
    private void UpdateWeapon(InventoryItem item)
    {
        if (item != null)
        {
            Enum.TryParse<WeaponType>(item.data.ItemSpecific.ToString(), out var weaponType);
            weapon = weaponType;
        }
        else
        {
            weapon = WeaponType.Unarmed;
        }
    }
    public void PlayAnimation(float AnimationTime, AbilityType Ability, AbilityItem AbilityData)
    {
        if (!animationPlaying)
        {
            animationTime = AnimationTime;
            ability = Ability;
            abilityItem = AbilityData;
            elapsedTime = 0;
            settled = false;
            switch ((int)weapon+(int)ability*100)
            {
                case 100:
                    SpellHandAnimation();
                    break;
                case 200:
                    BuffHandAnimation();
                    break;
                case 300:
                    AttackHandAnimation();
                    break;
                default:
                    Debug.Log("Weapon, ability pair has no case");
                    break;
            }
        }
        else if (animationTime-elapsedTime-animationTime*0.2<0)
        {
            bufferedAnimation = AnimationTime;
            bufferedAbility = ability;
            bufferedAbilityItem = AbilityData;
        }
    }
    public void SetWeaponType(WeaponType type)
    {
        this.weapon = type;
    }
    private float getScaledDeltaTime(AbilityType Ability)
    {
        switch ((int)Ability)
        {
            case 0:
                return Time.deltaTime * animationSpeed.Value;
            case 1:
            case 2:
                return Time.deltaTime * castingSpeed.Value * animationSpeed.Value;
            case 3:
            case 4:
                return Time.deltaTime * attackSpeed.Value * animationSpeed.Value;
            default:
                Debug.Log("ability type unset");
                return Time.deltaTime * animationSpeed.Value;

        }
    }
    private void SpellHandAnimation()
    {
        LoadAbilityBase();
        leftArmTargetPosition = new Vector3(-0.01539947f, -0.1010992f, 0.312396f);
        leftArmTargetRotation = new quaternion(-0.09592f, 0.53698f, 0.83658f, -0.05089f);
        leftArmHintTargetPosition = new Vector3(-0.0782f, 0.0525f, 0.262f);
        rightArmTargetPosition = new Vector3(0.03f, -0.0956f, 0.3197f);
        rightArmTargetRotation = new quaternion(-0.04082f, -0.49159f, -0.86964f, -0.02000f);
        rightArmHintTargetPosition = new Vector3(0.067f, 0.0525f, 0.262f);
        animationPlaying = true;
    }
    private void BuffHandAnimation()
    {
        animationPlaying = true;
    }
    private void AttackHandAnimation()
    {
        animationPlaying = true;
    }
    private void LoadAbilityBase()
    {
        switch ((int)weapon)
        {
            case 0:
                leftArmBasePosition = new Vector3(-0.03610138f, -0.02900007f, 0.2678969f);
                leftArmBaseRotation = new quaternion(0.1443452f, -0.539722f, 0.8137537f, -0.1602173f);
                leftArmHintBasePosition = new Vector3(-0.0687f, 0.0525f, 0.262f);
                rightArmBasePosition = new Vector3(0.03760367f, -0.05310554f, 0.2963893f);
                rightArmBaseRotation = new quaternion(0.0838614f, -0.582919f, 0.7199153f, -0.3672801f);
                rightArmHintBasePosition = new Vector3(0.0722f, 0.05254095f, 0.2619794f);
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    private void LoadIdleAnimation()
    {
        switch ((int)weapon) 
        {
            case 0:
                leftArmBasePosition = new Vector3(-0.06810334f, 0.0009017753f, 0.2167678f);
                leftArmBaseRotation = new quaternion(0.8271084f, -0.062163f, -0.0193626f, -0.5582584f);
                leftArmHintBasePosition = new Vector3(-0.0721f, 0.0525f, 0.262f);
                rightArmBasePosition = new Vector3(0.0674f, 0.0127f, 0.2224f);
                rightArmBaseRotation = new quaternion(-0.8236926f, -0.2241978f, -0.4545183f, 0.2543205f);
                rightArmHintBasePosition = new Vector3(0.0648f, 0.0525f, 0.262f);
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void TriggerAbility(AbilityItem abilityItem, Transform spawnOrigin, Dictionary<StatType, Stat> casterStats)
    {
        switch (abilityItem.ability.behaviour)
        {
            case AbilityBehaviourTag.Projectile:
                AbilityInitialiser.TriggerProjectile(abilityItem.effectPrefab, abilityItem.ability, abilityItem.DamageMultiplier, spawnOrigin, Target.position, casterStats, targetLayer);
                break;
            case AbilityBehaviourTag.Attack:
                break;
            case AbilityBehaviourTag.Slam:
                break;
            case AbilityBehaviourTag.TargetAOE:
                break;
            case AbilityBehaviourTag.CenterAOE:
                break;
        }
    }
}
