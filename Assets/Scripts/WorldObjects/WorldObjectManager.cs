using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;
    Player chosenPlayer;

    [Header("Object prefabs")]
    [SerializeField] private GameObject cityPrefab;
    [SerializeField] private GameObject armyPrefab;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private GameObject resourcePrefab;

    [Header("Cities information")]
    public List<GameObject> cities;
    public int numberOfCities = 0;

    [Header("Armies information")]
    public List<GameObject> armies;
    public int numberOfArmies = 0;

    [Header("Mines information")]
    public List<GameObject> mines;
    public int numberOfMines = 0;

    [Header("Resources information")]
    public List<GameObject> resources;
    public int numberOfResources = 0;
    
    private void Start ()
    {
        Instance = this;
    }

    public void CreateObject (ObjectType objectType, Vector2Int gridPosition)
    {
        switch (objectType){
            case ObjectType.City:
                InitialCitySetup(gridPosition);
            break;

            case ObjectType.Army:
                InitialArmySetup(gridPosition);
            break;

            case ObjectType.Mine:
                InitialMineSetup(gridPosition);
            break;

            case ObjectType.Dwelling:
                InitialDwellingSetup(gridPosition);
            break;

            case ObjectType.Building:
                //InitialBuildingSetup(gridPosition);
            break;

            case ObjectType.Resource:
                InitialResourceSetup(gridPosition);
            break;
        }
    }

    #region Enviroment
    private void InitialEnviromentSetup ()
    {
        //for now it's gonna be empty
    }
    #endregion

    #region City
    private void InitialCitySetup (Vector2Int _gridPosition)
    {
        Vector2Int gridPosition = _gridPosition;
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        cities.Add(Instantiate(cityPrefab, objectPosition, Quaternion.identity));
        cities[numberOfCities].GetComponent<City>().CityInitialization(
            gridPosition
        );
        cities[numberOfCities].transform.parent = transform;
        cities[numberOfCities].gameObject.name = "City " + (numberOfCities + 1);
   
        chosenPlayer = PlayerManager.Instance.neutralPlayer.GetComponent<Player>();
        chosenPlayer.NewCity(cities[numberOfCities]);
    
        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfCities++;   
    }
    #endregion

    #region Building
    private void InitialNeutralBuildingSetup (Vector2Int _gridPosition)
    {

    }
    #endregion

    #region Mine
    private void InitialMineSetup (Vector2Int _gridPosition)
    {
        Vector2Int gridPosition = _gridPosition;
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        mines.Add(Instantiate(minePrefab, objectPosition, Quaternion.identity));
        mines[numberOfMines].GetComponent<Mine>().MineInitialization(gridPosition);
        mines[numberOfMines].transform.parent = transform;
        mines[numberOfMines].gameObject.name = "Mine : " + (numberOfMines + 1);
        
        chosenPlayer = PlayerManager.Instance.neutralPlayer.GetComponent<Player>();
        chosenPlayer.NewMine(mines[numberOfMines]);
        

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfMines++;
    }
    #endregion

    #region Dwelling
    private void InitialDwellingSetup (Vector2Int _gridPosition)
    {

    }
    #endregion

    #region Army
    private void InitialArmySetup (Vector2Int _gridPosition)
    {
        Vector2Int gridPosition = _gridPosition;

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().ArmyInitialization(gridPosition);
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = "Army " + (numberOfArmies + 1);

        
        chosenPlayer = PlayerManager.Instance.neutralPlayer.GetComponent<Player>();
        chosenPlayer.NewArmy(armies[numberOfArmies]);

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfArmies++;   
    }

    public void RemoveArmy (GameObject selectedArmy)
    {
        chosenPlayer = selectedArmy.GetComponent<Army>().ownedByPlayer.GetComponent<Player>();
        chosenPlayer.RemoveArmy(selectedArmy);
        for (int i = 0; i < armies.Count; i++){
            if (armies[i].name == selectedArmy.name){
                armies.RemoveAt(i);
            }
        }
        numberOfArmies--;  
    }
    #endregion

    #region Resources
    private void InitialResourceSetup (Vector2Int _gridPosition)
    {
        Vector2Int gridPosition = _gridPosition;

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }
        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        resources.Add(Instantiate(resourcePrefab, objectPosition, Quaternion.identity));
        resources[numberOfResources].GetComponent<ResourcesObj>().ResourceInitialization(gridPosition);
        resources[numberOfResources].transform.parent = transform;
        resources[numberOfResources].gameObject.name = "Resource: " + (numberOfResources + 1);

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfResources++;  
    }
    #endregion

    #region Artifacts
    private void InitialArtifactSetup (string [] _splitLine)
    {

    }
    #endregion
}

public enum CityFraction{
    Random, Bazaar, Coalition, DarkOnes, Hive, Magic, Temple
}

public enum ObjectType{
    City, Army, Mine, Dwelling, Building, Resource
}

public enum ResourceType{
    Gold, Wood, Ore, Gems, Mercury, Sulfur, Crystal
}
