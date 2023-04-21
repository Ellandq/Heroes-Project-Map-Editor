using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private LayerMask layersToHit;

    [Header ("Events")]
    public Action onLeftMouseButton_Down;
    public Action onLeftMouseButton_Up;
    public Action onRightMouseButton_Down;
    public Action onRightMouseButton_Up;
    public Action onMiddleMouseButton_Down;
    public Action onMiddleMouseButton_Up;
    public Action onScrollWheelMove_Up;
    public Action onScrollWheelMove_Down;

    private void Update ()
    {
        GetMouseInput();
    }

    // Checks what mouse buttons are pressed
    private void GetMouseInput ()
    {
        if (Input.GetMouseButtonDown(0)) onLeftMouseButton_Down?.Invoke();
        else if (Input.GetMouseButtonUp(0)) onLeftMouseButton_Up?.Invoke();
        if (Input.GetMouseButtonDown(1)) onRightMouseButton_Down?.Invoke();
        else if (Input.GetMouseButtonUp(1)) onRightMouseButton_Up?.Invoke();
        if (Input.GetMouseButtonDown(2)) onMiddleMouseButton_Down?.Invoke();
        else if (Input.GetMouseButtonUp(2)) onMiddleMouseButton_Up?.Invoke();

        // Check for scroll wheel input
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta > 0f) onScrollWheelMove_Up?.Invoke();
        else if (scrollDelta < 0f) onScrollWheelMove_Down?.Invoke();
    }

    // Returns the mouse world position
    public Vector3 MouseWorldPosition(LayerMask layersToHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit))
        {
            return hitData.point;
        }
        else
        {
            return new Vector3(0, 10, 0);
        }
    }

    // Returns the object that the mouse is over
    public GameObject MouseOverWorldObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitData, 100, LayerMask.GetMask("WorldObject")))
        {
            Collider gridCellCollider = hitData.collider;
            return gridCellCollider.gameObject;
        }
        return null;
    }

    // Returns true if the mouse is over UI
    public bool IsMouseOverUI ()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
