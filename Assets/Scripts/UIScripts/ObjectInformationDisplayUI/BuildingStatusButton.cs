using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingStatusButton : MonoBehaviour
{
    [SerializeField] private GameObject connectedButton;

    [Header ("Button Status Sprites")]
    [SerializeField] private Sprite availableBuildingSprite;
    [SerializeField] private Sprite createdBuildingSprite;
    [SerializeField] private Sprite blockedBuildingSprite;

    [Header ("Other building information")]
    public short buttonStatus;
    [SerializeField] private BuildingID buildingID;

    public void ChangeButtonStatus ()
    {
        if (buttonStatus == 0) {
            buttonStatus = 1;
            connectedButton.GetComponent<Image>().sprite = createdBuildingSprite;
        }else if (buttonStatus == 1){
            buttonStatus = 2;
            connectedButton.GetComponent<Image>().sprite = blockedBuildingSprite;
        }else{
            buttonStatus = 0;
            connectedButton.GetComponent<Image>().sprite = availableBuildingSprite;
        }
        CityInformationDisplay.Instance.ChangeBuildingStatus(buildingID, buttonStatus);
    }

    public void ChangeButtonStatus (short _buttonStatus)
    {
        buttonStatus = _buttonStatus;
        if (buttonStatus == 1) {
            connectedButton.GetComponent<Image>().sprite = createdBuildingSprite;
        }else if (buttonStatus == 2){
            connectedButton.GetComponent<Image>().sprite = blockedBuildingSprite;
        }else{
            connectedButton.GetComponent<Image>().sprite = availableBuildingSprite;
        }
        CityInformationDisplay.Instance.ChangeBuildingStatus(buildingID, buttonStatus);
    }

}
