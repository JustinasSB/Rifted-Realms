using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BasicEnemy : MonoBehaviour, IMovement, IMovementData
{
    public Stat speed { get; set; }
    public Stat animationSpeed { get; set; }
    public Transform target;
    public bool isMoving { get; set; }
    public Vector3 MovementDirection { get; set; }
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject LeftFootIk;
    [SerializeField] GameObject RightFootIk;
    [SerializeField] EnemyDeathManager deathManager;
    bool dead = false;

    void Start()
    {
        speed = new(3f, StatType.MovementSpeed);
        animationSpeed = new(1f, StatType.AnimationSpeed);
        LeftFootIk.SetActive(true);
        RightFootIk.SetActive(true);
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                isMoving = false;
                MovementDirection = Vector3.zero;
                Debug.LogWarning("Player object not found in the scene.");
                return;
            }
        }
        deathManager.OnDeath += Dead;
    }
    private void Dead()
    {
        if (controller != null)
            controller.enabled = false;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;
        dead = true;
    }
    void Update()
    {
        if (dead) return;
        Ground();
        isMoving = true;
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed.Value * Time.deltaTime;
        MovementDirection = direction;
    }
    private void Ground()
    {
        Ray ray = new(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f, groundLayer))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y; //+ controller.height;
            transform.position = newPosition;
        }
    }
}
public interface IMovementData
{
    Stat speed { get; set; }
    Stat animationSpeed { get; set; }
}
