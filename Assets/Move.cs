using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float ledgeCheckDistance = 0.2f;
    public float maxDropDistanceForLedge = 0.4f;
    private PlayerStatsManager playerStatsManager;
    private CharacterController controller;
    private Stat Movement;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        Movement = playerStatsManager.playerStats.GetStat(StatType.MovementSpeed);
    }

    void Update()
    {
        Move();
        GroundPlayer();
        if (Input.GetMouseButtonDown(0))
        {
            playerStatsManager.playerStats.Stats[StatType.CurrentLife].DirectValueSet(playerStatsManager.playerStats.Stats[StatType.CurrentLife].Value - 50);
        }

        // Right Click (Reduce Mana)
        if (Input.GetMouseButtonDown(1))
        {
            playerStatsManager.playerStats.Stats[StatType.CurrentMana].DirectValueSet(playerStatsManager.playerStats.Stats[StatType.CurrentMana].Value - 10);
        }
    }

    private void Move() {
        float moveZ = Input.GetAxisRaw("Horizontal");
        float moveX = -Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        if (IsLedgeAhead(move))
        {
            move = Vector3.zero;
        }
        controller.Move(move * Movement.Value * Time.deltaTime);
    }

    private void GroundPlayer() {
        Ray ray = new(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + controller.height;
            transform.position = newPosition;
        }
    }
    private bool IsLedgeAhead(Vector3 direction)
    {
        Vector3 rayOrigin = transform.position;
        Ray ray = new(rayOrigin + direction.normalized * (controller.radius + ledgeCheckDistance), Vector3.down);
        //UnityEngine.Debug.DrawRay(rayOrigin + direction.normalized * (controller.radius + 0.2f), Vector3.down*100, Color.red);
        return !Physics.Raycast(ray, controller.height + maxDropDistanceForLedge);
    }
}
