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
    [SerializeField] public Player neutralPlayer;
    [SerializeField] public List<Player> players;

    [Header("Player colors")]
    [SerializeField] private List<Color> playerColors;

    public void Awake ()
    {
        Instance = this;
    }

    public Player GetPlayer (PlayerTag playerTag){
        return players[(int)playerTag];
    }

    public Color GetPlayerColor (PlayerTag playerTag){
        return playerColors[(int)playerTag];
    }
}

public enum PlayerTag{
    None, Blue, LightBlue, Purple, Red, Orange, Yellow, LightGreen, Green
}

[System.Serializable]
public class UnityPlayerEvent : UnityEvent<Player> { }