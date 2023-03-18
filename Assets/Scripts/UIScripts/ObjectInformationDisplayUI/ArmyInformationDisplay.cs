using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmyInformationDisplay : MonoBehaviour
{
    public static ArmyInformationDisplay Instance;

    [SerializeField] public Army army;

    [Header ("UI References")]
    [SerializeField] private GameObject objectPositionDisplay;
    [SerializeField] private TMP_Dropdown playerOwnershipOptions;
    [SerializeField] private TMP_InputField armyRotationDisplay;
    [SerializeField] private ObjectUnitsDisplay unitDisplay;

    private void Awake ()
    {
        Instance = this;
    }

    private void Start ()
    {
        PlayerManager.Instance.onPlayerCountChange.AddListener(UpdatePlayerOwnerOptions);
    }

    public void UpdateDisplay(Army _army)
    {
        army = _army;
        this.gameObject.SetActive(true);
        UpdatePositionDisplay(army.gridPosition);
        ObjectInformationDisplay.Instance.UpdateObjectTypeDisplay(army.objectType);
        UpdateRotationDisplay();
        UpdatePlayerOwnerOptions();
        unitDisplay.UpdateUnitDisplay(army.unitSlotsID, army.unitSlotsCount);
    }

    private void UpdatePositionDisplay (Vector2Int gridPosition){
        objectPositionDisplay.transform.GetChild(0).GetComponent<TMP_InputField>().text = "X: " + Convert.ToString(gridPosition.x);
        objectPositionDisplay.transform.GetChild(1).GetComponent<TMP_InputField>().text = "Y: " + Convert.ToString(gridPosition.y);
    }

    private void UpdatePlayerOwnerOptions ()
    {
        playerOwnershipOptions.ClearOptions();
        playerOwnershipOptions.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), PlayerTag.None)));
        foreach (PlayerTag _player in PlayerManager.Instance.existingPlayers){
            playerOwnershipOptions.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), _player)));
        }
        
        PlayerManager.Instance.GetCurrentPlayerIndex(army.playerTag);
    }

    public void ChangeArmyOwner (int index)
    {
        if (army != null){
            if (index != 0){
                army.AddOwningPlayer(PlayerManager.Instance.players[index - 1].gameObject);
            }else{
                army.AddOwningPlayer(PlayerManager.Instance.neutralPlayer);
            }
        }
    }

    public void ChangeArmyRotation (string _rotationChange)
    {
        int rotationChange = Convert.ToInt32(_rotationChange);
        if (army != null){
            if (rotationChange == 0 || rotationChange == 90 || rotationChange == 180 || rotationChange == 270 || rotationChange == 360){
            army.ChangeArmyRotation(rotationChange);
            UpdateRotationDisplay();
            }else{
                UpdateRotationDisplay();
            }
        } 
    }

    public void ChangeArmyRotation ()
    {
        int rotationChange = Convert.ToInt32(army.rotation.y);
        rotationChange += 90;
        if (army != null){
            if (rotationChange > 270){
                army.ChangeArmyRotation(0);
                UpdateRotationDisplay();
            }else{
                army.ChangeArmyRotation(rotationChange);
                UpdateRotationDisplay();
            }
        } 
    }

    private void UpdateRotationDisplay ()
    {
        if (army != null){
            armyRotationDisplay.text = Convert.ToString(army.GetArmyRotation());
        }
    }

    public void UpdateArmyUnits ()
    {
        for (int i = 0; i < 7; i ++){
            try{
                army.unitSlotsID[i] = (int)Enum.Parse(typeof(UnitName), unitDisplay.unitIdDisplay[i].text);
                army.unitSlotsCount[i] = Convert.ToInt32(unitDisplay.unitCountDisplay[i].text);
            }catch (ArgumentException){
                army.unitSlotsID[i] = Enum.GetValues(typeof(UnitName)).Cast<int>().Max() + (int)Enum.Parse(typeof(HeroTag), unitDisplay.unitIdDisplay[i].text);
                army.unitSlotsCount[i] = 1;
            }
        }
        unitDisplay.UpdateUnitDisplay(army.unitSlotsID, army.unitSlotsCount);
    }
}
