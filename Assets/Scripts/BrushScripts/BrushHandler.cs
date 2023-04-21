using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrushHandler : MonoBehaviour
{
    public static BrushHandler Instance;

    [Header ("Events")]
    public UnityEvent onLeftMouseButtonPressed;
    public UnityEvent onRightMouseButtonPressed;
    public UnityEvent onNoButtonPressesDetected;

    [Header ("Frame Update Information")]
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private bool singlePressModeActivated;
    [SerializeField] private bool actionTaken;
    [SerializeField] private bool allowMultipleGridCellSelection;
    [SerializeField] private BrushMode currentBrushMode;

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
    
    private void Awake (){
        Instance = this;
        currentSelectedCells = new List<GridCell>();
        currentPosition = new Vector2Int(0, 0);
        actionTaken = true;
        allowToolUsage = true;
        brushSize = 1;
    }

    // Update is called once per frame
    private void Update(){
        // Check whether the mouse is over UI
        if (!allowToolUsage) return;
        if (InputManager.Instance.mouseInput.IsMouseOverUI() == false){
            // Check whether the current tool requires a single press or constant mouse state information
            if (!singlePressModeActivated){
                // If left mouse button is pressed
                if (InputManager.Instance.mouseInput.mouseButtonPressed_0){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        actionTaken = true;
                        onLeftMouseButtonPressed?.Invoke();
                        if (allowMultipleGridCellSelection) gridSelectionActivated = true;
                    }
                // If left button is pressed
                }else if (InputManager.Instance.mouseInput.mouseButtonPressed_1){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        actionTaken = true;
                        onRightMouseButtonPressed?.Invoke();
                        if (allowMultipleGridCellSelection) gridSelectionActivated = true;
                    }
                // If no buttons are pressed
                }else{
                    if (actionTaken){
                        actionTaken = false;
                        onNoButtonPressesDetected?.Invoke();
                        if (allowMultipleGridCellSelection) gridSelectionActivated = false;
                    }
                }
            }else{

                if (Input.GetMouseButtonDown(0)){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        onLeftMouseButtonPressed?.Invoke();
                    }
                }else if (Input.GetMouseButtonDown(1)){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        onRightMouseButtonPressed?.Invoke();
                    }
                }
            }
            Ray gridRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit gridHit;
            if (Physics.Raycast(gridRay, out gridHit, Mathf.Infinity, LayerMask.GetMask("GridCell"))){
                if (currentPosition != gridHit.collider.gameObject.GetComponent<GridCell>().GetPosition()){
                    HighlightCells(gridHit.collider.gameObject.GetComponent<GridCell>().GetPosition());
                }
            }
        }
    }

    public void ChangeBrushMode (BrushMode mode){
        currentBrushMode = mode;
        switch (currentBrushMode){
            case BrushMode.SelectTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("WorldObjects");
                singlePressModeActivated = true;
                allowMultipleGridCellSelection = false;
                selectTool.ActivateSelectTool();
            break;

            case BrushMode.EraseTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.TerrainTypeTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // terrainTypeTool.ActivateTerrainTypeTool();
            break;

            case BrushMode.RoadTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.WallTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.ForestTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = false;
                // eraseTool.ActivateEraseTool();
            break;  

            case BrushMode.TerrainShaperTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                allowMultipleGridCellSelection = true;
                terrainShaperTool.ActivateTerrainShaperTool();
            break;

            case BrushMode.TerrainSlopeTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

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

    public List<GridCell> GetCurrentSelectedGridCells(){
        return currentSelectedCells;
    }

    public Vector2Int GetCurrentSelectedPosition (){
        return currentPosition;
    }

}
