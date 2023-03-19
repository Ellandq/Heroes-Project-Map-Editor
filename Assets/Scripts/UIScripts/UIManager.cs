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
    [SerializeField] private GameObject bottomAreaUI;
    [SerializeField] private GameObject middleAreaUI;


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

    public void EnableBottomUI ()
    {
        bottomAreaUI.SetActive(true);
    }

    public void DisableBottomUI ()
    {
        bottomAreaUI.SetActive(false);
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

    #region Top UI 

    

    #endregion
}
