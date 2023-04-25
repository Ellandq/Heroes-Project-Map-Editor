using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainShaperTool : MonoBehaviour
{
    private Coroutine raiseTerrain;
    private Coroutine lowerTerrain;
    private bool terrainoModificationCompleted;

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
        terrainoModificationCompleted = true;
        BrushHandler.Instance.onLeftMouseButtonAction += HeightenTerrainLevel;
        BrushHandler.Instance.onRightMouseButtonAction += LowerTerrainLevel;
        BrushHandler.Instance.onNoButtonPressesDetected += ResetHeightLevel;
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
            if (terrainoModificationCompleted){
                raiseTerrain = null;
                terrainoModificationCompleted = false;
                raiseTerrain = StartCoroutine(RaiseTerrain(new List<Vector2Int>(BrushHandler.Instance.GetCurrentSelectedGridCells())));
            }
        } 
    }

    private void LowerTerrainLevel (){
        if (!heightLevelAdjusted) UpdateCurrentAdjustedHeightLevel();
        if (currentHeightLevel > 1.5f){
            if (terrainoModificationCompleted){
                raiseTerrain = null;
                terrainoModificationCompleted = false;
                lowerTerrain = StartCoroutine(LowerTerrain(new List<Vector2Int>(BrushHandler.Instance.GetCurrentSelectedGridCells())));
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

    public float GetCurrentHeightLevel (){
        UpdateCurrentAdjustedHeightLevel();
        return Mathf.FloorToInt(currentHeightLevel);
    }

    public IEnumerator RaiseTerrain (List<Vector2Int> _gridCellList){
        List<Vector2Int> gridCellList = new List<Vector2Int> (_gridCellList);
        for (int i = 0; i < gridCellList.Count; i++){
            TerrainManager.Instance.RaiseGridTerrainLevel(gridCellList[i], Mathf.FloorToInt(currentHeightLevel) + 1, allowTallClifs);
            yield return null;
        }
        terrainoModificationCompleted = true;
        yield break;
    }

    public IEnumerator LowerTerrain (List<Vector2Int> _gridCellList){
        List<Vector2Int> gridCellList = new List<Vector2Int> (_gridCellList);
        for (int i = 0; i < gridCellList.Count; i++){
            TerrainManager.Instance.LowerGridTerrainLevel(gridCellList[i], Mathf.FloorToInt(currentHeightLevel) - 1, allowTallClifs);
            yield return null;
        }
        terrainoModificationCompleted = true;
        yield break;
    }
}
