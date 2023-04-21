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
    private void InitialCitySetup (Vector2Int gridPosition)
    {    
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        cities.Add(Instantiate(cityPrefab, transform.position, Quaternion.identity));
        City city = cities[numberOfCities].GetComponent<City>();
        city = new City (gridPosition, transform.localEulerAngles, CityFraction.Random, PlayerTag.None);
        cities[numberOfCities].transform.parent = transform;
        cities[numberOfCities].gameObject.name = "City " + (numberOfCities + 1);
    
        numberOfCities++;   
    }
    #endregion

    #region Building
    private void InitialNeutralBuildingSetup (Vector2Int _gridPosition)
    {

    }
    #endregion

    #region Mine
    private void InitialMineSetup (Vector2Int gridPosition)
    {       
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        mines.Add(Instantiate(minePrefab, transform.position, Quaternion.identity));
        Mine mine = mines[numberOfMines].GetComponent<Mine>();
        mine = new Mine(gridPosition, transform.localEulerAngles, PlayerTag.None, ResourceType.Gold);
        mines[numberOfMines].transform.parent = transform;
        mines[numberOfMines].gameObject.name = "Mine : " + (numberOfMines + 1);     

        numberOfMines++;
    }
    #endregion

    #region Dwelling
    private void InitialDwellingSetup (Vector2Int _gridPosition)
    {

    }
    #endregion

    #region Army
    private void InitialArmySetup (Vector2Int gridPosition)
    {
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        Army army = armies[numberOfArmies].GetComponent<Army>();
        army = new Army(gridPosition, transform.localEulerAngles, PlayerTag.None);
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = "Army " + (numberOfArmies + 1);

        numberOfArmies++;   
    }
    #endregion

    #region Resources
    private void InitialResourceSetup (Vector2Int gridPosition)
    {
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }
        resources.Add(Instantiate(resourcePrefab, transform.position, Quaternion.identity));
        ResourcesObject resourcesObject = resources[numberOfResources].GetComponent<ResourcesObject>();
        resourcesObject = new ResourcesObject (gridPosition, transform.localEulerAngles, PlayerTag.None, ResourceType.Gold);
        resources[numberOfResources].transform.parent = transform;
        resources[numberOfResources].gameObject.name = "Resource: " + (numberOfResources + 1);

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

public enum CityBuildingStatus{
    NotBuilt, Built, Blocked
}

public enum BuildingType{
    OneByOne,
    TwoByOne, OneByTwo, TwoByTwo,
    ThreeByOne, ThreeByTwo, ThreeByThree,
    FiveByFive
}

public enum BuildingID{
    // Basic buildings
    VillageHall, TownHall, CityHall, Tavern, Prison, Fort, Citadel, Castle, Caravan, Shipyard,
    // Bonus buildings
    BonusBuilding_1, BonusBuilding_2, EquipementBuilding, RacialBuilding, GraalBuilding,
    // Magic buildings
    MagicHall_1, MagicHall_2, MagicHall_3, MagicHall_4, MagicHall_5, AdditionalMagic_1, AdditionalMagic_2,
    // Unit Buildings
    Tier1_1, Tier1_2, Tier2_1, Tier2_2, Tier3_1, Tier3_2, Tier4_1, Tier4_2,
    // Conditional ID's
    Tier_1, Tier_2, Tier_3, Tier_4
}
