using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] InputManager inputManager;

    [Header ("Events")]
    public Action onUpPressed;
    public Action onDownPressed;
    public Action onLeftPressed;
    public Action onRightPressed;
    public Action onResetPressed;
    public Action onLeftShiftChangedState;

    [Header ("Button states")]
    private bool isLeftShiftPressed;

    // internal bool isUpPressed;
    // internal bool isDownPressed;
    // internal bool isLeftPressed;
    // internal bool isRightPressed;
    // internal bool resetCameraPressed;
    //internal bool isTabKeyPressed;
    //internal bool isSpaceKeyPressed

    private void Update ()
    {
        if(Input.GetKey(KeyCode.A)){
            onLeftPressed?.Invoke();
        }

        if(Input.GetKey(KeyCode.D)){
            onRightPressed?.Invoke();
        }

        if(Input.GetKey(KeyCode.W)){
            onUpPressed?.Invoke();
        }

        if(Input.GetKey(KeyCode.S)){
            onDownPressed?.Invoke();
        }

        if(Input.GetKey(KeyCode.R)){
            onResetPressed?.Invoke();
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            if (!isLeftShiftPressed){
                onLeftShiftChangedState?.Invoke();
                isLeftShiftPressed = true;
            }
        }else if (isLeftShiftPressed){
            isLeftShiftPressed = false;
            onLeftShiftChangedState?.Invoke();
        }
    }
}