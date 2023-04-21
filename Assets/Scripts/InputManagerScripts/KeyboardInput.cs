using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] InputManager inputManager;

    [Header ("Events")]
    public Action onForwardChangedState;
    public Action onBackwardChangedState;
    public Action onLeftChangedState;
    public Action onRightChangedState;
    public Action onResetButtonPressed;
    public Action onLeftShiftChangedState;

    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.A)) onLeftChangedState?.Invoke();
        if (Input.GetKeyUp(KeyCode.A)) onLeftChangedState?.Invoke();

        if(Input.GetKeyDown(KeyCode.D)) onRightChangedState?.Invoke();
        if (Input.GetKeyUp(KeyCode.D)) onRightChangedState?.Invoke();

        if(Input.GetKeyDown(KeyCode.W)) onForwardChangedState?.Invoke();
        if (Input.GetKeyUp(KeyCode.W)) onForwardChangedState?.Invoke();

        if(Input.GetKeyDown(KeyCode.S)) onBackwardChangedState?.Invoke();
        if (Input.GetKeyUp(KeyCode.S)) onBackwardChangedState?.Invoke();

        if(Input.GetKeyDown(KeyCode.R)) onResetButtonPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftShift)) onLeftShiftChangedState?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftShift)) onLeftShiftChangedState?.Invoke();
    }
}