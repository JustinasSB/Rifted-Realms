using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float ledgeCheckDistance = 0.2f;
    public float maxDropDistanceForLedge = 0.4f;
    private CharacterController controller;
    private Stat movementSpeed;
    private Stat animationSpeed;
    public Vector3 MovementDirection;
    public bool isMoving;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementSpeed = PlayerStatsManager.playerStats.GetStat(StatType.MovementSpeed);
        animationSpeed = PlayerStatsManager.playerStats.GetStat(StatType.AnimationSpeed);
    }

    void Update()
    {
        Move();
        if (!isMoving) return;
        GroundPlayer();
    }

    private void Move() {
        float moveZ = Input.GetAxisRaw("Horizontal");
        float moveX = -Input.GetAxisRaw("Vertical");
        if (moveZ == 0 && moveX == 0)
        {
            isMoving = false;
            return;
        }
        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized;
        if (IsLedgeAhead(move))
        {
            move = Vector3.zero;
        }
        if (move != Vector3.zero)
        {
            MovementDirection = move;
            isMoving = true;
        }
        else 
        {
            MovementDirection = Vector3.zero;
            isMoving = false;
        }
        CollisionFlags flags =  controller.Move(move * movementSpeed.Value * animationSpeed.Value * Time.deltaTime);
        if ((flags & CollisionFlags.Sides) != 0)
        {
            MovementDirection = Vector3.zero;
            isMoving = false;
        }
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
        return !Physics.Raycast(ray, controller.height + maxDropDistanceForLedge);
    }
}
