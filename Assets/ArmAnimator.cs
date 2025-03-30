using UnityEngine;

public class ArmAnimator : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] CharacterController body;
    [SerializeField] Transform Skeleton;
    [SerializeField] Transform LeftArmTarget;
    [SerializeField] Transform RightArmTarget;
    [SerializeField] Transform LeftArmHint;
    [SerializeField] Transform RightArmHint;
    private Stat attackSpeed;
    private Stat castingSpeed;
    private Stat animationSpeed;
    public int WeaponType;
    private bool animationPlaying;
    private float animationTime;
    private Vector3 LeftArmBasePosition;
    private Vector3 LeftArmCurrentPosition;
    private Vector3 LeftArmTargetPosition;
    private Vector3 LeftArmHintBasePosition;
    private Vector3 LeftArmHintCurrentPosition;
    private Vector3 RightArmBasePosition;
    private Vector3 RightArmCurrentPosition;
    private Vector3 RightArmTargetPosition;
    private Vector3 RightArmHintBasePosition;
    private Vector3 RightArmHintCurrentPosition;
    void Start()
    {
        playerStatsManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsManager>();
        attackSpeed = playerStatsManager.playerStats.GetStat(StatType.AttackSpeed);
        castingSpeed = playerStatsManager.playerStats.GetStat(StatType.CastingSpeed);
        animationSpeed = playerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
        WeaponType = 0;
        LeftArmBasePosition = LeftArmTarget.transform.position;
        RightArmBasePosition = RightArmTarget.transform.position;
    }
    void Update()
    {
        if (animationPlaying) 
        {
            
        }
    }
    void PlayAnimation(float AnimationTime)
    {
        animationTime = AnimationTime;
        switch (WeaponType)
        {
            case 0:
                HandAnimation();
                break;
            case 1:
                break;
        }
    }
    void HandAnimation()
    {
    }
}
