using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Player basic information")]
    public bool isPlayerAi = true;
    public PlayerTag playerTag;

    [Header("Player structures and armies: ")]
    public List<Army> ownedArmies;
    public List<City> ownedCities;
    public List<Mine> ownedMines;

    private void Start ()
    {
        PlayerManager.Instance.GetPlayerColor(playerTag);
    }

    // Adds a new army for the player
    public void AddArmy(Army newArmy){
        ownedArmies.Add(newArmy);
    }

    // Removes a given army from the player
    public void RemoveArmy (Army armyToRemove){    
        for (int i = 0; i < ownedArmies.Count; i++){
            if (ownedArmies[i].name == armyToRemove.name){
                ownedArmies.RemoveAt(i);
            }
        }
    }

    // Adds a new City for the player
    public void AddCity(City newCity){
        ownedCities.Add(newCity);
    }

    // Removes a given city from the player
    public void RemoveCity (City cityToRemove)
    {    
        for (int i = 0; i < ownedCities.Count; i++){
            if (ownedArmies[i].name == cityToRemove.name){
                ownedArmies.RemoveAt(i);
            }
        }
    }

    // Adds a new mine for the player
    public void AddMine(Mine newMine){
        ownedMines.Add(newMine);
    }

    // Removes a given city from the player
    public void RemoveMine (Mine mineToRemove){    
        for (int i = 0; i < ownedMines.Count; i++){
            if (ownedMines[i].name == mineToRemove.name){
                ownedMines.RemoveAt(i);
            }
        }
    }
}