using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class RigRotateToMouse : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Transform Model;
    [SerializeField] Camera Camera;
    [SerializeField] LayerMask layer;
    Vector2 screenCenter;
    private void Start()
    {
        Target.position = transform.position;
    }
    void Update()
    {
        if (DeathManager.Dead)
        {
            Model.rotation = Quaternion.Euler(90, 0, 0);
            return;
        }
        else
        {
            Model.rotation = Quaternion.Euler(0, 0, 0);
        }
        screenCenter = new Vector2(Screen.width / 2, (Screen.height / 2) - 100);
        float distanceFromCenter = Vector2.Distance(Input.mousePosition, screenCenter);
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distanceFromCenter, layer))
        {
            Target.position = hit.point;
        }
        Vector3 lookPos = new Vector3(Target.position.x, Model.position.y, Target.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookPos - Model.position);
        Model.rotation = lookRotation;
    }
}
