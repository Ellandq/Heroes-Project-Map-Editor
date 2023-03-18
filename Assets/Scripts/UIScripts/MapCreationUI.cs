using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapCreationUI : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private TextMeshProUGUI sizeDisplay;


    private void UpdateDisplay ()
    {
        sizeDisplay.text = (Convert.ToString(gridSize.x) + " x " + Convert.ToString(gridSize.y));
    }

    public void ChangeDefaultSize (Vector2Int size){
        gridSize = size;
        UpdateDisplay();
    }

    public void ChangeSize(Vector2Int size)
    {
        gridSize = size;
        UpdateDisplay();
    }

    public void ChangeXSize (string str){
        gridSize.x = Convert.ToInt32(str);
        UpdateDisplay();
    }

    public void ChangeYSize (string str){
        gridSize.y = Convert.ToInt32(str);
        UpdateDisplay();
    }

    public void CreateGrid ()
    {
        EditorManager.Instance.CreateGrid(gridSize);
        this.gameObject.SetActive(false);
    }
}
