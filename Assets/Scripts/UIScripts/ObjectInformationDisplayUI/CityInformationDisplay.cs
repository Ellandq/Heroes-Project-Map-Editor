using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityInformationDisplay : MonoBehaviour
{
    public static CityInformationDisplay Instance;

    [SerializeField] public City city;
    private Vector3 position;

    [Header ("UI References")]
    [SerializeField] private GameObject objectPositionDisplay;
    [SerializeField] private TMP_Dropdown playerOwnershipOptions;
    [SerializeField] private TMP_Dropdown cityFractionDisplay;
    [SerializeField] private TMP_InputField cityRotationDisplay;
    [SerializeField] private List<GameObject> buildingButtons;
    [SerializeField] private ObjectUnitsDisplay unitDisplay;
    [SerializeField] private Transform scrollableContent;

    private void Awake ()
    {
        Instance = this;
    }

    private void Update ()
    {
        position = scrollableContent.localPosition;

        if (position.y > 135f){
            position.y = 135f;
        }else if (position.y < 0){
            position.y = 0;
        }
        scrollableContent.localPosition = position;
    }

    private void Start ()
    {
        PlayerManager.Instance.onPlayerCountChange.AddListener(UpdatePlayerOwnerOptions);
        GetBuildingButtons();
    }

    private void OnEnable()
    {
        UpdatePlayerOwnerOptions();
    }

    public void UpdateDisplay(City _city)
    {
        city = _city;
        this.gameObject.SetActive(true);
        UpdatePositionDisplay(city.gridPosition);
        ObjectInformationDisplay.Instance.UpdateObjectTypeDisplay(city.objectType);
        UpdateRotationDisplay();
        UpdateBuildingDisplay();
        UpdatePlayerOwnerOptions();
        unitDisplay.UpdateUnitDisplay(city.garrisonSlotsID, city.garrisonSlotsCount);
        cityFractionDisplay.value = (int)city.cityFraction;
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
        try{
            playerOwnershipOptions.value = PlayerManager.Instance.GetCurrentPlayerIndex(city.playerTag);
        }catch (NullReferenceException){}
        
    }

    public void ChangeCityOwner (int index)
    {
        if (city != null){
            if (index != 0){
                city.AddOwningPlayer(PlayerManager.Instance.players[index - 1].gameObject);
            }else{
                city.AddOwningPlayer(PlayerManager.Instance.neutralPlayer);
            }
        }
    }

    public void ChangeCityRotation (string _rotationChange)
    {
        int rotationChange = Convert.ToInt32(_rotationChange);
        if (city != null){
            if (rotationChange == 0 || rotationChange == 90 || rotationChange == 180 || rotationChange == 270 || rotationChange == 360){
            city.ChangeCityRotation(rotationChange);
            UpdateRotationDisplay();
            }else{
                UpdateRotationDisplay();
            }
        } 
    }

    public void ChangeCityRotation ()
    {
        int rotationChange = Convert.ToInt32(city.rotation.y);
        rotationChange += 90;
        if (city != null){
            if (rotationChange > 270){
                city.ChangeCityRotation(0);
                UpdateRotationDisplay();
            }else{
                city.ChangeCityRotation(rotationChange);
                UpdateRotationDisplay();
            }
        } 
    }

    private void UpdateRotationDisplay ()
    {
        if (city != null){
            cityRotationDisplay.text = Convert.ToString(city.GetCityRotation());
        }
    }

    private void GetBuildingButtons ()
    {
        Transform trf = transform.GetChild(0);
        for (int i = 1; i < 31; i++){
            buildingButtons.Add(trf.GetChild(0).GetChild(i).gameObject);
        }
        UpdateBuildingDisplay();
    }

    public void ChangeBuildingStatus (BuildingID id, short status)
    {
        city.ChangeBuildingStatus(id, status);
    }

    public void UpdateBuildingDisplay()
    {
        if (city != null){
            for (int i = 0; i < buildingButtons.Count; i++){
                buildingButtons[i].GetComponent<BuildingStatusButton>().ChangeButtonStatus(city.cityBuildings[i]);
            }
        }
    }

    public void UpdateCityUnits ()
    {
        
        for (int i = 0; i < 7; i ++){
            try{
                city.garrisonSlotsID[i] = (int)Enum.Parse(typeof(UnitName), unitDisplay.unitIdDisplay[i].text);
                city.garrisonSlotsCount[i] = Convert.ToInt32(unitDisplay.unitCountDisplay[i].text);
            }catch (ArgumentException){
                city.garrisonSlotsID[i] = Enum.GetValues(typeof(UnitName)).Cast<int>().Max() + (int)Enum.Parse(typeof(HeroTag), unitDisplay.unitIdDisplay[i].text);
                city.garrisonSlotsCount[i] = 1;
            }
        }
        unitDisplay.UpdateUnitDisplay(city.garrisonSlotsID, city.garrisonSlotsCount);
    }

    public void ChangeCityFraction (int fractionIndex)
    {
        if (fractionIndex != (int)city.cityFraction){
            city.ChangeCityFraction((CityFraction)fractionIndex);
            cityFractionDisplay.value = fractionIndex;
        }
        
    }
}

public enum BuildingID{
    // Basic buildings
    VillageHall, TownHall, CityHall, Tavern, Prison, Fort, Citadel, Castle, Caravan, Shipyard,
    // Bonus buildings
    BonusBuilding_1, BonusBuilding_2, EquipementBuilding, RacialBuilding, GraalBuilding,
    // Magic buildings
    MagicHall_1, MagicHall_2, MagicHall_3, MagicHall_4, MagicHall_5, AdditionalMagic_1, AdditionalMagic_2,
    // Unit Buildings
    Tier1_1, Tier1_2, Tier2_1, Tier2_2, Tier3_1, Tier3_2, Tier4_1, Tier4_2,
    // Conditional ID's
    Tier_1, Tier_2, Tier_3, Tier_4
}
