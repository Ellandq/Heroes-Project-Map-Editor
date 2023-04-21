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
    [SerializeField] private bool heightLevelAdjusted;
    [SerializeField] private bool allowTallClifs;

    [Header ("UI References")]
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private TMP_Text brushSizeDisplay;
    [SerializeField] private Button allowTallClifs_Button;

    [Header ("Button Sprites")]
    [SerializeField] private Sprite buttonSprite_enabled;
    [SerializeField] private Sprite buttonSprite_disabled;

    public void ActivateTerrainShaperTool(){
        BrushHandler.Instance.onLeftMouseButtonAction += HeightenTerrainLevel;
        BrushHandler.Instance.onRightMouseButtonAction += LowerTerrainLevel;
        BrushHandler.Instance.onNoButtonPressesDetected += ResetHeightLevel;
        BrushHandler.Instance.onNoButtonPressesDetected += TerrainManager.Instance.ResetSelectedGridCellList;
        UpdateBrushSize();
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
        if (!heightLevelAdjusted) UpdateCurrentAdjustedHeightLevel();
        if (currentHeightLevel < 4.5f){
            foreach (GridCell cell in BrushHandler.Instance.GetCurrentSelectedGridCells()){
                if (cell.GetHeightLevel() == currentHeightLevel){
                    TerrainManager.Instance.RaiseGridTerrainLevel(cell.GetPosition(), Mathf.FloorToInt(currentHeightLevel) + 1, allowTallClifs);
                }
            }
        } 
    }

    private void LowerTerrainLevel (){
        if (!heightLevelAdjusted) UpdateCurrentAdjustedHeightLevel();
        if (currentHeightLevel > 1.5f){
            foreach (GridCell cell in BrushHandler.Instance.GetCurrentSelectedGridCells()){
                if (cell.GetHeightLevel() == currentHeightLevel){
                    TerrainManager.Instance.LowerGridTerrainLevel(cell.GetPosition(), Mathf.FloorToInt(currentHeightLevel) - 1, allowTallClifs);
                }
            }
        } 
    }

    private void UpdateCurrentAdjustedHeightLevel(){
        currentHeightLevel = GameGrid.Instance.GetGridCellInformation(BrushHandler.Instance.GetCurrentSelectedPosition()).GetHeightLevel();
        heightLevelAdjusted = true;
    }

    private void ResetHeightLevel (){
        heightLevelAdjusted = false;
    }

    public void AllowTallClifs (){
        if (allowTallClifs){
            allowTallClifs = false;
            allowTallClifs_Button.image.sprite = buttonSprite_disabled;
        }
        else {
            allowTallClifs = true;
            allowTallClifs_Button.image.sprite = buttonSprite_enabled;
        }
    }
}
