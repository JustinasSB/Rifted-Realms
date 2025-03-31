using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AbilityAnimator : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    private CharacterController body;
    [SerializeField] Transform Skeleton;
    [SerializeField] Transform LeftArmTarget;
    [SerializeField] Transform RightArmTarget;
    [SerializeField] Transform LeftArmHint;
    [SerializeField] Transform RightArmHint;
    private Stat attackSpeed;
    private Stat castingSpeed;
    private Stat animationSpeed;
    private WeaponType weapon;
    private AbilityType ability;
    private bool animationPlaying;
    private float animationTime;
    private float elapsedTime;
    private float bufferedAnimation;
    private AbilityType bufferedAbility;

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

    private bool Triggered;
    void Start()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        body = GetComponent<CharacterController>();
        attackSpeed = playerStatsManager.playerStats.GetStat(StatType.AttackSpeed);
        castingSpeed = playerStatsManager.playerStats.GetStat(StatType.CastingSpeed);
        animationSpeed = playerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
        weapon = 0;
        leftArmBasePosition = LeftArmTarget.transform.localPosition;
        leftArmHintBasePosition = LeftArmHint.transform.localPosition;
        leftArmBaseRotation = LeftArmTarget.transform.localRotation;
        rightArmBasePosition = RightArmTarget.transform.localPosition;
        rightArmHintBasePosition = RightArmHint.transform.localPosition;
        rightArmBaseRotation = RightArmTarget.transform.localRotation;
    }
    void Update()
    {
        LeftArmTarget.transform.localPosition = leftArmCurrentPosition;
        LeftArmTarget.transform.localRotation = leftArmCurrentRotation;
        LeftArmHint.transform.localPosition = leftArmHintCurrentPosition;
        RightArmTarget.transform.localPosition = rightArmCurrentPosition;
        RightArmTarget.transform.localRotation = rightArmCurrentRotation;
        RightArmHint.transform.localPosition = rightArmHintCurrentPosition;
        if (animationPlaying) 
        {
            elapsedTime += GetScaledDeltaTime(ability);
            float NormalizedElapsedTime = Mathf.Clamp01(elapsedTime / animationTime);
            //lerp factor climbs from 0 to 1 during the first half of animation time, then from 1 to 0 to return to base position 
            float lerpFactor = (NormalizedElapsedTime < 0.5f) ? (NormalizedElapsedTime * 2f) : ((1f - NormalizedElapsedTime) * 2f);

            leftArmCurrentPosition = Vector3.Lerp(leftArmBasePosition, leftArmTargetPosition, lerpFactor);
            leftArmCurrentRotation = Quaternion.Slerp(leftArmBaseRotation, leftArmTargetRotation, lerpFactor);
            leftArmHintCurrentPosition = Vector3.Lerp(leftArmHintBasePosition, leftArmHintTargetPosition, lerpFactor);

            rightArmCurrentPosition = Vector3.Lerp(rightArmBasePosition, rightArmTargetPosition, lerpFactor);
            rightArmCurrentRotation = Quaternion.Slerp(rightArmBaseRotation, rightArmTargetRotation, lerpFactor);
            rightArmHintCurrentPosition = Vector3.Lerp(rightArmHintBasePosition, rightArmHintTargetPosition, lerpFactor);

            if (!Triggered && NormalizedElapsedTime >= 0.5f)
            {
                Triggered = true;
                //do some ability
            }
            if (NormalizedElapsedTime >= 1) 
            {
                animationPlaying = false;
                Triggered = false;
                if (bufferedAnimation == 0 && bufferedAbility == 0)
                {
                    leftArmHintCurrentPosition = leftArmHintBasePosition;
                    rightArmHintCurrentPosition = rightArmHintBasePosition;
                }
                else 
                {
                    PlayAnimation(bufferedAnimation, bufferedAbility);
                }
                bufferedAnimation = 0;
                bufferedAbility = 0;
            }
        }
    }
    public void PlayAnimation(float AnimationTime, AbilityType Ability)
    {
        if (!animationPlaying)
        {
            animationTime = AnimationTime;
            ability = Ability;
            elapsedTime = 0;
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
        else if (animationTime-elapsedTime-Time.deltaTime*2<0)
        {
            bufferedAnimation = AnimationTime;
            bufferedAbility = ability;
        }
    }
    public void SetWeaponType(WeaponType type)
    {
        this.weapon = type;
    }
    private float GetScaledDeltaTime(AbilityType Ability)
    {
        switch ((int)Ability)
        {
            case 1:
            case 2:
                return Time.deltaTime * castingSpeed.Value * animationSpeed.Value;
            case 3:
            case 4:
                return Time.deltaTime * attackSpeed.Value * animationSpeed.Value;
            default:
                Debug.Log("ability type unset");
                return Time.deltaTime;

        }
    }
    private void SpellHandAnimation()
    {
        leftArmTargetPosition = new Vector3((float)-0.016, (float)-0.083, (float)0.33);
        leftArmTargetRotation = new quaternion((float)0.1503837, (float)-0.4924039, (float)-0.8528685, (float)0.0868241);
        leftArmHintTargetPosition = new Vector3((float)-0.0866, (float)0.0525, (float)0.2774);
        rightArmTargetPosition = new Vector3((float)0.0329, (float)-0.0975, (float)0.2808);
        rightArmTargetRotation = new quaternion((float)0.2668861, (float)-0.5242557, (float)-0.8040591, (float)-0.0861205);
        rightArmHintTargetPosition = new Vector3((float)0.1282, (float)0.0525, (float)0.262);
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
}
