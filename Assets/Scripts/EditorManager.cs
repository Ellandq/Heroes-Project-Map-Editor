using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance;

    [Header ("Map Information")]
    [SerializeField] private MapScriptableObject mapScriptableObject;
    [SerializeField] private MapWorldObjects mapWorldObjects;
    [SerializeField] public string mapName;
    [SerializeField] public int mapSize;
    [SerializeField] private bool mapCreated;

    [Header ("Terrain information")]
    [SerializeField] private TerrainManager terrainManager;

    private void Awake ()
    {
        Instance = this;
        mapCreated = false;
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void ChangeMapName (string _name)
    {
        mapName = _name;
    }

    #region World Creation

    public void CreateMap (string _mapName, int _mapSize, bool _enableUnderground){
        if (mapCreated) DestroyMap();
        // Set the map name
        mapName = _mapName;

        // Set the map size
        mapSize = _mapSize;

        // Setup the camera borders
        CameraManager.Instance.cameraMovement.UpdateMovementBorder(mapSize);

        // Create the grid
        GameGrid.Instance.CreateGrid(mapSize);

        // Create the terrain
        terrainManager.SetUpTerrainManager(mapSize);
    }

    private void DestroyMap (){

    }

    #endregion

    #region Save and Load System

    public void SaveMap ()
    {
        mapScriptableObject.mapSize = 0;
        mapScriptableObject.numberOfPlayers = 0;
        mapScriptableObject.players.Clear();
        mapScriptableObject.numberOfPossibleHumanPlayers = 0;
        mapScriptableObject.possibleHumanPlayers.Clear();

        mapScriptableObject.mapSize = mapSize;
        // mapScriptableObject.numberOfPlayers = Convert.ToInt16(PlayerManager.Instance.existingPlayers.Count);;
        // mapScriptableObject.players = PlayerManager.Instance.existingPlayers;
        // mapScriptableObject.numberOfPossibleHumanPlayers = Convert.ToInt16(PlayerManager.Instance.existingPlayers.Count);
        // mapScriptableObject.possibleHumanPlayers = PlayerManager.Instance.existingPlayers;


        mapWorldObjects.cities = new List<int>();
        mapWorldObjects.citiesCount = 0;
        mapWorldObjects.armies = new List<int>();
        mapWorldObjects.armiesCount = 0;
        mapWorldObjects.mines = new List<int>();
        mapWorldObjects.minesCount = 0;
        mapWorldObjects.resources = new List<int>();
        mapWorldObjects.resourcesCount = 0;

        mapWorldObjects.mapName = mapName;
        
        foreach (GameObject _city in  WorldObjectManager.Instance.cities){
            mapWorldObjects.cities.AddRange(_city.GetComponent<City>().GetConvertedObjectInformation());
            mapWorldObjects.citiesCount++;
        }

        foreach (GameObject _army in  WorldObjectManager.Instance.armies){
            mapWorldObjects.armies.AddRange(_army.GetComponent<Army>().GetConvertedObjectInformation());
            mapWorldObjects.armiesCount++;
        }

        foreach (GameObject _mine in  WorldObjectManager.Instance.mines){
            mapWorldObjects.mines.AddRange(_mine.GetComponent<Mine>().GetConvertedObjectInformation());
            mapWorldObjects.minesCount++;
        }

        foreach (GameObject _resource in  WorldObjectManager.Instance.resources){
            mapWorldObjects.resources.AddRange(_resource.GetComponent<ResourcesObject>().GetConvertedObjectInformation());
            mapWorldObjects.resourcesCount++;
        }
       SaveMapToFile();
    }

    private bool IsSaveFile()
    {
        return Directory.Exists(Application.dataPath + "/CreatedMaps");
    }

    private void SaveMapToFile()
    {
        if (mapName != null){
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.dataPath + "/CreatedMaps");
            }
            if (!Directory.Exists(Application.dataPath + "/CreatedMaps/" + mapName))
            {
                Directory.CreateDirectory(Application.dataPath + "/CreatedMaps/" + mapName);
            }
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Create(Application.dataPath + "/CreatedMaps/" + mapName + "/MapWorldObjects.txt");
            var json = JsonUtility.ToJson(mapWorldObjects);
            bf.Serialize(file, json);

            file = File.Create(Application.dataPath + "/CreatedMaps/" + mapName + "/MapInformation.txt");
            json = JsonUtility.ToJson(mapScriptableObject);
            bf.Serialize(file, json);

            file.Close();
            Debug.Log("Map Succesfully created. ");
        }else{
            Debug.Log("Map name is empty");
        }
        
    }

    #endregion
}
