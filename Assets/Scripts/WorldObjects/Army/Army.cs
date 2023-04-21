using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Army : WorldObject
{
    [Header("Unit slots Information")]
    [SerializeField] private List<UnitSlot> unitSlots;

    // Initialization 
    public Army(Vector2Int gridPosition, Vector3 rotation, PlayerTag ownedByPlayer = PlayerTag.None, ObjectType objectType = ObjectType.Army)
        : base(gridPosition, rotation, objectType, ownedByPlayer)
    {
        ChangeOwningPlayer(ownedByPlayer);
        
        unitSlots = new List<UnitSlot>(7);
        for (int i = 0; i < unitSlots.Count; i++) unitSlots[i].SetSlotStatus(0, 0);

    }

    // Other functions

    public override void ChangeOwningPlayer (PlayerTag ownedByPlayer = PlayerTag.None){ 
        PlayerManager.Instance.GetPlayer(ownedByPlayer).RemoveArmy(this);
        base.ChangeOwningPlayer(ownedByPlayer);
        PlayerManager.Instance.GetPlayer(ownedByPlayer).AddArmy(this);
        if (ownedByPlayer != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColor(ownedByPlayer);  // Get color from player
        }else{
            flag.SetActive(false);
        }
    }

    // Getters

    public override List<UnitSlot> GetUnitSlots (){
        return unitSlots;
    }

    public override List<int> GetConvertedObjectInformation ()
    {
        List<int> armyInfo = new List<int>();

        armyInfo.Add((int)GetPlayerTag());
        armyInfo.Add(GetGridPosition().x);
        armyInfo.Add(GetGridPosition().y);
        armyInfo.Add(Convert.ToInt32(GetRotation()));

        for (int i = 0; i < 7; i++){
            armyInfo.Add(unitSlots[i].GetSlotUnitID());
            armyInfo.Add(unitSlots[i].GetSlotUnitCount());
        }

        return armyInfo;
    }

    // On Destroy
    protected override void OnDestroy (){
        base.OnDestroy();
        GameGrid.Instance.GetGridCellInformation(GetGridPosition()).RemoveOccupyingObject();
    }
}