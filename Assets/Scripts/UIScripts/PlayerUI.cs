using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [Header ("Create Player Objects")]
    [SerializeField] private TMP_Dropdown playersAvailableToCreateDropdown;
    [SerializeField] private Button createPlayerButton;

    [Header ("Remove Player Objects")]
    [SerializeField] private TMP_Dropdown playersCreatedDropdown;
    [SerializeField] private Button removePlayerButton;

    private void Awake ()
    {
        Instance = this;
    }

    private void OnEnable ()
    {
        UpdatePlayerMenu();
    }

    public void UpdatePlayerMenu()
    {
        // playersAvailableToCreateDropdown.ClearOptions();
        // playersCreatedDropdown.ClearOptions();
        // foreach (PlayerTag _player in PlayerManager.Instance.availablePlayers){
        //     playersAvailableToCreateDropdown.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), _player)));
        // } 
        // foreach (PlayerTag _player in PlayerManager.Instance.existingPlayers){
        //     playersCreatedDropdown.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(PlayerTag), _player)));
        // }  

        // if (playersAvailableToCreateDropdown.options.Count < 0){
        //     createPlayerButton.interactable = false;
        // }else{
        //     createPlayerButton.interactable = true;
        // }

        // if (playersCreatedDropdown.options.Count < 0){
        //     removePlayerButton.interactable = false;
        // } else{
        //     removePlayerButton.interactable = true;
        // }
    }

    public void CreatePlayer ()
    {
        if (playersAvailableToCreateDropdown.value != 0){
            //PlayerManager.Instance.CreatePlayer(playersAvailableToCreateDropdown.value);
            this.gameObject.SetActive(false);
        }
    }

    public void RemovePlayer ()
    {
        //PlayerManager.Instance.RemovePlayer(playersCreatedDropdown.value);
        this.gameObject.SetActive(false);
    }
}