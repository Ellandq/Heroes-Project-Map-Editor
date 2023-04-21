using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header ("UI Areas")]
    [SerializeField] private GameObject topAreaUI;
    [SerializeField] private GameObject leftAreaUI;
    [SerializeField] private GameObject rightAreaUI;
    [SerializeField] private GameObject middleAreaUI;
    [SerializeField] private GameObject popupUI;

    [Header ("UI Options")]
    [SerializeField] private MapSetup mapSetup;
    [SerializeField] private BrushDisplay brushDisplay;

    private void Awake ()
    {
        Instance = this;
        DisableLeftUI();
        DisablePopupUI();
        DisableRightUI();
        DisableTopUI();
        EnableMiddleUI();
        StartMapSetup();
    }

    #region UI Regions

    public void EnableUI (){
        EnableLeftUI();
        EnablePopupUI();
        EnableRightUI();
        EnableTopUI();
        EnableMiddleUI();
    }

    public void EnableTopUI (){
        topAreaUI.SetActive(true);
    }

    public void DisableTopUI (){
        topAreaUI.SetActive(false);
    }

    public void EnablePopupUI (){
        popupUI.SetActive(true);
    }

    public void DisablePopupUI (){
        popupUI.SetActive(false);
    }

    public void EnableLeftUI (){
        leftAreaUI.SetActive(true);
    }

    public void DisableLeftUI (){
        leftAreaUI.SetActive(false);
    }

    public void EnableRightUI (){
        rightAreaUI.SetActive(true);
    }

    public void DisableRightUI (){
        rightAreaUI.SetActive(false);
    }

    public void EnableMiddleUI (){
        middleAreaUI.SetActive(true);
    }

    public void DisableMiddleUI (){
        middleAreaUI.SetActive(false);
    }

    #endregion

    #region TopUI 

    

    #endregion

    #region BottomUI

    #endregion

    #region LeftUI

    public void ChangeTool(int value){
        brushDisplay.ActivateTool((BrushMode)value);
    }

    #endregion

    #region RightUI

    #endregion

    #region MiddleUI

    public void StartMapSetup (){
        mapSetup.EnableDisplay();
    }

    #endregion

    #region PopupUI

    #endregion
}