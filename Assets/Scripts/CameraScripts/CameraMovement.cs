using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
    [Header ("Object References")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Transform cameraFollowObject;
    [SerializeField] private Transform cameraRotationObject;
    [SerializeField] private GameObject objectToMoveTowards;

    [Header("Camera Movement Options")]
    [SerializeField] private float minSpeed; // The camera's movement speed
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed; // The camera's maximum movement speed
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.2f;

    [Header ("Camera Rotation Options")]
    [SerializeField] private float rotationSpeed;
    private bool isRotating;

    [Header ("Camera Zoom Options")]
    [SerializeField] private float zoomSpeed;
    private Vector3 initialPosition;
    
    [Header ("Camera Movement limits")]
    [SerializeField] private Vector2 cameraMoveLimit;
    [SerializeField] private Vector2 cameraVerticalMoveLimit;

    [Header ("Camera Information")]
    private Vector3 position;
    private Vector3 cameraOffset; 
    private Vector3 adjustedMovementVector; 
    private Vector3 centerOfRotation;
    private float zoomObjectRotation;
    private float rotation;
    private bool centerCalculated;
    private bool isMovingLeft;
    private bool isMovingRight;
    private bool isMovingForward;
    private bool isMovingBackward;

    private int screenWidth; // The width of the screen
    private int screenHeight; // The height of the screen

    private void Awake()
    {
        InputManager.Instance.keyboardInput.onLeftChangedState += ChangeMovementStatus_Left;
        InputManager.Instance.keyboardInput.onRightChangedState += ChangeMovementStatus_Right;
        InputManager.Instance.keyboardInput.onForwardChangedState += ChangeMovementStatus_Forward;
        InputManager.Instance.keyboardInput.onBackwardChangedState += ChangeMovementStatus_Backward;

        InputManager.Instance.mouseInput.onMiddleMouseButton_Down += ChangeRotationStatus;
        InputManager.Instance.mouseInput.onMiddleMouseButton_Up += ChangeRotationStatus;

        InputManager.Instance.mouseInput.onScrollWheelMove_Up += ZoomIn;
        InputManager.Instance.mouseInput.onScrollWheelMove_Down += ZoomOut;
        position = transform.position;
        initialPosition = position;
    }

    private void Start () 
    {
        // Set the screen width and height
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        currentSpeed = minSpeed;
    }
	
    // Check what type of movement if any is supposed to run every frame
    private void Update () 
    {
        if (isRotating){
            cameraManager.DisableCameraMovement();
            if (Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
            RotateCamera();
        }else{
            centerCalculated = false;
            Cursor.lockState = CursorLockMode.None;
            cameraManager.EnableCameraMovement();
        }
        if (cameraManager.cameraEnabled){
            
            if (cameraManager.cameraMovementEnabled){
                if (isMovingLeft) MoveLeft();
                if (isMovingRight) MoveRight();
                if (isMovingBackward) MoveBackward();
                if (isMovingForward) MoveForward();
            }
            // Clamp the camera's position to the camera move limit
            position.x = Mathf.Clamp(position.x, 0f, cameraMoveLimit.x);
            position.z = Mathf.Clamp(position.z, -20f, cameraMoveLimit.y);
            position.y = Mathf.Clamp(position.y, cameraVerticalMoveLimit.x, cameraVerticalMoveLimit.y);

            // Update the camera's position, the object it's following, and its rotation
            transform.position = position;
            cameraFollowObject.transform.position = position;
            rotation = transform.localEulerAngles.y;

            adjustedMovementVector = CalculateMovementAngle();

            CalculateCurrentSpeed();
        }
    }

    private void CalculateCurrentSpeed()
    {
        if (!isMovingBackward && !isMovingForward && !isMovingLeft && !isMovingRight)
        {
            currentSpeed -= deceleration;
            currentSpeed = Mathf.Max(currentSpeed, minSpeed);
        }
        else if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration;
        }
    }

    // Manual Movement
    private void ChangeMovementStatus_Left (){
        isMovingLeft = !isMovingLeft;
    }

    private void MoveLeft (){
        cameraFollowObject.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * currentSpeed * Time.deltaTime); // move on -X axis 
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Right (){
        isMovingRight = !isMovingRight;
    }

    private void MoveRight (){
        cameraFollowObject.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * currentSpeed * Time.deltaTime); // move on +X axis
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Forward (){
        isMovingForward = !isMovingForward;
    }

    private void MoveForward () {
        cameraFollowObject.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * currentSpeed * Time.deltaTime); // move on +Z axis
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Backward (){
        isMovingBackward = !isMovingBackward;
    }

    private void MoveBackward (){
        cameraFollowObject.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * currentSpeed * Time.deltaTime); // move on -Z axis
        position = cameraFollowObject.position;
    }

    // Rotation

    private void RotateCamera ()
    {
        Vector3 rotation = cameraRotationObject.transform.localEulerAngles;
        float currentMousePosition = Input.GetAxis("Mouse X");

        if (!centerCalculated){
            centerOfRotation = CalculateCenterOfRotation();
            centerCalculated = true;
        }
        
        rotation.y += currentMousePosition;
        cameraRotationObject.transform.localEulerAngles = rotation;
        position = CalculateCameraOffsetIndependent() + centerOfRotation;

        position.x = Mathf.Clamp(position.x, 0, GetCameraMoveLimit().x);
        position.z = Mathf.Clamp(position.z, -20, GetCameraMoveLimit().y);
        cameraFollowObject.position = Vector3.Lerp(cameraFollowObject.position, position, rotationSpeed);
        position = cameraFollowObject.position;
    }

    private void ChangeRotationStatus (){
        isRotating = !isRotating;
    }

    // Zooming in and out

    private void ZoomIn()
    {
        position += transform.forward * zoomSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, cameraVerticalMoveLimit.x, cameraVerticalMoveLimit.y);
    }

    private void ZoomOut()
    {
        position -= transform.forward * zoomSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, cameraVerticalMoveLimit.x, cameraVerticalMoveLimit.y);
    }


    public void CameraTeleportToWorldObject ()
    {
        cameraFollowObject.transform.position = (objectToMoveTowards.transform.position + CalculateCameraOffset());
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    public void UpdateMovementBorder (int gridSize)
    {
        cameraMoveLimit.x = (gridSize * 5);
        cameraMoveLimit.y = ((gridSize * 5) - 20);
    }

    #region Offset Calculation

    // Calculates the position based on the camera rotation, angle and the selected object position
    public Vector3 CalculateCameraOffset ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y - objectToMoveTowards.transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Camera offset independent of any object 
    public Vector3 CalculateCameraOffsetIndependent ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Vector at which the camera is supposed to move forwards
    public Vector3 CalculateMovementAngle ()
    {
        double y = Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(y), 0, Convert.ToSingle(x));
    }

    // Returns the position around which the camera is supposed to rotate
    public Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }

    #endregion

    // Getters
    public Vector2 GetCameraMoveLimit (){
        return cameraMoveLimit;
    }
}

