using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : WorldObject
{
    [Header("Mine information")]
    [SerializeField] private ResourceType mineType;

    [Header("Mine Garrison references")]
    [SerializeField] private List<UnitSlot> unitSlots;

    public Mine (Vector2Int gridPosition, Vector3 rotation, PlayerTag ownedByPlayer = PlayerTag.None, ResourceType resourceType = ResourceType.Gold, ObjectType objectType = ObjectType.Mine)
        : base(gridPosition, rotation, objectType, ownedByPlayer)
    {
        ChangeOwningPlayer(ownedByPlayer);
        ChangeMineType(mineType);

        // Unit Slots initialization
        unitSlots = new List<UnitSlot>(7);
        for (int i = 0; i < unitSlots.Count; i++) unitSlots[i].SetSlotStatus(0, 0);
    }

    public override void ChangeOwningPlayer (PlayerTag ownedByPlayer = PlayerTag.None){ 
        PlayerManager.Instance.GetPlayer(ownedByPlayer).RemoveMine(this);
        base.ChangeOwningPlayer(ownedByPlayer);
        PlayerManager.Instance.GetPlayer(ownedByPlayer).AddMine(this);
        if (ownedByPlayer != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColor(ownedByPlayer);  // Get color from player
        }else{
            flag.SetActive(false);
        }
    }

    public void ChangeMineType (ResourceType _mineType)
    {
        mineType = _mineType;
    }

    public override List<int> GetConvertedObjectInformation ()
    {
        List<int> mineInfo = new List<int>();

        mineInfo.Add((int)GetPlayerTag());
        mineInfo.Add(GetGridPosition().x);
        mineInfo.Add(GetGridPosition().y);
        mineInfo.Add(Convert.ToInt32(GetRotation()));
        mineInfo.Add((int)mineType);

        for (int i = 0; i < 7; i++){
            mineInfo.Add(unitSlots[i].GetSlotUnitID());
            mineInfo.Add(unitSlots[i].GetSlotUnitCount());
        }

        return mineInfo;
    }
}
