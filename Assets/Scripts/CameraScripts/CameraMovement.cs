using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Transform cameraFollowObject;
    [SerializeField] private Transform cameraRotationObject;
    [SerializeField] private GameObject objectToMoveTowards;

    [Header("Camera Movement Options")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.2f;

    [Header("Camera Rotation Options")]
    [SerializeField] private float rotationSpeed;
    private bool isRotating;

    [Header("Camera Zoom Options")]
    [SerializeField] private float zoomSpeed;

    [Header("Camera Movement Limits")]
    [SerializeField] private Vector2 cameraMoveLimit;
    [SerializeField] private Vector2 cameraVerticalMoveLimit;

    private Vector3 position;
    private Vector3 rotation;
    private Vector3 adjustedMovementVector;
    private Vector3 centerOfRotation;
    private bool centerCalculated;
    private bool isMovingLeft;
    private bool isMovingRight;
    private bool isMovingForward;
    private bool isMovingBackward;

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
    }

    private void Start()
    {
        currentSpeed = minSpeed;
    }

    private void Update()
    {
        if (isRotating)
        {
            cameraManager.DisableCameraMovement();
            Cursor.lockState = CursorLockMode.Locked;
            RotateCamera();
        }
        else
        {
            centerCalculated = false;
            Cursor.lockState = CursorLockMode.None;
            cameraManager.EnableCameraMovement();
        }

        if (cameraManager.cameraEnabled && cameraManager.cameraMovementEnabled)
        {
            adjustedMovementVector = CalculateMovementAngle();

            if (isMovingLeft) MoveLeft();
            if (isMovingRight) MoveRight();
            if (isMovingForward) MoveForward();
            if (isMovingBackward) MoveBackward();

            CalculateCurrentSpeed();

            // Clamp the camera's position to the camera move limit
            ClampCameraPosition();

            // Update the camera's position, the object it's following, and its rotation
            transform.position = position;
            cameraFollowObject.transform.position = position;
            rotation = transform.localEulerAngles;
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

    #region Movement

    private void ChangeMovementStatus_Left()
    {
        isMovingLeft = !isMovingLeft;
    }

    private void MoveLeft()
    {
        cameraFollowObject.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * currentSpeed * Time.deltaTime);
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Right()
    {
        isMovingRight = !isMovingRight;
    }

    private void MoveRight()
    {
        cameraFollowObject.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * currentSpeed * Time.deltaTime);
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Forward()
    {
        isMovingForward = !isMovingForward;
    }

    private void MoveForward()
    {
        cameraFollowObject.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * currentSpeed * Time.deltaTime);
        position = cameraFollowObject.position;
    }

    private void ChangeMovementStatus_Backward()
    {
        isMovingBackward = !isMovingBackward;
    }

    private void MoveBackward()
    {
        cameraFollowObject.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * currentSpeed * Time.deltaTime);
        position = cameraFollowObject.position;
    }

    #endregion

    private void RotateCamera()
    {
        if (!centerCalculated)
        {
            centerOfRotation = CalculateCenterOfRotation();
            centerCalculated = true;
        }

        Quaternion targetRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + Input.GetAxis("Mouse X"), transform.localEulerAngles.z);
        position = CalculateCameraOffsetIndependent() + centerOfRotation;

        ClampCameraPosition();

        cameraFollowObject.position = Vector3.Lerp(cameraFollowObject.position, position, rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);

        transform.position = cameraFollowObject.position;

        position = transform.position;
        rotation = transform.localEulerAngles;
    }


    private void ChangeRotationStatus()
    {
        isRotating = !isRotating;
    }

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

    public void CameraTeleportToWorldObject()
    {
        cameraFollowObject.transform.position = objectToMoveTowards.transform.position + CalculateCameraOffset();
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    public void UpdateMovementBorder(int gridSize)
    {
        cameraMoveLimit.x = gridSize * 5;
        cameraMoveLimit.y = gridSize * 5 - 20;
    }

    private void ClampCameraPosition()
    {
        position.x = Mathf.Clamp(position.x, 0f, cameraMoveLimit.x);
        position.z = Mathf.Clamp(position.z, -20f, cameraMoveLimit.y);
        position.y = Mathf.Clamp(position.y, cameraVerticalMoveLimit.x, cameraVerticalMoveLimit.y);
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

    private Vector3 CalculateMovementAngle()
    {
        float angle = transform.localEulerAngles.y * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);
        return new Vector3(x, 0, z);
    }

    private Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }

    #endregion

    private Vector2 GetCameraMoveLimit()
    {
        return new Vector2(cameraMoveLimit.x, cameraMoveLimit.y);
    }
}
