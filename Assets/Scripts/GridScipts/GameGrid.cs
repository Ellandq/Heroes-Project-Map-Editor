using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;
    public float gridSpaceSize = 5f;
    private int gridSize;

    [SerializeField] private GameObject gridCellPrefab;
    public GameObject[,] gameGrid;
    public bool gridCreated;

    void Start ()
    {
        Instance = this;
        gridCreated = false;
    }

    // Creates the grid when the game starts
    public void CreateGrid(int _gridSize)
    {
        if (!gridCreated){
            gridSize = _gridSize;
            gridCreated = true;
            gameGrid = new GameObject[gridSize, gridSize];

            if (gridCellPrefab == null )
            {
                Debug.LogError("Error: Grid Cell Prefab on the Game grid is not assigned");
            }

            // Create the grid
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    // Create a new GridSpace object for each cell
                    gameGrid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0.1f, z * gridSpaceSize), Quaternion.identity);
                    gameGrid[x, z].GetComponent<GridCell>().SetPosition(x, z, 1);
                    gameGrid[x, z].transform.parent = transform;
                    gameGrid[x, z].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Z: " + z.ToString() + ")";
                }
            }
        }else{
            Debug.Log("Grid already created");
        }
        
    }

    // Gets the grid position from world position
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int z = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, gridSize);
        z = Mathf.Clamp(z, 0, gridSize);

        return new Vector2Int(x, z);
    }
    
    // Gets the world position of a grid position
    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float z = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, z);
    }
    
    // Returns a selected GridCell
    public GridCell GetGridCellInformation (Vector2Int _gridPos)
    {
        return gameGrid[_gridPos.x, _gridPos.y].gameObject.GetComponent<GridCell>();    //Grid Space ( X: 0 , Z: 0)
    }
    
    // Returns the length of the grid
    public int GetGridLength ()
    {
        return gridSize;
    }
    
    // Returns the width of the grid
    public int GetGridWidth ()
    {
        return gridSize;
    }
    
    // Returns the nearest empty GridCell
    public List<GridCell> GetEmptyNeighbourCell (Vector2Int gridPosition)
    {
        List<GridCell> neighbourList = new List<GridCell>();

        if (gridPosition.x - 1 >= 0){
            // Left
            if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)).isOccupied){
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }
            // Left Down
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
                }
            }
            // Left Up
            if (gridPosition.y + 1 < gridSize) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x + 1 < gridSize){
            // Right
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)).isOccupied){ 
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
                }
            }
            // Right Down
            if (gridPosition.y - 1 >= 0){ 
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)).isOccupied){ 
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
                }
            }
            // Right Up
                if (gridPosition.y + 1 < gridSize) {
                    if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x - 1 >= 0){
            // Down
            if (gridPosition.y - 1 >= 0){
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
                }
            }
            
            // Up
            if (gridPosition.y + 1 < gridSize) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
                }
            }
        }
        return neighbourList;
    }

    public List<GridCell> GetGridCellList(Vector2Int gridPosition, int size)
    {
        List<GridCell> gridCells = new List<GridCell>();
        int offset = (size - 1) / 2;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                Vector2Int currentPosition = gridPosition + new Vector2Int(i, j);
                if (currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < gameGrid.GetLength(0) && currentPosition.y < gameGrid.GetLength(1)){
                    gridCells.Add(GetGridCellInformation(currentPosition));
                }
            }
        }

        return gridCells;
    }

    // public List<GridCell> GetNeighbourCells (Vector2Int gridPosition){

    // }

    // public List<GridCell> GetNeighbourCells (Vector2Int gridPosition, int height){

    // }

    public bool AreNeighboursHigherLevel (Vector2Int gridPosition, float height){
        List<GridCell> gridCells = new List<GridCell>();
        int offset = 1;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                Vector2Int currentPosition = gridPosition + new Vector2Int(i, j);
                if (currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < gameGrid.GetLength(0) && currentPosition.y < gameGrid.GetLength(1)){
                    if (GetGridCellInformation(currentPosition).GetHeightLevel() < height) return true;
                }
            }
        }

        return false;
    }

    public bool AreNeighboursLowerLevel (Vector2Int gridPosition, float height){
        List<GridCell> gridCells = new List<GridCell>();
        int offset = 1;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                Vector2Int currentPosition = gridPosition + new Vector2Int(i, j);
                if (currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < gameGrid.GetLength(0) && currentPosition.y < gameGrid.GetLength(1)){
                    if (GetGridCellInformation(currentPosition).GetHeightLevel() > height) return true;
                }
            }
        }

        return false;
    }
}