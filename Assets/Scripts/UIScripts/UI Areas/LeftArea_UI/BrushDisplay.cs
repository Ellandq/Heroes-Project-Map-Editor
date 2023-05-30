using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushDisplay : MonoBehaviour
{
    [Header ("Brushes")]
    [SerializeField] private List<GameObject> brushObjects;
    [SerializeField] public BrushMode currentBrushMode;

    [Header ("Eraser options")]
    [SerializeField] private int eraserBrushSize;

    [Header ("Terrain type options")]
    [SerializeField] private int terrainTypeBrushSize;
    [SerializeField] private TerrainType selectedTerrainType;

    [Header ("Road options")]
    [SerializeField] private int roadBrushSize;
    [SerializeField] private TerrainType selectedRoadType;

    [Header ("Wall options")]
    [SerializeField] private int wallBrushSize;

    [Header ("Forrest options")]
    [SerializeField] private int forrestBrushSize;
    [SerializeField] private ForrestType selectedForrestType;

    [Header ("TerrainShaper options")]
    [SerializeField] private float terrainShaperBrushSize;
    [SerializeField] private bool terrainRaising;

    [Header ("TerrainLevel options")]
    [SerializeField] private int terrainLevelBrushSize;
    [SerializeField] private bool slopeToolActivated;

    private void Awake (){
        ActivateTool(BrushMode.SelectTool);
    }
    
    private void DisableTool (){
        for (int i = 0; i < brushObjects.Count; i++){
            brushObjects[i].SetActive(false);
        }
    }

    public void ActivateTool (BrushMode mode){
        DisableTool();
        brushObjects[(int)mode].SetActive(true);
        BrushHandler.Instance.ChangeBrushMode(mode);
    }  
}

public enum BrushMode{
    SelectTool, EraseTool, TerrainTypeTool, RoadTool, WallTool, ForestTool, TerrainShaperTool, TerrainSlopeTool
}

public enum TerrainType{
    Grass_Lush, Rock, Sand
}

public enum ForrestType{

}