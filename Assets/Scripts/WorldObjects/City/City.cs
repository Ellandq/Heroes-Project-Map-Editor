using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : WorldObject
{
    [Header("Main city information")]
    public CityFraction cityFraction;

    [Header("Garrison refrences")]
    [SerializeField] private List<UnitSlot> unitSlots;

    [Header ("City Buildings")]
    [SerializeField] public List<CityBuildingStatus> cityBuildings;

    public void Initialize (Vector2Int gridPosition, Vector3 rotation, CityFraction cityFraction, PlayerTag ownedByPlayer = PlayerTag.None, ObjectType objectType = ObjectType.City)
    {
        base.Initialize(gridPosition, rotation, objectType, ownedByPlayer);
        ChangeOwningPlayer(ownedByPlayer);
        ChangeCityFraction(cityFraction);

        // Unit Slots initialization
        unitSlots = new List<UnitSlot>(7);
        for (int i = 0; i < unitSlots.Count; i++) unitSlots[i].SetSlotStatus(0, 0);

        // City buildings initialization
        for (int i = 0; i < 30; i++) cityBuildings.Add(0);
        cityBuildings[0] = CityBuildingStatus.Built;
        cityBuildings[4] = CityBuildingStatus.Built;

        GameGrid.Instance.SetGridCellStatus(this.gameObject, ObjectShapeType.City, gridPosition);
    }

    public override void ChangeOwningPlayer (PlayerTag ownedByPlayer = PlayerTag.None){ 
        PlayerManager.Instance.GetPlayer(base.GetPlayerTag()).RemoveCity(this);
        base.ChangeOwningPlayer(ownedByPlayer);
        PlayerManager.Instance.GetPlayer(ownedByPlayer).AddCity(this);
        if (ownedByPlayer != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColor(ownedByPlayer);  // Get color from player
        }else{
            flag.SetActive(false);
        }
    }

    public void SetUnitSlotStatus (int unitID, int unitCount, int unitSlotIndex){
        unitSlots[unitSlotIndex].SetSlotStatus(unitID, unitCount);
    }

    public void ChangeBuildingStatus (BuildingID id, CityBuildingStatus status){
        cityBuildings[(int)id] = status;
    }

    public void ChangeCityFraction (CityFraction _fraction){
        cityFraction = _fraction;
    }

    public override List<int> GetConvertedObjectInformation ()
    {
        List<int> cityInfo = new List<int>();

        cityInfo.Add((int)GetPlayerTag());
        cityInfo.Add(GetGridPosition().x);
        cityInfo.Add(GetGridPosition().y);
        cityInfo.Add(Convert.ToInt32(GetRotation()));
        cityInfo.Add((int)cityFraction);
        
        for (int i = 0; i < cityBuildings.Count; i++){
            cityInfo.Add((int)cityBuildings[i]);
        }

        for (int i = 0; i < 7; i++){
            cityInfo.Add(unitSlots[i].GetSlotUnitID());
            cityInfo.Add(unitSlots[i].GetSlotUnitCount());
        }

        return cityInfo;
    }

    public CityFraction GetCityFraction (){
        return cityFraction;
    }
}
