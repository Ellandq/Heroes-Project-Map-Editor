using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushHandler : MonoBehaviour
{
    public static BrushHandler Instance;

    [Header ("Events")]
    public Action onLeftMouseButtonAction;
    public Action onRightMouseButtonAction;
    public Action onNoButtonPressesDetected;

    [Header ("Frame Update Information")]
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private BrushMode currentBrushMode;
    private bool singlePressModeActivated;
    private bool actionTaken;
    private bool allowMultipleGridCellSelection;

    [Header ("Current brush information")]
    [SerializeField] int brushSize;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    private Vector2Int currentPosition;
    private List<Vector2Int> currentSelectedCells;
    private bool gridSelectionActivated;
    private bool allowToolUsage;
    private bool checkHeightLevel;
    private bool invalidUsage;
    private float currentAdjustedHeightLevel;


    [Header ("Tool References")]
    [SerializeField] private SelectTool selectTool;
    [SerializeField] private EraseTool eraseTool;
    [SerializeField] private TerrainTypeTool terrainTypeTool;
    [SerializeField] private RoadTool roadTool;
    [SerializeField] private WallTool wallTool;
    [SerializeField] private ForestTool forestTool;
    [SerializeField] private TerrainShaperTool terrainShaperTool;
    [SerializeField] private TerrainSlopeTool terrainSlopeTool;

    [Header ("Selection status")]
    private bool leftMouseButtonSelectionActivated;
    private bool rightMouseButtonSelectionActivated;
    
    private void Awake (){
        Instance = this;
        currentSelectedCells = new List<Vector2Int>();
        currentPosition = new Vector2Int(0, 0);
        actionTaken = true;
        allowToolUsage = false;
        invalidUsage = false;
        brushSize = 1;

        // Left mouse button interactions
        InputManager.Instance.mouseInput.onLeftMouseButton_Down += LeftMouseButtonInteraction;
        InputManager.Instance.mouseInput.onLeftMouseButton_Up += LeftMouseButtonInteraction;

        // Right mouse button interactions
        InputManager.Instance.mouseInput.onRightMouseButton_Down += RightMouseButtonInteraction;
        InputManager.Instance.mouseInput.onRightMouseButton_Up += RightMouseButtonInteraction;

        // Grid highlights
        InputManager.Instance.worldObjectInteractionManager.onNewGridCellHoveredOver.AddListener(HighlightCells);
    }

    // Update is called once per frame
    private void Update(){
        if (actionTaken){
            actionTaken = false;
            onNoButtonPressesDetected?.Invoke();
        }
    }

    private void LeftMouseButtonInteraction (){
        if (!allowToolUsage || InputManager.Instance.mouseInput.IsMouseOverUI()){
            invalidUsage = !invalidUsage;
            return;
        }
        if (invalidUsage){
            invalidUsage = false;
            return;
        }
        if (singlePressModeActivated){
            if (!actionTaken){
                UpdateCurrentAdjustedHeight();
                actionTaken = true;
            }else{
                onNoButtonPressesDetected?.Invoke();
            }
        }else if (!gridSelectionActivated){
            UpdateCurrentAdjustedHeight();
            gridSelectionActivated = true;
        }else{
            gridSelectionActivated = false;
            onLeftMouseButtonAction?.Invoke();
            onNoButtonPressesDetected?.Invoke();
        }
    }

    private void RightMouseButtonInteraction (){
        if (!allowToolUsage || InputManager.Instance.mouseInput.IsMouseOverUI()){
            invalidUsage = !invalidUsage;
            return;
        }
        if (invalidUsage){
            invalidUsage = false;
            return;
        }
        if (singlePressModeActivated){
            if (!actionTaken){
                onRightMouseButtonAction?.Invoke();
                actionTaken = true;
            }else{
                onNoButtonPressesDetected?.Invoke();
            }
        }else if (!gridSelectionActivated){
            UpdateCurrentAdjustedHeight();
            gridSelectionActivated = true;
        }else{
            gridSelectionActivated = false;
            onRightMouseButtonAction?.Invoke();
            onNoButtonPressesDetected?.Invoke();
        }
    }

    public void ChangeBrushMode (BrushMode mode){
        onLeftMouseButtonAction = null;
        onRightMouseButtonAction = null;
        onNoButtonPressesDetected = null;
        allowToolUsage = true;

        currentBrushMode = mode;
        switch (currentBrushMode){
            case BrushMode.SelectTool:
                layersToHit = LayerMask.GetMask("WorldObjects");
                singlePressModeActivated = true;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                selectTool.ActivateSelectTool();
            break;

            case BrushMode.EraseTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.TerrainTypeTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                // terrainTypeTool.ActivateTerrainTypeTool();
            break;

            case BrushMode.RoadTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.WallTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.ForestTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                // eraseTool.ActivateEraseTool();
            break;  

            case BrushMode.TerrainShaperTool:
                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = true;
                terrainShaperTool.ActivateTerrainShaperTool();
            break;

            case BrushMode.TerrainSlopeTool:
                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                terrainSlopeTool.ActivateTerrainSlopeTool();
            break;
        }
    }

    public void ChangeBrushSize (int size){
        brushSize = size;
    }

    public void UpdateCurrentAdjustedHeight (){
        currentAdjustedHeightLevel = terrainShaperTool.GetCurrentHeightLevel();
    }

    public void AllowToolUsage (bool status){
        allowToolUsage = status;
    }

    private void HighlightCells(Vector2Int position){
        List<Vector2Int> selectedCells = GameGrid.Instance.GetGridCellPositionList(position, brushSize);

        if (!gridSelectionActivated || !allowMultipleGridCellSelection){
            // Iterate over the cells in previousSelectedCells
            for (int i = currentSelectedCells.Count - 1; i >= 0; i--) {
                Vector2Int cellPosition = currentSelectedCells[i];
                // Check if the cell exists in selectedCells
                bool found = false;
                for (int j = 0; j < selectedCells.Count; j++) {
                    if (selectedCells[j] == cellPosition) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    GameGrid.Instance.GetGridCellInformation(cellPosition).ChangeCellHighlight(defaultColor);
                    currentSelectedCells.RemoveAt(i);
                }
            }
        }
        
        // Iterate over the cells in selectedCells
        for (int i = 0; i < selectedCells.Count; i++) {
            // Perform action on the cell
            Vector2Int cellPosition = selectedCells[i];
            if (!currentSelectedCells.Contains(cellPosition)) {
                if (checkHeightLevel && gridSelectionActivated){
                    if (GameGrid.Instance.GetGridCellInformation(cellPosition).GetHeightLevel() == currentAdjustedHeightLevel){
                        GameGrid.Instance.GetGridCellInformation(cellPosition).ChangeCellHighlight(highlightColor);
                        currentSelectedCells.Add(cellPosition);
                    }
                }else{
                    GameGrid.Instance.GetGridCellInformation(cellPosition).ChangeCellHighlight(highlightColor);
                    currentSelectedCells.Add(cellPosition);
                }
                
            }
        }
        currentPosition = position;
    }

    private void ClearHighlight (){
        foreach (Vector2Int cellPosition in currentSelectedCells){
            GameGrid.Instance.GetGridCellInformation(cellPosition).ChangeCellHighlight(defaultColor);
        }
        HighlightCells(currentPosition);
    }

    // getters
    public List<Vector2Int> GetCurrentSelectedGridCells(){
        return currentSelectedCells;
    }

    public Vector2Int GetCurrentSelectedPosition (){
        return currentPosition;
    }

}
