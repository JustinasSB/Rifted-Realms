using Unity.VisualScripting;
using UnityEngine;

public class ProceduralLegAnimator : MonoBehaviour
{
    [SerializeField] CharacterController body;
    [SerializeField] Transform Skeleton;
    [SerializeField] float footSpacing;
    [SerializeField] LayerMask layer;
    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;
    [SerializeField] ProceduralLegAnimator otherFoot;
    [SerializeField] MonoBehaviour movementInterface;
    [SerializeField] float scale;
    bool stepping = false;
    Stat speed;
    Stat animationSpeed;
    float lerp;
    Vector3 currentposition;
    Vector3 oldPosition;
    Vector3 newposition;
    Vector3 usedMovementDirection;
    float endMovement;
    private float stepTimer = 0f;
    private float maxStepDuration = 0.2f;
    private IMovement movement => movementInterface as IMovement;
    [SerializeField] private MonoBehaviour movementDataProvider;
    private IMovementData movementData => movementDataProvider as IMovementData;
    private void Start()
    {
        if (movementData == null)
        {
            speed = PlayerStatsManager.playerStats.GetStat(StatType.MovementSpeed);
            animationSpeed = PlayerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
        }
        else
        {
            speed = new Stat(5f, StatType.MovementSpeed);//movementData.speed;
            animationSpeed = movementData.animationSpeed;
        }
        currentposition = transform.position;
    }
    void Update()
    {
        transform.position = currentposition;
        Ray ray;
        Vector3 bodyposition = body.transform.position;
        bodyposition.y -= (body.height / 2 - body.center.y);
        if (movement.isMoving)
        {
            ray = new Ray(bodyposition + movement.MovementDirection * speed.Value * scale + Vector3.up * stepHeight + Skeleton.transform.right * footSpacing , Vector3.down);
        }
        else 
        {
            ray = new Ray(bodyposition + (Skeleton.transform.forward * footSpacing) + Skeleton.transform.right * footSpacing + Vector3.up * stepHeight , Vector3.down);
        }
        //Debug.DrawRay(bodyposition + movement.MovementDirection * speed.Value * scale + Vector3.up * stepHeight + Skeleton.transform.right * footSpacing, Vector3.down*10, Color.red);
        if (Physics.Raycast(ray, out RaycastHit info, 8, layer))
        {
            //Debug.Log(Vector3.Distance(newposition, info.point));
            if (Vector3.Distance(newposition, info.point) > stepDistance && !stepping && movement.isMoving)
            {
                lerp = 0;
                newposition = info.point;
                usedMovementDirection = movement.MovementDirection;
            }
            if (!movement.isMoving && !stepping && Vector3.Distance(newposition, info.point) > stepDistance/10) 
            {
                lerp = 0;
                newposition = info.point;
                usedMovementDirection = movement.MovementDirection;
            }
            if (movementData == null)
            {
                if (stepping && movement.MovementDirection != usedMovementDirection)
                {
                    endMovement = 2;
                    usedMovementDirection = movement.MovementDirection;
                    newposition = bodyposition + (Skeleton.transform.forward * footSpacing) + Skeleton.transform.right * footSpacing;
                }
            }
        }
        if (lerp < 1 )
        {
            if (!otherFoot.stepping)
            {
                stepping = true;
                Vector3 footPosition = Vector3.Lerp(oldPosition, newposition, lerp);
                footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
                currentposition = footPosition;
                lerp += Time.deltaTime * speed.Value * animationSpeed.Value * endMovement;

                // prevent steps from running too long
                stepTimer += Time.deltaTime;
                if (stepTimer >= maxStepDuration)
                {
                    lerp = 1f; // Force step to complete
                    stepTimer = 0f;
                }
            }
        }
        else {
            oldPosition = newposition;
            endMovement = 1;
            usedMovementDirection = Vector3.zero;
            stepping = false;
            stepTimer = 0f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(currentposition, 0.5f);
    }
}