using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainSlopeTool : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> selectedGridCells;

    public void ActivateTerrainSlopeTool(){
        BrushHandler.Instance.ChangeBrushSize(1);
        BrushHandler.Instance.onLeftMouseButtonAction += UpdateGridCellSelection;
        BrushHandler.Instance.onRightMouseButtonAction += AttemptSlopeRemoval;
        BrushHandler.Instance.onNoButtonPressesDetected += AttemptSlopeCreation;
        selectedGridCells = new List<Vector2Int>();
    }

    private void UpdateGridCellSelection (){
        if (!selectedGridCells.Contains(BrushHandler.Instance.GetCurrentSelectedPosition())){
            selectedGridCells.Add(BrushHandler.Instance.GetCurrentSelectedPosition());
        }
    }

    private void AttemptSlopeCreation (){
        if (selectedGridCells == null || (selectedGridCells.Count != 3 && selectedGridCells.Count != 2)){
            selectedGridCells = new List<Vector2Int>();
        }else if (selectedGridCells.Count == 3){
            TerrainManager.Instance.CreateSlope(selectedGridCells);
            selectedGridCells = new List<Vector2Int>();
        }else if (selectedGridCells.Count == 2){
            TerrainManager.Instance.CreateSlope(selectedGridCells[0], selectedGridCells[1]);
            selectedGridCells = new List<Vector2Int>();
        }
    }

    private void AttemptSlopeRemoval (){
        if (GameGrid.Instance.GetGridCellInformation(BrushHandler.Instance.GetCurrentSelectedPosition()).IsSlope()){
            TerrainManager.Instance.RemoveSlope (BrushHandler.Instance.GetCurrentSelectedPosition());
        }
    }
}
