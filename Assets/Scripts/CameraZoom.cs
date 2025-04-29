using Unity.Mathematics;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    [SerializeField] GameObject Panel;
    [SerializeField] Transform playerCamera;
    private float zoomLevel = 1f;
    [SerializeField] float ZoomRate = -0.75f;
    private quaternion lowRotation = new quaternion(0.20121f, -0.71903f, 0.23206f, 0.62342f);
    private quaternion highRotation = new quaternion(0.25540f, -0.69577f, 0.29457f, 0.60325f);
    private Vector3 lowPosition = new Vector3(6.48f, 5.31f, 0.93f);
    private Vector3 highPosition = new Vector3(16.72f, 17.43f, 2.39f);
    private void Update()
    {
        if (Panel.activeSelf) return;
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return;
        zoomLevel = Mathf.Clamp(zoomLevel + ZoomRate * scroll, 0, 1f);
        playerCamera.localPosition = Vector3.Lerp(lowPosition, highPosition, zoomLevel);
        playerCamera.rotation = Quaternion.Slerp(lowRotation, highRotation, zoomLevel);
    }
}
