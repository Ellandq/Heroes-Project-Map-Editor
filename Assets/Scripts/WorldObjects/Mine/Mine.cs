using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineReady = false;
    [SerializeField] GameObject flag;

    [Header("Mine information")]
    [SerializeField] public GameObject ownedByPlayer;
    public PlayerTag playerTag;
    public ResourceType mineType;
    public Vector2Int gridPosition;
    private Vector3 position;
    public Vector3 rotation;
    public ObjectType objectType = ObjectType.Mine;

    [Header("Mine Garrison references")]
    [SerializeField] public List <int> garrisonSlotsID;
    [SerializeField] public List <int> garrisonSlotsCount;

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        playerTag = ownedByPlayer.GetComponent<Player>().playerTag;
    }

    private void ChangeOwningPlayer (GameObject _ownedByPlayer)
    {
        ownedByPlayer.GetComponent<Player>().ownedMines.Remove(this.gameObject);
        if (ownedByPlayer.name == "Neutral Player"){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        ownedByPlayer.GetComponent<Player>().ownedMines.Add(this.gameObject);
        playerTag = ownedByPlayer.GetComponent<Player>().playerTag;
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    public void MineInitialization (Vector2Int _gridPosition)
    {
        playerTag = PlayerTag.None;
        mineType = ResourceType.Gold;
        gridPosition = _gridPosition;
        transform.localEulerAngles = rotation;

        for (int i = 0; i < 7; i++){
            garrisonSlotsID.Add(0);
            garrisonSlotsCount.Add(0);
        }
    }

    public float GetMineRotation ()
    {
        return rotation.y;
    }

    public void ChangeMineRotation (int _rotation)
    {
        transform.localEulerAngles = new Vector3 (0, _rotation, 0);
        rotation = transform.localEulerAngles;
    }

    public void ChangeMineType (ResourceType _mineType)
    {
        mineType = _mineType;
    }

    public List<int> GetConvertedMineInformation ()
    {
        List<int> mineInfo = new List<int>();

        mineInfo.Add((int)ownedByPlayer.GetComponent<Player>().playerTag);
        mineInfo.Add(gridPosition.x);
        mineInfo.Add(gridPosition.y);
        mineInfo.Add(Convert.ToInt32(rotation.y));
        mineInfo.Add((int)mineType);

        for (int i = 0; i < 7; i++){
            mineInfo.Add(garrisonSlotsID[i]);
            mineInfo.Add(garrisonSlotsCount[i]);
        }

        return mineInfo;
    }
}
