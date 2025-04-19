using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform cameraHolder; // Assign PhotoCameraHolder in the Inspector

    float xRotation;
    float yRotation;

    [Header("Camera Raise Settings")]
    public Vector3 defaultCamPosition; // Store the default local position
    public Vector3 raisedCamPosition; // Store the raised local position
    public float camMoveSpeed = 5f; // Speed of the camera movement

    public bool isCameraRaised = false; // Toggle state

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store the initial local position of the camera holder
        defaultCamPosition = cameraHolder.localPosition;
        raisedCamPosition = defaultCamPosition + new Vector3(0, 0.5f, -0.2f); // Move up slightly
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 30f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Toggle camera position on right-click
        if (Input.GetMouseButtonDown(1)) // Detects single click instead of holding
        {
            isCameraRaised = !isCameraRaised; // Toggle state
        }

        // Smooth transition based on the toggle state
        Vector3 targetPosition = isCameraRaised ? raisedCamPosition : defaultCamPosition;
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPosition, Time.deltaTime * camMoveSpeed);
    }
}
