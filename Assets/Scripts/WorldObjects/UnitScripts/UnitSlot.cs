using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Linq;

public class UnitSlot : MonoBehaviour
{
    [Header ("Slot Information")]
    public bool slotEmpty = true;
    public bool isSlotHero;
    public int unitID;
    public float movementPoints;

    [Header ("Unit Information")]  
    public UnitObject unitObject;
    public UnitName unitName;
    public int unitCount;


    [Header ("Hero Information")]
    public Hero heroObject;
    public HeroTag heroTag;
    

    public void ChangeSlotStatus(int _unitId, int _unitCount, float _movementPoints)
    { 
        if (_unitId > Enum.GetValues(typeof(UnitName)).Cast<int>().Max())
        {
            isSlotHero = true;
            slotEmpty = false;
            unitID = _unitId;
            unitCount = 1;
            movementPoints = _movementPoints;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = (HeroTag)(unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max());
            heroObject = HeroesManager.Instance.GetHeroObject(heroTag);
        }
        else if (_unitCount == 0 | _unitId == 0)
        {
            isSlotHero = false;
            slotEmpty = true;
            unitID = 0;
            unitCount = 0;
            movementPoints = 0f;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = HeroTag.Empty;
            heroObject = null;

            return;
        }else{
            isSlotHero = false;
            slotEmpty = false;
            unitID = _unitId;
            unitCount = _unitCount;
            movementPoints = _movementPoints;

            unitName = (UnitName)unitID;
            unitObject = UnitManager.Instance.GetUnitObject(unitID);

            heroTag = HeroTag.Empty;
            heroObject = null;
        }
        
    }

    public void SetSlotStatus(int _unitId, int _unitCount)
    {       
        if (_unitCount == 0){
            return;
        }
        if (_unitId > Enum.GetValues(typeof(UnitName)).Cast<int>().Max())
        {
            isSlotHero = true;
            slotEmpty = false;
            unitID = _unitId;
            _unitCount = 1;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = (HeroTag)(unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max());
            heroObject = HeroesManager.Instance.GetHeroObject(heroTag);

            movementPoints = heroObject.mapMovement * 100; 
        }else{
            slotEmpty = false;
            unitID = _unitId; 
            unitCount = _unitCount;

            unitName = (UnitName)unitID;
            unitObject = UnitManager.Instance.GetUnitObject(unitID);

            heroTag = HeroTag.Empty;
            heroObject = null;
            
            movementPoints = unitObject.mapMovement * 100; 
        }
    }

    public void RemoveUnits()
    {
        slotEmpty = true;
        isSlotHero = false;

        unitName = UnitName.Empty;
        unitObject = null;

        heroTag = HeroTag.Empty;
        heroObject = null;

        unitID = 0;
        unitCount = 0;
        movementPoints = 0f;
    }

    public void AddUnits (int _unitCount)
    {
        unitCount += _unitCount;
    }

    public void RestoreMovementPoints ()
    {
        if (unitObject != null){
            movementPoints = unitObject.mapMovement * 100;
        }else if (heroObject != null){
            movementPoints = heroObject.mapMovement * 100;
        }
    }

    public void RemoveMovementPoints (int movmentPointsToRemove)
    {
        if (!slotEmpty){
            movementPoints -= movmentPointsToRemove;
        }
    }

    public float GetMovementPoints ()
    {
        return movementPoints;
    }

    public int GetSlotUnitID (){
        return unitID;
    }

    public int GetSlotUnitCount (){
        return unitCount;
    }
}
