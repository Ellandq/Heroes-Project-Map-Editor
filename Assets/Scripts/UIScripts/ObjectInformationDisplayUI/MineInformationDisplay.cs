using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MineInformationDisplay : MonoBehaviour
{
    public static MineInformationDisplay Instance;

    [SerializeField] public Mine mine;

    [Header ("UI References")]
    [SerializeField] private GameObject objectPositionDisplay;
    [SerializeField] private TMP_Dropdown playerOwnershipOptions;
    [SerializeField] private TMP_Dropdown mineTypeDisplay;
    [SerializeField] private TMP_InputField MineRotationDisplay;
    [SerializeField] private ObjectUnitsDisplay unitDisplay;

    private void Awake ()
    {
        Instance = this;
    }

    // private void Start ()
    // {
    //     PlayerManager.Instance.onPlayerCountChange.AddListener(UpdatePlayerOwnerOptions);
    // }

    // private void OnEnable()
    // {
    //     UpdatePlayerOwnerOptions();
    // }

    // public void UpdateDisplay(Mine _mine)
    // {
    //     mine = _mine;
    //     this.gameObject.SetActive(true);
    //     UpdatePositionDisplay(mine.gridPosition);
    //     ObjectInformationDisplay.Instance.UpdateObjectTypeDisplay(mine.objectType);
    //     UpdateRotationDisplay();
    //     UpdatePlayerOwnerOptions();
    //     unitDisplay.UpdateUnitDisplay(mine.garrisonSlotsID, mine.garrisonSlotsCount);
    //     mineTypeDisplay.value = (int)mine.mineType;
    // }

    // private void UpdatePositionDisplay (Vector2Int gridPosition){
    //     objectPositionDisplay.transform.GetChild(0).GetComponent<TMP_InputField>().text = "X: " + Convert.ToString(gridPosition.x);
    //     objectPositionDisplay.transform.GetChild(1).GetComponent<TMP_InputField>().text = "Y: " + Convert.ToString(gridPosition.y);
    // }

    // private void UpdatePlayerOwnerOptions ()
    // {
    //     playerOwnershipOptions.ClearOptions();
    //     playerOwnershipOptions.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), PlayerTag.None)));
    //     foreach (PlayerTag _player in PlayerManager.Instance.existingPlayers){
    //         playerOwnershipOptions.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), _player)));
    //     }
    //     try{
    //         playerOwnershipOptions.value = PlayerManager.Instance.GetCurrentPlayerIndex(mine.playerTag);
    //     }catch (NullReferenceException){}
    // }

    // public void ChangeMineOwner (int index)
    // {
    //     if (mine != null){
    //         if (index != 0){
    //             mine.AddOwningPlayer(PlayerManager.Instance.players[index - 1].gameObject);
    //         }else{
    //             mine.AddOwningPlayer(PlayerManager.Instance.neutralPlayer);
    //         }
    //     }
    // }

    // public void ChangeMineRotation (string _rotationChange)
    // {
    //     int rotationChange = Convert.ToInt32(_rotationChange);
    //     if (mine != null){
    //         if (rotationChange == 0 || rotationChange == 90 || rotationChange == 180 || rotationChange == 270 || rotationChange == 360){
    //         mine.ChangeMineRotation(rotationChange);
    //         UpdateRotationDisplay();
    //         }else{
    //             UpdateRotationDisplay();
    //         }
    //     } 
    // }

    // public void ChangeMineRotation ()
    // {
    //     int rotationChange = Convert.ToInt32(mine.rotation.y);
    //     rotationChange += 90;
    //     if (mine != null){
    //         if (rotationChange > 270){
    //             mine.ChangeMineRotation(0);
    //             UpdateRotationDisplay();
    //         }else{
    //             mine.ChangeMineRotation(rotationChange);
    //             UpdateRotationDisplay();
    //         }
    //     } 
    // }

    // private void UpdateRotationDisplay ()
    // {
    //     if (mine != null){
    //         MineRotationDisplay.text = Convert.ToString(mine.GetMineRotation());
    //     }
    // }

    // public void UpdateMineUnits ()
    // {
    //     for (int i = 0; i < 7; i ++){
    //         try{
    //             mine.garrisonSlotsID[i] = (int)Enum.Parse(typeof(UnitName), unitDisplay.unitIdDisplay[i].text);
    //             mine.garrisonSlotsCount[i] = Convert.ToInt32(unitDisplay.unitCountDisplay[i].text);
    //         }catch (ArgumentException){
    //             mine.garrisonSlotsID[i] = Enum.GetValues(typeof(UnitName)).Cast<int>().Max() + (int)Enum.Parse(typeof(HeroTag), unitDisplay.unitIdDisplay[i].text);
    //             mine.garrisonSlotsCount[i] = 1;
    //         }
    //     }
    //     unitDisplay.UpdateUnitDisplay(mine.garrisonSlotsID, mine.garrisonSlotsCount);
    // }

    // public void ChangeMineType (int index)
    // {
    //     if (index != (int)mine.mineType){
    //         mine.ChangeMineType((ResourceType)index);
    //         mineTypeDisplay.value = index;
    //         Debug.Log(mine.gameObject.name);
    //     }
    // }
}
