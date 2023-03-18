using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    internal short objectsToCreate = 0;
    internal short objectsCreated = 0;
    private short daysToLoose = 4;
    private GameObject lastObjectSelectedByPlayer;
    private bool playerLost;

    private GameObject objectToDestroy;

    [Header("Player basic information")]
    public string playerName;
    public string playerColorString;
    public Color playerColor;
    public bool isPlayerAi = true;
    public PlayerTag playerTag;

    [Header("Player structures and armies: ")]
    public List<GameObject> ownedArmies;
    public List<GameObject> ownedCities;
    public List<GameObject> ownedMines;

    private void Start ()
    {
        string[] str = this.name.Split(' ');
        playerColorString = str[0];
        playerName = str[1];
    }

    // Adds a new army for the player
    public void NewArmy(GameObject newArmy)
    {
        ownedArmies.Add(newArmy);
        ownedArmies[ownedArmies.Count - 1].GetComponent<Army>().AddOwningPlayer(this.gameObject);
    }

    // Removes a given army from the player
    public void RemoveArmy (GameObject armyToRemove)
    {    
        for (int i = 0; i < ownedArmies.Count; i++){
            if (ownedArmies[i].name == armyToRemove.name){
                ownedArmies.RemoveAt(i);
                Destroy(armyToRemove);
            }
        }
    }

    // Adds a new City for the player
    public void NewCity(GameObject newCity)
    {
        ownedCities.Add(newCity);
        ownedCities[ownedCities.Count - 1].GetComponent<City>().AddOwningPlayer(this.gameObject);
    }

    // Removes a given city from the player
    public void RemoveCity (GameObject cityToRemove)
    {    
        for (int i = 0; i < ownedCities.Count; i++){
            if (ownedArmies[i].name == cityToRemove.name){
                ownedArmies.RemoveAt(i);
                Destroy(cityToRemove);
            }
        }
    }

    // Adds a new mine for the player
    public void NewMine(GameObject newMine)
    {
        ownedMines.Add(newMine);
        ownedMines[ownedMines.Count - 1].GetComponent<Mine>().AddOwningPlayer(this.gameObject);
    }

    // Removes a given city from the player
    public void RemoveMine (GameObject mineToRemove)
    {    
        for (int i = 0; i < ownedCities.Count; i++){
            if (ownedArmies[i].name == mineToRemove.name){
                ownedArmies.RemoveAt(i);
                Destroy(mineToRemove);
            }
        }
    }
}