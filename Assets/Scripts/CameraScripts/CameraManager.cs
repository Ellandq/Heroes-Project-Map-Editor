using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// The main camera script
//==============================================

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    // Store a referance to all sub camera scripts
    [Header ("Camera script references")]
    [SerializeField] internal CameraMovement cameraMovement;

    [Header ("Camera status")]
    [SerializeField] internal bool cameraEnabled;
    [SerializeField] internal bool cameraMovementEnabled;

    private void Awake (){
        Instance = this;
    }

    public void EnableCamera (){
        cameraEnabled = true;
    }

    public void DisableCamera (){
        cameraEnabled = false;
    }

    public void EnableCameraMovement (){
        cameraMovementEnabled = true;
    }

    public void DisableCameraMovement (){
        cameraMovementEnabled = false;
    }
}
