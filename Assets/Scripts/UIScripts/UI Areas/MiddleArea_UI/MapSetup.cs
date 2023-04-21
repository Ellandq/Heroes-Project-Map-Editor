using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSetup : MonoBehaviour
{
    [Header ("Object References")]
    [SerializeField] private TMP_InputField mapName_InputField;
    [SerializeField] private TMP_InputField customSize_InputField;
    [SerializeField] private List<Image> sizeSelectionButtons;
    [SerializeField] private Button createMapButton;

    [Header ("Map Settings")]
    [SerializeField] private string mapName;
    [SerializeField] private bool enableUnderground;
    [SerializeField] private int mapSize;

    [Header ("Map Creation Status")]
    [SerializeField] private bool nameSelected;
    [SerializeField] private bool sizeSelected;

    [Header ("Button Sprites")]
    [SerializeField] private Sprite button_Selected;
    [SerializeField] private Sprite button_Deselected;

    private void Awake (){
        ResetDisplay();
    }

    public void ChangeSelectedMapSize (string size){
        ChangeSelectedMapSize(Convert.ToInt32(size));
    }

    private void ChangeSelectedMapSize (int size){
        if (size >= 40 && size < 400){
            mapSize = size;
            sizeSelected = true;
        }else{
            sizeSelected = false;
        }
        IsMapReady();
    }

    public void UpdateButtonDisplay (int buttonIndex){
        for (int i = 0; i < sizeSelectionButtons.Count; i++){
            if (i == buttonIndex) sizeSelectionButtons[i].sprite = button_Selected;
            else sizeSelectionButtons[i].sprite = button_Deselected;
        }

        if (buttonIndex != 7){
            ChangeSelectedMapSize(buttonIndex * 32 + 64);
            customSize_InputField.interactable = false;
        }else{
            customSize_InputField.interactable = true;
            sizeSelected = false;
        }
    }

    public void UpdateMapName(string name) {
        if (GetValidFolderName(name) == null){
            mapName = "";
            nameSelected = false;
            IsMapReady();
            return;
        }
        mapName = GetValidFolderName(name);
        nameSelected = true;
        IsMapReady();
    }

    private string GetValidFolderName(string inputName) {
        // Remove any characters that are not allowed in folder names
        char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
        string folderName = new string(inputName.Where(c => !invalidChars.Contains(c)).ToArray());

        // Trim any leading or trailing spaces
        folderName = folderName.Trim();

        // Ensure that the folder name is not an empty string
        if (string.IsNullOrEmpty(folderName)) {
            return null;
        }

        // Return the valid folder name
        return folderName;
    }

    private void ResetDisplay (){
        nameSelected = false;
        mapName = "";
        mapName_InputField.text = "";

        sizeSelected = false;
        mapSize = 0;
        customSize_InputField.text = "";
        for (int i = 0; i < sizeSelectionButtons.Count; i++){
            sizeSelectionButtons[i].sprite = button_Deselected;
        }
        IsMapReady();
    }

    private void IsMapReady (){
        if (nameSelected && sizeSelected) createMapButton.interactable = true;
        else createMapButton.interactable = false;
    }

    public void EnableDisplay (){
        this.gameObject.SetActive(true);
        ResetDisplay();
    }

    public void DisableDisplay (){
        this.gameObject.SetActive(false);
    }

    public void CreateMap (){
        EditorManager.Instance.CreateMap(mapName, mapSize, enableUnderground);
        UIManager.Instance.EnableUI();
        DisableDisplay();
    }
}
