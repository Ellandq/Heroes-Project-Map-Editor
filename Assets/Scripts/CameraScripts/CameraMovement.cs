using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement: MonoBehaviour 
{
    [SerializeField] internal CameraManager cameraManager;
    [SerializeField] private GameObject inputManager;
    [SerializeField] internal Transform cameraFollowObject;
    [SerializeField] private GameObject objectToMoveTowards;
    private MouseInput mouseInput;
    private KeyboardInput keyboardInput;

    private int screenWidth;
    private int screenHeight;

    [Header("Camera Options")]
    [SerializeField] private int speed = 10;
    [SerializeField] private int maxSpeed = 15;
    [SerializeField] private int distanceFromBoundary = 50;
    [SerializeField] private int minDistanceFromBoundary = 5;
    [SerializeField] internal Vector2 cameraMoveLimit;
    [SerializeField] private float rotation;
    [SerializeField] private float moveSpeed;
    internal Vector3 position;  
    private Vector3 cameraOffset; 
    private Vector3 adjustedMovementVector;

    private void Awake()
    {
        mouseInput = inputManager.GetComponent<MouseInput>();
        keyboardInput = inputManager.GetComponent<KeyboardInput>();
    }

	private void Start () 
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}
	
    // Check what type of movement if any is supposed to run every frame
	private void Update () 
    {
        if (cameraManager.cameraEnabled)
        {
            ManualCameraMovement();
        }
        
        position.x = Mathf.Clamp(position.x, 0, cameraMoveLimit.x);
        position.z = Mathf.Clamp(position.z, -20, cameraMoveLimit.y);

        transform.position = position;
        cameraFollowObject.transform.position = position;
        rotation = transform.localEulerAngles.y;
    }

    // Movement based on keyboard input
    private void ManualCameraMovement()
    {
        position = transform.position;
        adjustedMovementVector = CalculateMovementAngle();

        if(keyboardInput.isRightPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
        }
        if(keyboardInput.isLeftPressed){
           cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis 
           position = cameraFollowObject.transform.position;
        }
        if(keyboardInput.isUpPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
        }
        if(keyboardInput.isDownPressed){
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
        }
        transform.position = position;  
    }

    public void UpdateMovementBorder (Vector2Int gridSize)
    {
        cameraMoveLimit.x = (gridSize.x * 5);
        cameraMoveLimit.x = ((gridSize.y * 5) - 20);
    }

    public void CameraTeleportToWorldObject ()
    {
        cameraFollowObject.transform.position = (objectToMoveTowards.transform.position + CalculateCameraOffset());
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    // Calculates the position based on the camera rotation, angle and the selected object position
    internal Vector3 CalculateCameraOffset ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y - objectToMoveTowards.transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Camera offset independent of any object 
    internal Vector3 CalculateCameraOffsetIndependent ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Vector at which the camera is supposed to move to move forwards
    internal Vector3 CalculateMovementAngle ()
    {
        double y = System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(y), 0, Convert.ToSingle(x));
    }

    // Returns the position around which the camera is supposed to rotate
    internal Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }
}

