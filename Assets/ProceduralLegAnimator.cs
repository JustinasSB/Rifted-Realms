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
    [SerializeField] PlayerMovement movement;
    [SerializeField] float scale;
    [SerializeField] SkinnedMeshRenderer material;
    bool stepping = false;
    Stat speed;
    private Stat animationSpeed;
    float lerp;
    Vector3 currentposition;
    Vector3 oldPosition;
    Vector3 newposition;
    Vector3 usedMovementDirection;
    float endMovement;
    private void Start()
    {
        speed = PlayerStatsManager.playerStats.GetStat(StatType.MovementSpeed);
        animationSpeed = PlayerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
        currentposition = transform.position;
    }
    void Update()
    {
        transform.position = currentposition;
        Ray ray;
        Vector3 bodyposition = body.transform.position;
        bodyposition.y -= body.height;
        if (movement.isMoving)
        {
            ray = new Ray(bodyposition + movement.MovementDirection * speed.Value * scale + Vector3.up * stepHeight + Skeleton.transform.right * footSpacing , Vector3.down);
        }
        else 
        {
            ray = new Ray(bodyposition + (Skeleton.transform.forward * footSpacing) + Skeleton.transform.right * footSpacing + Vector3.up * stepHeight , Vector3.down);
        }
        Debug.DrawRay(bodyposition + Skeleton.transform.forward * footSpacing + Vector3.up * stepHeight, Vector3.down*1, Color.red);
        if (Physics.Raycast(ray, out RaycastHit info, 2, layer))
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
            if (stepping && movement.MovementDirection != usedMovementDirection) 
            {
                endMovement = 2;
                usedMovementDirection = movement.MovementDirection;
                newposition = bodyposition + (Skeleton.transform.forward * footSpacing) + Skeleton.transform.right * footSpacing;
            }
        }
        if (lerp < 1 )
        {
            if (!otherFoot.stepping)
            {
                stepping = true;
                material.material.color = Color.red;
                Vector3 footPosition = Vector3.Lerp(oldPosition, newposition, lerp);
                footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
                currentposition = footPosition;
                lerp += Time.deltaTime * speed.Value * animationSpeed.Value * endMovement;
            }
        }
        else {
            oldPosition = newposition;
            endMovement = 1;
            usedMovementDirection = Vector3.zero;
            stepping = false;
            material.material.color = Color.white;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(currentposition, 0.5f);
    }
}
