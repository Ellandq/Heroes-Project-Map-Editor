using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainSlopeTool : MonoBehaviour
{
    private List<Vector2Int> selectedGridCells;

    public void ActivateTerrainSlopeTool(){
        BrushHandler.Instance.onLeftMouseButtonPressed.AddListener(UpdateGridCellSelection);
        BrushHandler.Instance.onNoButtonPressesDetected.AddListener(AttemptSlopeCreation);
    }

    private void UpdateGridCellSelection (){
        if (!selectedGridCells.Contains(BrushHandler.Instance.GetCurrentSelectedPosition())){
            selectedGridCells.Add(BrushHandler.Instance.GetCurrentSelectedPosition());
        }
    }

    private void AttemptSlopeCreation (){
        if (selectedGridCells == null || selectedGridCells.Count != 3){
            selectedGridCells = new List<Vector2Int>();
        }else{
            TerrainManager.Instance.CreateSlope(selectedGridCells);
        }
    }
}
