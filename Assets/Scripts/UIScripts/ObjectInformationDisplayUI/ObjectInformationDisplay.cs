using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInformationDisplay : MonoBehaviour
{
    public static ObjectInformationDisplay Instance;

    [Header ("UI References")]
    [SerializeField] private TMP_Dropdown objectTypeDisplay;
    [SerializeField] private GameObject objectPositionDisplay;

    [SerializeField] private GameObject cityDisplay; 
    [SerializeField] private GameObject armyDisplay; 
    [SerializeField] private GameObject mineDisplay; 
    [SerializeField] private GameObject buildingDisplay; 
    [SerializeField] private GameObject dwellingDisplay; 
    [SerializeField] private GameObject resourceDisplay; 

    [Header ("Displayed Object")]
    [SerializeField] private GameObject selectedObject;

    private void Awake ()
    {
        Instance = this;
    }

    // public void ActivateObjectDisplay()
    // {
    //     DisableObjectDisplay();
    //     switch (selectedObject.tag){
    //         case "City":
    //             cityDisplay.SetActive(true);
    //             CityInformationDisplay.Instance.UpdateDisplay(selectedObject.GetComponent<City>());
    //             objectTypeDisplay.value = 1;
    //         break;

    //         case "Army":
    //             armyDisplay.SetActive(true);
    //             ArmyInformationDisplay.Instance.UpdateDisplay(selectedObject.GetComponent<Army>());
    //             objectTypeDisplay.value = 2;
    //         break;

    //         case "Mine":
    //             mineDisplay.SetActive(true);
    //             MineInformationDisplay.Instance.UpdateDisplay(selectedObject.GetComponent<Mine>());
    //             objectTypeDisplay.value = 3;
    //         break;

    //         case "Building":

    //         break;

    //         case "Dwelling":

    //         break;

    //         case "Resource":
    //             resourceDisplay.SetActive(true);
    //             ResourceInformationDisplay.Instance.UpdateDisplay(selectedObject.GetComponent<ResourcesObj>());
    //             objectTypeDisplay.value = 6;
    //         break;
    //     }
    // }

    // private void DisableObjectDisplay ()
    // {
    //     cityDisplay.SetActive(false);
    //     armyDisplay.SetActive(false);
    //     mineDisplay.SetActive(false);
    //     // buildingDisplay.SetActive(false);
    //     // dwellingDisplay.SetActive(false);
    //     resourceDisplay.SetActive(false);
    //     UpdateObjectTypeDisplay();
    // }

    // public void ChangeSelectedObject (GameObject objectToSelect)
    // {
    //     selectedObject = objectToSelect;

    //     if (selectedObject != null){
    //         if (selectedObject.tag != "GridCell"){
    //             ActivateObjectDisplay();
    //         }else{
    //             DisableObjectDisplay();
    //         }
    //     }else{
    //         DisableObjectDisplay();
    //     }
    // }

    // public void UpdateObjectTypeDisplay (ObjectType _objectType)
    // {
    //     objectTypeDisplay.value = (int)_objectType + 1;
    // }

    // public void UpdateObjectTypeDisplay ()
    // {
    //     objectTypeDisplay.value = 0;
    // }
}
