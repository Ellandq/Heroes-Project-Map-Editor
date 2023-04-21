using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
    [SerializeField] private CameraManager cameraManager; // Reference to the camera manager script
    [SerializeField] private GameObject inputManager; // Reference to the object containing the input manager script
    [SerializeField] internal Transform cameraFollowObject; // The object the camera should follow
    [SerializeField] private GameObject objectToMoveTowards; // The object the camera should move towards when teleported

    [Header("Camera Options")]
    [SerializeField] private int speed = 10; // The camera's movement speed
    [SerializeField] private int maxSpeed = 15; // The camera's maximum movement speed
    [SerializeField] private int distanceFromBoundary = 50; // The distance from the edge of the screen at which the camera should start moving
    [SerializeField] private int minDistanceFromBoundary = 5; // The minimum distance from the edge of the screen at which the camera should move
    [SerializeField] private float rotation; // The camera's current rotation
    [SerializeField] private float moveSpeed; // The camera's current movement speed
    [SerializeField] private Vector2 cameraMoveLimit; // The limit of the camera's movement on the X and Z axes

    internal Vector3 position; // The camera's current position
    private Vector3 cameraOffset; // The offset between the camera and the object it's following
    private Vector3 adjustedMovementVector; // The adjusted movement vector based on the camera's rotation

    private MouseInput mouseInput; // The mouse input manager script
    private KeyboardInput keyboardInput; // The keyboard input manager script

    private int screenWidth; // The width of the screen
    private int screenHeight; // The height of the screen

    private void Awake()
    {
        // Get the mouse and keyboard input manager scripts
        mouseInput = inputManager.GetComponent<MouseInput>();
        keyboardInput = inputManager.GetComponent<KeyboardInput>();
    }

    private void Start () 
    {
        // Set the screen width and height
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
	
    // Check what type of movement if any is supposed to run every frame
    private void Update () 
    {
        if (cameraManager.cameraEnabled){
            // Move the camera manually if the camera manager script says it's enabled
            ManualCameraMovement();

            // Clamp the camera's position to the camera move limit
            position.x = Mathf.Clamp(position.x, 0, cameraMoveLimit.x);
            position.z = Mathf.Clamp(position.z, -20, cameraMoveLimit.y);

            // Update the camera's position, the object it's following, and its rotation
            transform.position = position;
            cameraFollowObject.transform.position = position;
            rotation = transform.localEulerAngles.y;
        }
    }

    // Movement based on keyboard input
    private void ManualCameraMovement()
    {
        position = transform.position;
        adjustedMovementVector = CalculateMovementAngle();

        // if(keyboardInput.isRightPressed){
        //     cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
        //     position = cameraFollowObject.transform.position;
        // }
        // if(keyboardInput.isLeftPressed){
        //    cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis 
        //    position = cameraFollowObject.transform.position;
        // }
        // if(keyboardInput.isUpPressed){
        //     cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
        //     position = cameraFollowObject.transform.position;
        // }
        // if(keyboardInput.isDownPressed){
        //     cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
        //     position = cameraFollowObject.transform.position;
        // }
        transform.position = position;  
    }

    public void UpdateMovementBorder (int gridSize)
    {
        cameraMoveLimit.x = (gridSize * 5);
        cameraMoveLimit.y = ((gridSize * 5) - 20);
    }

    public void CameraTeleportToWorldObject ()
    {
        cameraFollowObject.transform.position = (objectToMoveTowards.transform.position + CalculateCameraOffset());
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    // Calculates the position based on the camera rotation, angle and the selected object position
    public Vector3 CalculateCameraOffset ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y - objectToMoveTowards.transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Camera offset independent of any object 
    public Vector3 CalculateCameraOffsetIndependent ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Vector at which the camera is supposed to move to move forwards
    public Vector3 CalculateMovementAngle ()
    {
        double y = System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(y), 0, Convert.ToSingle(x));
    }

    // Returns the position around which the camera is supposed to rotate
    public Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }

    public Vector2 GetCameraMoveLimit (){
        return cameraMoveLimit;
    }
}

