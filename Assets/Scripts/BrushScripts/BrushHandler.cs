using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrushHandler : MonoBehaviour
{
    public static BrushHandler Instance;
    public UnityEvent onLeftMouseButtonPressed;
    public UnityEvent onRightMouseButtonPressed;
    public UnityEvent onNoButtonPressesDetected;

    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private bool singlePressModeActivated;

    [SerializeField] private BrushMode currentBrushMode;

    [Header ("Current brush information")]
    [SerializeField] int brushSize;
    private Vector2Int currentPosition;
    private List<GridCell> currentSelectedCells;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;

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
        CreateBrushIndicator();
        currentSelectedCells = new List<GridCell>();
        currentPosition = new Vector2Int(0, 0);
        brushSize = 1;
    }

    // Update is called once per frame
    private void Update(){
        // Check whether the mouse is over UI
        if (InputManager.Instance.mouseInput.IsMouseOverUI() == false){
            // Check whether the current tool requires a single press or constant mouse state information
            if (!singlePressModeActivated){
                if (Input.GetMouseButton(0)){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        
                        onLeftMouseButtonPressed?.Invoke();
                    }
                }else if (Input.GetMouseButton(1)){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        onRightMouseButtonPressed?.Invoke();
                    }
                }else{
                    onNoButtonPressesDetected?.Invoke();
                }
            }else{
                if (InputManager.Instance.mouseInput.mouseButtonPressed_0){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        onLeftMouseButtonPressed?.Invoke();
                    }
                }else if (InputManager.Instance.mouseInput.mouseButtonPressed_1){
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit)){
                        onRightMouseButtonPressed?.Invoke();
                    }
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

    public void ChangeBrushMode (BrushMode mode){
        currentBrushMode = mode;
        switch (currentBrushMode){
            case BrushMode.SelectTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("WorldObjects");
                singlePressModeActivated = true;
                selectTool.ActivateSelectTool();
            break;

            case BrushMode.EraseTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.TerrainTypeTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                // terrainTypeTool.ActivateTerrainTypeTool();
            break;

            case BrushMode.RoadTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.WallTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                // eraseTool.ActivateEraseTool();
            break;

            case BrushMode.ForestTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("Terrain");
                singlePressModeActivated = false;
                // eraseTool.ActivateEraseTool();
            break;  

            case BrushMode.TerrainShaperTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                terrainShaperTool.ActivateTerrainShaperTool();
            break;

            case BrushMode.TerrainSlopeTool:
                onLeftMouseButtonPressed.RemoveAllListeners();
                onRightMouseButtonPressed.RemoveAllListeners();

                layersToHit = LayerMask.GetMask("GridCell");
                singlePressModeActivated = false;
                terrainSlopeTool.ActivateTerrainSlopeTool();
            break;
        }
    }

    public void ChangeBrushSize (int size){
        brushSize = size;
    }

    private void CreateBrushIndicator (){
        
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
