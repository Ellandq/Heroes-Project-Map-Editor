using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceInformationDisplay : MonoBehaviour
{
    public static ResourceInformationDisplay Instance;

    [SerializeField] public ResourcesObject resourcesObj;

    [Header ("UI References")]
    [SerializeField] private GameObject objectPositionDisplay;
    [SerializeField] private TMP_Dropdown resourcesObjTypeDisplay;
    [SerializeField] private TMP_InputField resourcesObjRotationDisplay;
    [SerializeField] private TMP_InputField resourcesObjCountDisplay;

    private void Awake ()
    {
        Instance = this;
    }

    // public void UpdateDisplay(ResourcesObj _resourcesObj)
    // {
    //     resourcesObj = _resourcesObj;
    //     this.gameObject.SetActive(true);
    //     UpdatePositionDisplay(resourcesObj.gridPosition);
    //     ObjectInformationDisplay.Instance.UpdateObjectTypeDisplay(resourcesObj.objectType);
    // }

    // private void UpdatePositionDisplay (Vector2Int gridPosition){
    //     objectPositionDisplay.transform.GetChild(0).GetComponent<TMP_InputField>().text = "X: " + Convert.ToString(gridPosition.x);
    //     objectPositionDisplay.transform.GetChild(1).GetComponent<TMP_InputField>().text = "Y: " + Convert.ToString(gridPosition.y);
    // }

    // public void ChangeResourcesType (int index)
    // {
    //     resourcesObj.ChangeResourceType((ResourceType)index);
    //     SetResourceDefaultValue();
    // }

    // public void ChangeResourceCount(string sr)
    // {
    //     resourcesObj.count = Convert.ToInt32(sr);
    // }

    // public void SetResourceDefaultValue ()
    // {
    //     switch (resourcesObj.resourceType){
    //         case ResourceType.Gold:
    //             resourcesObj.count = 500;
    //         break;

    //         case ResourceType.Wood:
    //             resourcesObj.count = 5;
    //         break;

    //         case ResourceType.Ore:
    //             resourcesObj.count = 5;
    //         break;

    //         default:
    //             resourcesObj.count = 3;
    //         break;
    //     }
    // }
}
