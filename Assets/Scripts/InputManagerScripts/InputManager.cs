using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// The main input script
//==============================================

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    // Store a referance to all sub input scripts

    [SerializeField]
    internal WorldObjectInteractionManager worldObjectInteractionManager;

    [SerializeField]
    internal MouseInput mouseInput;

    [SerializeField]
    internal KeyboardInput keyboardInput;

    private void Awake ()
    {
        Instance = this;
    }

}
