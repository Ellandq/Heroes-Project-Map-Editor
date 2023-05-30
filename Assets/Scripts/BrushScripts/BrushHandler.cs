using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushHandler : MonoBehaviour
{
    public static BrushHandler Instance;

    [Header ("Events")]
    public Action onLeftMouseButtonAction;
    public Action onLeftMouseButtonActionStarted;
    public Action onRightMouseButtonAction;
    public Action onNoButtonPressesDetected;
    public Action onInvalidActionDetected;

    [Header ("Frame Update Information")]
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private BrushMode currentBrushMode;
    private bool singlePressModeActivated;
    private bool actionTaken;
    private bool allowMultipleGridCellSelection;

    [Header ("Current Grid Brush Information")]
    [SerializeField] int brushSize;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    private Vector2Int currentPosition;
    private List<Vector2Int> currentSelectedCells;

    [Header ("Paint brush Information")]
    [SerializeField] private LineRenderer brush;
    [SerializeField] private float paintBrushSize;
    [SerializeField] private float thickness;
    [SerializeField] private int steps;
    [SerializeField] private bool enablePaintBrush;
    private Vector3 brushWorldPosition;

    [Header ("General Information")]
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

    // Input handle
    private void LeftMouseButtonInteraction (){
        if (!allowToolUsage || InputManager.Instance.mouseInput.IsMouseOverUI()){
            invalidUsage = !invalidUsage;
            onInvalidActionDetected?.Invoke();
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
            onLeftMouseButtonActionStarted?.Invoke();
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

    // Tool handle
    public void ChangeBrushMode (BrushMode mode){
        onLeftMouseButtonAction = null;
        onRightMouseButtonAction = null;
        onNoButtonPressesDetected = null;
        onInvalidActionDetected = null;
        allowToolUsage = true;

        currentBrushMode = mode;
        switch (currentBrushMode){
            case BrushMode.SelectTool:
                layersToHit = LayerMask.GetMask("WorldObjects");
                singlePressModeActivated = true;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(true);
                selectTool.ActivateSelectTool();
            break;

            case BrushMode.EraseTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(true);
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.TerrainTypeTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                ChangePaintBrushStatus(true);
                ChangeGridVisability(false);
                terrainTypeTool.ActivateTerrainTypeTool();
            break;

            case BrushMode.RoadTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(false);
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.WallTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(true);
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.ForestTool:
                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                checkHeightLevel = false;
                ChangePaintBrushStatus(true);
                ChangeGridVisability(false);
                // eraseTool.ActivateEraseTool();
            break;  

            case BrushMode.TerrainShaperTool:
                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = true;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(true);
                terrainShaperTool.ActivateTerrainShaperTool();
            break;

            case BrushMode.TerrainSlopeTool:
                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                checkHeightLevel = false;
                ChangePaintBrushStatus(false);
                ChangeGridVisability(true);
                terrainSlopeTool.ActivateTerrainSlopeTool();
            break;
        }
    }

    public void ChangeBrushSize (int size){
        brushSize = size;
    }

    public void ChangeBrushSize (float size){
        paintBrushSize = size;
    }

    public void ChangePaintBrushStatus (bool status){
        enablePaintBrush = status;
        brush.gameObject.SetActive(status);
        if (enablePaintBrush){
            InputManager.Instance.worldObjectInteractionManager.onTerrainHit.AddListener(CreatePaintBrush);
        }else{
            InputManager.Instance.worldObjectInteractionManager.onTerrainHit.RemoveListener(CreatePaintBrush);
        }
    }

    public void CreatePaintBrush (Vector3 worldPosition){
        brushWorldPosition = worldPosition;
        brush.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++){
            float circumferenceProgress = (float)currentStep/steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float zScaled = Mathf.Sin(currentRadian);

            float x = xScaled * paintBrushSize;
            float z = zScaled * paintBrushSize;

            Vector3 currentPosition =  new Vector3(x + worldPosition.x, worldPosition.y + 1f, z + worldPosition.z);

            brush.SetPosition(currentStep, currentPosition);
        }
        // Set the brush thickness
        brush.startWidth = thickness; 
        brush.endWidth = thickness;
    }

    // Other options
    public void UpdateCurrentAdjustedHeight (){
        currentAdjustedHeightLevel = terrainShaperTool.GetCurrentHeightLevel();
    }

    public void AllowToolUsage (bool status){
        allowToolUsage = status;
    }

    private void ChangeGridVisability (bool status){
        GameGrid.Instance.ChangeGridVisability(status);
    }

    public void ChangeToGridMode (){
        // Set the layer to hit
        layersToHit = LayerMask.GetMask("GridCell");
        ChangePaintBrushStatus(false);
        ChangeGridVisability(true);
    }

    public void ChangeToTerrainMode (){
        // Set the layer to hit
        layersToHit = LayerMask.GetMask("Terrain");

        ChangePaintBrushStatus(true);
        ChangeGridVisability(false);
    }

    // Highlights
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

    public Vector3 GetCurrentBrushWorldPosition (){
        return brushWorldPosition;
    }
}
