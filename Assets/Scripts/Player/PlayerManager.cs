using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private string defaultPlayerName = "Player";

    public UnityEvent onPlayerCountChange;

    [Header("Player information")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] public GameObject neutralPlayer;
    public List<GameObject> players;

    [SerializeField] public List<PlayerTag> availablePlayers;
    [SerializeField] public List<PlayerTag> existingPlayers;

    [Header("Player colors")]
    [SerializeField] private Color blue;
    [SerializeField] private Color lightBlue;
    [SerializeField] private Color purple;
    [SerializeField] private Color red;
    [SerializeField] private Color orange;
    [SerializeField] private Color yellow;
    [SerializeField] private Color lightGreen;
    [SerializeField] private Color green;

    public void Awake ()
    {
        Instance = this;
    }

    // Creates a new player
    public void CreatePlayer(int index)
    {
        players.Add(Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        players[players.Count - 1].GetComponent<Player>();
        players[players.Count - 1].transform.parent = transform;
        players[players.Count - 1].gameObject.name = Enum.GetName(typeof(PlayerTag), availablePlayers[index]) + " " + defaultPlayerName;
        players[players.Count - 1].GetComponent<Player>().playerColor = AssignPlayerColour(Enum.GetName(typeof(PlayerTag), availablePlayers[index]));
        players[players.Count - 1].GetComponent<Player>().playerTag = availablePlayers[index];
        existingPlayers.Add(availablePlayers[index]);
        availablePlayers.RemoveAt(index);
        PlayerUI.Instance.UpdatePlayerMenu();
        onPlayerCountChange?.Invoke();
    }

    public void RemovePlayer(int index)
    {
        availablePlayers.Add(existingPlayers[index]);
        existingPlayers.RemoveAt(index);
        Destroy(players[index].gameObject);
        players.RemoveAt(index);
        PlayerUI.Instance.UpdatePlayerMenu();
        onPlayerCountChange?.Invoke();
    }

    // Sets the player color 
    private Color AssignPlayerColour (string playerColour)
    {
        switch (playerColour){
            case "Blue":
                return blue;

            case "LightBlue":
                return lightBlue;

            case "Purple":
                return purple;

            case "Red":
                return red;

            case "Orange":
                return orange;

            case "Yellow":
                return yellow;

            case "LightGreen":
                return lightGreen;

            case "Green":
                return green;
            default: 
                return blue;
        }
    }

    public int GetCurrentPlayerIndex (PlayerTag playerTag)
    {
        for (int i = 0; i < existingPlayers.Count; i++)
        {
            if (existingPlayers[i] == playerTag){
                return i + 1;
            }
        }
        return 0;
    }
}

public enum PlayerTag{
    None, Blue, LightBlue, Purple, Red, Orange, Yellow, LightGreen, Green
}

[System.Serializable]
public class UnityPlayerEvent : UnityEvent<Player> { }