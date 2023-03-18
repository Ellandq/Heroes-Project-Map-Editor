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
    [SerializeField] public Vector2Int gridSize;
    [SerializeField] public string mapName;

    [Header ("Terrain information")]
    [SerializeField] private TerrainManager terrainManager;

    private void Awake ()
    {
        Instance = this;
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void ChangeMapName (string _name)
    {
        mapName = _name;
    }

    #region World Creation

    public void CreateGrid(Vector2Int size){
        gridSize = size;
        CameraManager.Instance.cameraMovement.UpdateMovementBorder(size);
        GameGrid.Instance.CreateGrid(size);
        terrainManager.SetUpTerrainManager(size);

    }  

    #endregion

    #region Save and Load System

    public void SaveMap ()
    {
        mapScriptableObject.mapSize = new Vector2Int(0, 0);
        mapScriptableObject.numberOfPlayers = 0;
        mapScriptableObject.players.Clear();
        mapScriptableObject.numberOfPossibleHumanPlayers = 0;
        mapScriptableObject.possibleHumanPlayers.Clear();

        mapScriptableObject.mapSize = gridSize;
        mapScriptableObject.numberOfPlayers = Convert.ToInt16(PlayerManager.Instance.existingPlayers.Count);;
        mapScriptableObject.players = PlayerManager.Instance.existingPlayers;
        mapScriptableObject.numberOfPossibleHumanPlayers = Convert.ToInt16(PlayerManager.Instance.existingPlayers.Count);
        mapScriptableObject.possibleHumanPlayers = PlayerManager.Instance.existingPlayers;


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
            mapWorldObjects.cities.AddRange(_city.GetComponent<City>().GetConvertedCityInformation());
            mapWorldObjects.citiesCount++;
        }

        foreach (GameObject _army in  WorldObjectManager.Instance.armies){
            mapWorldObjects.armies.AddRange(_army.GetComponent<Army>().GetConvertedArmyInformation());
            mapWorldObjects.armiesCount++;
        }

        foreach (GameObject _mine in  WorldObjectManager.Instance.mines){
            mapWorldObjects.mines.AddRange(_mine.GetComponent<Mine>().GetConvertedMineInformation());
            mapWorldObjects.minesCount++;
        }

        foreach (GameObject _resource in  WorldObjectManager.Instance.resources){
            mapWorldObjects.resources.AddRange(_resource.GetComponent<ResourcesObj>().GetConvertedResourceInformation());
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
