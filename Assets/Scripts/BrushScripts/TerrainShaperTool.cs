using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainShaperTool : MonoBehaviour
{
    [Header ("Brush Information")]
    [SerializeField] private int brushSize;
    [SerializeField] private float currentHeightLevel;

    [Header ("UI References")]
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private TMP_Text brushSizeDisplay;

    public void ActivateTerrainShaperTool(){
        BrushHandler.Instance.onLeftMouseButtonPressed.AddListener(HeightenTerrainLevel);
        BrushHandler.Instance.onRightMouseButtonPressed.AddListener(LowerTerrainLevel);
        BrushHandler.Instance.onNoButtonPressesDetected.AddListener(UpdateCurrentAdjustedHeightLevel);
        BrushHandler.Instance.onNoButtonPressesDetected.AddListener(TerrainManager.Instance.ResetSelectedGridCellList);
    }

    public void UpdateBrushSize (float size){
        brushSize = Convert.ToInt32(size) * 2 - 1;
        BrushHandler.Instance.ChangeBrushSize(brushSize);
        brushSizeDisplay.text = Convert.ToString(brushSize);
    }

    public void UpdateBrushSize (){
        BrushHandler.Instance.ChangeBrushSize(brushSize);
    }

    private void HeightenTerrainLevel (){
        if (currentHeightLevel < 4.5f){
            foreach (GridCell cell in BrushHandler.Instance.GetCurrentSelectedGridCells()){
                if (cell.GetHeightLevel() == currentHeightLevel){
                    TerrainManager.Instance.RaiseGridTerrainLevel(cell.GetPosition(), Mathf.FloorToInt(currentHeightLevel) + 1);
                }
            }
        } 
    }

    private void LowerTerrainLevel (){
        if (currentHeightLevel > 1.5f){
            foreach (GridCell cell in BrushHandler.Instance.GetCurrentSelectedGridCells()){
                if (cell.GetHeightLevel() == currentHeightLevel){
                    TerrainManager.Instance.LowerGridTerrainLevel(cell.GetPosition(), Mathf.FloorToInt(currentHeightLevel) - 1);
                }
            }
        } 
    }

    private void UpdateCurrentAdjustedHeightLevel(){
        currentHeightLevel = GameGrid.Instance.GetGridCellInformation(BrushHandler.Instance.GetCurrentSelectedPosition()).GetHeightLevel();
    }
}
