using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainTypeTool : MonoBehaviour
{
    [Header ("UI References")]
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private TMP_Text brushSizeDisplay;
    [SerializeField] private TMP_Dropdown terrainTypeDropDown;
    [SerializeField] private Image terrainTypePreview;

    [Header ("Terrain Types")]
    [SerializeField] private List<Sprite> terrainPreviews;

    [Header ("Brush Information")]
    [SerializeField] private float brushSize;
    [SerializeField] private TerrainType terrainType;
    private bool terrainPainterActivated;

    private void Awake (){
        List<TMP_Dropdown.OptionData> terrainOptions = new List<TMP_Dropdown.OptionData>();

        foreach (TerrainType terrainType in Enum.GetValues(typeof(TerrainType)))
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(terrainType.ToString());
            terrainOptions.Add(optionData);
        }

        terrainTypeDropDown.options = terrainOptions;

        ChangeTerrainType((int)terrainType);
    }

    private void Update (){
        if (terrainPainterActivated){
            PaintTerrain();
        }
    }

    public void ActivateTerrainTypeTool (){
        terrainPainterActivated = false;
        BrushHandler.Instance.onLeftMouseButtonAction += ChangeTerrainPainterStatus;
        BrushHandler.Instance.onLeftMouseButtonActionStarted += EnableTerrainPainter;
        BrushHandler.Instance.onNoButtonPressesDetected += DisableTerrainPainter;
        BrushHandler.Instance.onInvalidActionDetected += DisableTerrainPainter;
        UpdateBrushSize();
        terrainTypeDropDown.value = (int)terrainType;
        ChangeTerrainType((int)terrainType);
    }

    public void UpdateBrushSize (float size){
        brushSize = size * 5;
        BrushHandler.Instance.ChangeBrushSize(brushSize);
        brushSizeDisplay.text = Convert.ToString(brushSize);
    }

    public void UpdateBrushSize (){
        BrushHandler.Instance.ChangeBrushSize(brushSize);
    }

    public void ChangeTerrainType (int type){
        this.terrainType = (TerrainType)type;
        terrainTypePreview.sprite = terrainPreviews[type];
        
    }

    private void PaintTerrain (){
        TerrainManager.Instance.PaintTerrain(BrushHandler.Instance.GetCurrentBrushWorldPosition(), brushSize, terrainType);
    }

    private void ChangeTerrainPainterStatus (){
        terrainPainterActivated = !terrainPainterActivated;
    }

    private void DisableTerrainPainter (){
        terrainPainterActivated = false;
    }

    private void EnableTerrainPainter (){
        terrainPainterActivated = true;
    }
}
