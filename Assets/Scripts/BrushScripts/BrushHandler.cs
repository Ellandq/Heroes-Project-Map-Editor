using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private List<GridCell> currentSelectedCells;
    private bool gridSelectionActivated;
    private bool allowToolUsage;

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
        currentSelectedCells = new List<GridCell>();
        currentPosition = new Vector2Int(0, 0);
        actionTaken = true;
        allowToolUsage = true;
        brushSize = 1;

        InputManager.Instance.mouseInput.onLeftMouseButton_Down += ChangeLeftMouseButtonSelectionStatus;
        InputManager.Instance.mouseInput.onLeftMouseButton_Up += ChangeLeftMouseButtonSelectionStatus;
        InputManager.Instance.mouseInput.onRightMouseButton_Down += ChangeRightMouseButtonSelectionStatus;
        InputManager.Instance.mouseInput.onRightMouseButton_Up += ChangeRightMouseButtonSelectionStatus;

        InputManager.Instance.worldObjectInteractionManager.onNewGridCellHoveredOver.AddListener(HighlightCells);
    }

    // Update is called once per frame
    private void Update(){
        if (InputManager.Instance.mouseInput.IsMouseOverUI() || !allowToolUsage) return;

        if (leftMouseButtonSelectionActivated) LeftMouseButtonInteraction();
        else if  (rightMouseButtonSelectionActivated) RightMouseButtonInteraction();
        else if (actionTaken){
            actionTaken = false;
            onNoButtonPressesDetected?.Invoke();
        }

        // Ray gridRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit gridHit;
        // if (Physics.Raycast(gridRay, out gridHit, Mathf.Infinity, LayerMask.GetMask("GridCell"))){
        //     if (currentPosition != gridHit.collider.gameObject.GetComponent<GridCell>().GetPosition()){
        //         HighlightCells(gridHit.collider.gameObject.GetComponent<GridCell>().GetPosition());
        //     }
        // }
    }

    private void LeftMouseButtonInteraction (){
        if (singlePressModeActivated){
            if (!actionTaken){
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                    onLeftMouseButtonAction?.Invoke();
                }
            }
        }else{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                actionTaken = true;
                onLeftMouseButtonAction?.Invoke();
                if (allowMultipleGridCellSelection) gridSelectionActivated = true;
            }
        }
    }

    private void RightMouseButtonInteraction (){
        if (singlePressModeActivated){
            if (!actionTaken){
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                    onRightMouseButtonAction?.Invoke();
                }
            }
        }else{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                actionTaken = true;
                onRightMouseButtonAction?.Invoke();
                if (allowMultipleGridCellSelection) gridSelectionActivated = true;
            }
        }
    }

    public void ChangeBrushMode (BrushMode mode){
        currentBrushMode = mode;
        switch (currentBrushMode){
            case BrushMode.SelectTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("WorldObjects");
                singlePressModeActivated = true;
                allowMultipleGridCellSelection = false;
                selectTool.ActivateSelectTool();
            break;

            case BrushMode.EraseTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.TerrainTypeTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // terrainTypeTool.ActivateTerrainTypeTool();
            break;

            case BrushMode.RoadTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.WallTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.ForestTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                // eraseTool.ActivateEraseTool();
            break;  

            case BrushMode.TerrainShaperTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                terrainShaperTool.ActivateTerrainShaperTool();
            break;

            case BrushMode.TerrainSlopeTool:
                onLeftMouseButtonAction = null;
                onRightMouseButtonAction = null;

                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                terrainSlopeTool.ActivateTerrainSlopeTool();
            break;
        }
    }

    public void ChangeBrushSize (int size){
        brushSize = size;
    }

    public void AllowToolUsage (bool status){
        allowToolUsage = status;
    }

    public void ChangeLeftMouseButtonSelectionStatus (){
        leftMouseButtonSelectionActivated = !leftMouseButtonSelectionActivated;
    }

    public void ChangeRightMouseButtonSelectionStatus (){
        rightMouseButtonSelectionActivated = !rightMouseButtonSelectionActivated;
    }

    private void HighlightCells(Vector2Int position){
        List<GridCell> selectedCells = GameGrid.Instance.GetGridCellList(position, brushSize);

        // Iterate over the cells in previousSelectedCells
        foreach (GridCell cell in currentSelectedCells) {
            // Check if the cell exists in selectedCells
            if (!selectedCells.Contains(cell)) {
                cell.ChangeCellHighlight(defaultColor);
            }
        }

        // Iterate over the cells in selectedCells
        foreach (GridCell cell in selectedCells) {
            // Perform action on the cell
            if (!currentSelectedCells.Contains(cell)) {
                cell.ChangeCellHighlight(highlightColor);
            }
        }
        currentPosition = position;
        currentSelectedCells = selectedCells;
    }

    // getters
    public List<GridCell> GetCurrentSelectedGridCells(){
        return currentSelectedCells;
    }

    public Vector2Int GetCurrentSelectedPosition (){
        return currentPosition;
    }

}
