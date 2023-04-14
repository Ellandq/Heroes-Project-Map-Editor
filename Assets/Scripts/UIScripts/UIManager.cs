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
    [SerializeField] private BrushDisplay brushDisplay;
    [SerializeField] private GameObject rightAreaUI;
    [SerializeField] private GameObject middleAreaUI;
    [SerializeField] private GameObject popupUI;

    [Header ("UI Options")]
    [SerializeField] private BrushMode currentBrushMode;

    private void Awake ()
    {
        Instance = this;
    }

    #region UI Regions

    public void EnableTopUI ()
    {
        topAreaUI.SetActive(true);
    }

    public void DisableTopUI ()
    {
        topAreaUI.SetActive(false);
    }

    public void EnablePopupUI ()
    {
        popupUI.SetActive(true);
    }

    public void DisablePopupUI ()
    {
        popupUI.SetActive(false);
    }

    public void EnableLeftUI ()
    {
        leftAreaUI.SetActive(true);
    }

    public void DisableLeftUI ()
    {
        leftAreaUI.SetActive(false);
    }

    public void EnableRightUI ()
    {
        rightAreaUI.SetActive(true);
    }

    public void DisableRightUI ()
    {
        rightAreaUI.SetActive(false);
    }

    #endregion

    #region TopUI 

    

    #endregion

    #region LeftUI

    public void ChangeTool(int value){
        brushDisplay.ActivateTool((BrushMode)value);
    }

    #endregion
}