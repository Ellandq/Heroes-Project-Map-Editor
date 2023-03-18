using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MonoBehaviour
{
    [SerializeField] GameObject flag;

    public ObjectType objectType = ObjectType.City;

    [Header("Main city information")]
    [SerializeField] public GameObject ownedByPlayer;
    public CityFraction cityFraction;
    public Vector2Int gridPosition;
    private Vector3 position;
    public Vector3 rotation;
    public PlayerTag playerTag;

    [Header("Garrison refrences")]
    [SerializeField] public List <int> garrisonSlotsID;
    [SerializeField] public List <int> garrisonSlotsCount;

    [Header ("City Buildings")]
    public List<Int16> cityBuildings;

    public void CityInitialization (Vector2Int _gridPosition)
    {
        playerTag = PlayerTag.None;
        gridPosition = _gridPosition;
        transform.localEulerAngles = rotation;
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;

        for (int i = 0; i < 30; i++){
            cityBuildings.Add(0);
        }

        for (int i = 0; i < 7; i++){
            garrisonSlotsID.Add(0);
            garrisonSlotsCount.Add(0);
        }
        
        cityBuildings[0] = 1;
        cityBuildings[4] = 1;

        
    }

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
        ownedByPlayer.GetComponent<Player>().ownedCities.Remove(this.gameObject);
        if (ownedByPlayer.name == "Neutral Player"){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        ownedByPlayer.GetComponent<Player>().ownedCities.Add(this.gameObject);
        playerTag = ownedByPlayer.GetComponent<Player>().playerTag;
    }

    public void ChangeCityRotation (int _rotation)
    {
        transform.localEulerAngles = new Vector3 (0, _rotation, 0);
        rotation = transform.localEulerAngles;
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
        playerTag = PlayerTag.None;
    }

    public float GetCityRotation ()
    {
        return rotation.y;
    }

    public void AddUnits (int unitID, int unitCount, int garrisonIndex){
        
    }

    public void ChangeBuildingStatus (BuildingID id, short status)
    {
        cityBuildings[(int)id] = status;
    }

    public void ChangeCityFraction (CityFraction _fraction)
    {
        cityFraction = _fraction;
    }

    public List<int> GetConvertedCityInformation ()
    {
        List<int> cityInfo = new List<int>();

        cityInfo.Add((int)ownedByPlayer.GetComponent<Player>().playerTag);
        cityInfo.Add(gridPosition.x);
        cityInfo.Add(gridPosition.y);
        cityInfo.Add(Convert.ToInt32(rotation.y));
        cityInfo.Add((int)cityFraction);
        
        for (int i = 0; i < cityBuildings.Count; i++){
            cityInfo.Add(cityBuildings[i]);
        }

        for (int i = 0; i < 7; i++){
            cityInfo.Add(garrisonSlotsID[i]);
            cityInfo.Add(garrisonSlotsCount[i]);
        }

        return cityInfo;
    }
}
