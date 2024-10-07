using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Constants")]
    private const float minCameraHeight = 5;
    private const float maxCameraHeight = 15;

    [Header("Values")]
    public float cameraHeight;
    public float panSpeed;
    public float panBorderThickness;
    public float zoomSpeed;

    private void Start()
    {
        // Set camera to desired height
        Vector3 newPosition = transform.position;
        newPosition.y = cameraHeight;
        transform.position = newPosition;
    }

    private void Update()
    {
        MoveCamera();
    }

    /// <summary>
    /// Updates the camera's position based on player input
    /// </summary>
    private void MoveCamera()
    {
        // Get current position of camera
        Vector3 newPosition = transform.position;

        // Add movement from player inputs
        if (Input.GetKey(Keybinds.instance.forwardKey) || (Settings.instance.edgePanning && Input.mousePosition.y >= Screen.height - panBorderThickness))
        {
            newPosition.z += panSpeed / 2 * Time.deltaTime;
            newPosition.x -= panSpeed / 2 * Time.deltaTime;
        }
        if (Input.GetKey(Keybinds.instance.backwardKey) || (Settings.instance.edgePanning && Input.mousePosition.y <= panBorderThickness))
        {
            newPosition.z -= panSpeed / 2 * Time.deltaTime;
            newPosition.x += panSpeed / 2 * Time.deltaTime;
        }
        if (Input.GetKey(Keybinds.instance.leftKey) || (Settings.instance.edgePanning && Input.mousePosition.x <= panBorderThickness))
        {
            newPosition.x -= panSpeed / 2 * Time.deltaTime;
            newPosition.z -= panSpeed / 2 * Time.deltaTime;
        }
        if (Input.GetKey(Keybinds.instance.rightKey) || (Settings.instance.edgePanning && Input.mousePosition.x >= Screen.width - panBorderThickness))
        {
            newPosition.x += panSpeed / 2 * Time.deltaTime;
            newPosition.z += panSpeed / 2 * Time.deltaTime;
        }

        // Set camera height based on scroll input
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 alteredZoom = newPosition + transform.forward * zoom * zoomSpeed * 100f * Time.deltaTime;
        if (alteredZoom.y > minCameraHeight && alteredZoom.y < maxCameraHeight)
            newPosition += transform.forward * zoom * zoomSpeed * 100f * Time.deltaTime;

        // Limit position to map constraints
        newPosition.x = Mathf.Clamp(newPosition.x, -MapController.instance.mapSize / 2 + (5.5f * newPosition.y / 10), MapController.instance.mapSize / 2 + (5.5f * newPosition.y / 10));
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraHeight, maxCameraHeight);
        newPosition.z = Mathf.Clamp(newPosition.z, -MapController.instance.mapSize / 2 - (5.5f * newPosition.y / 10), MapController.instance.mapSize / 2 - (5.5f * newPosition.y / 10));

        // Update position of camera to new position
        transform.localPosition = newPosition;
    }
}
