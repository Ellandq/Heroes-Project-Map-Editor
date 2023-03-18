using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Army : MonoBehaviour
{
    [SerializeField] GameObject flag;

    [Header("Army information")]
    [SerializeField] public GameObject ownedByPlayer;
    public Vector2Int gridPosition;
    public Vector3 rotation;
    public ObjectType objectType;
    public PlayerTag playerTag;

    [Header("Unit slots Information")]
    [SerializeField] public List <int> unitSlotsID;
    [SerializeField] public List <int> unitSlotsCount;

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        playerTag = ownedByPlayer.GetComponent<Player>().playerTag;
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
        playerTag = PlayerTag.None;
    }

    public void ArmyInitialization (Vector2Int _gridPosition)
    {
        playerTag = PlayerTag.None;
        gridPosition = _gridPosition;
        transform.localEulerAngles = rotation;

        for (int i = 0; i < 7; i++){
            unitSlotsID.Add(0);
            unitSlotsCount.Add(0);
        }
    }

    public void UpdateArmyGridPosition ()
    {
        gridPosition = GameGrid.Instance.GetGridPosFromWorld(this.gameObject.transform.position);
    }

    private void OnDestroy ()
    {
        try{
            GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
        }catch (MissingReferenceException){
            Debug.Log("Missing Reference Exception in Army script");
        }
        
    }

    public void ChangeArmyRotation (int _rotation)
    {
        transform.localEulerAngles = new Vector3 (0, _rotation, 0);
        rotation = transform.localEulerAngles;
    }

    public float GetArmyRotation ()
    {
        return rotation.y;
    }

    public List<int> GetConvertedArmyInformation ()
    {
        List<int> armyInfo = new List<int>();

        armyInfo.Add((int)ownedByPlayer.GetComponent<Player>().playerTag);
        armyInfo.Add(gridPosition.x);
        armyInfo.Add(gridPosition.y);
        armyInfo.Add(Convert.ToInt32(rotation.y));

        for (int i = 0; i < 7; i++){
            armyInfo.Add(unitSlotsID[i]);
            armyInfo.Add(unitSlotsCount[i]);
        }

        return armyInfo;
    }
}