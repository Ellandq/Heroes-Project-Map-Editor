using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // Static instance of this script
    public static GameGrid Instance;

    [Header ("Grid Information")]
    private GameObject[,] gameGrid;
    private int gridSize;
    private bool gridCreated;

    [Header ("Grid Cell Information")]
    [SerializeField] private GameObject gridCellPrefab;
    public const float gridCellSize = 5f;

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
                    gameGrid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * gridCellSize, 0.1f, z * gridCellSize), Quaternion.identity);
                    gameGrid[x, z].GetComponent<GridCell>().SetPosition(x, z, 1);
                    gameGrid[x, z].transform.parent = transform;
                    gameGrid[x, z].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Z: " + z.ToString() + ")";
                }
            }
        }else{
            Debug.Log("Grid already created");
        }
        
    }

    public void ChangeGridVisability (bool status){
        this.gameObject.SetActive(status);
    }

    // Gets the grid position from world position
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridCellSize);
        int z = Mathf.FloorToInt(worldPosition.z / gridCellSize);

        x = Mathf.Clamp(x, 0, gridSize);
        z = Mathf.Clamp(z, 0, gridSize);

        return new Vector2Int(x, z);
    }
    
    // Gets the world position of a grid position
    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridCellSize;
        float z = gridPos.y * gridCellSize;
        return new Vector3(x, GetGridCellInformation(gridPos).GetHeightLevel() * 2.5f, z);
    }
    
    // Returns a selected GridCell
    public GridCell GetGridCellInformation (Vector2Int _gridPos)
    {
        return gameGrid[_gridPos.x, _gridPos.y].gameObject.GetComponent<GridCell>();    //Grid Space ( X: 0 , Z: 0)
    }
    
    // Returns the size of the grid
    public int GetGridSize ()
    {
        return gridSize;
    }
    
    // Returns the nearest empty GridCell
    public List<GridCell> GetEmptyNeighbourCell (Vector2Int gridPosition)
    {
        List<GridCell> neighbourList = new List<GridCell>();

        if (gridPosition.x - 1 >= 0){
            // Left
            if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)).IsOccupied()){
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }
            // Left Down
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)).IsOccupied()){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
                }
            }
            // Left Up
            if (gridPosition.y + 1 < gridSize) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)).IsOccupied()){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x + 1 < gridSize){
            // Right
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)).IsOccupied()){ 
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
                }
            }
            // Right Down
            if (gridPosition.y - 1 >= 0){ 
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)).IsOccupied()){ 
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
                }
            }
            // Right Up
                if (gridPosition.y + 1 < gridSize) {
                    if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)).IsOccupied()){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x - 1 >= 0){
            // Down
            if (gridPosition.y - 1 >= 0){
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)).IsOccupied()){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
                }
            }
            
            // Up
            if (gridPosition.y + 1 < gridSize) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)).IsOccupied()){
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

    public List<Vector2Int> GetGridCellPositionList(Vector2Int gridPosition, int size)
    {
        List<Vector2Int> gridCells = new List<Vector2Int>();
        int offset = (size - 1) / 2;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                Vector2Int currentPosition = gridPosition + new Vector2Int(i, j);
                if (currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < gameGrid.GetLength(0) && currentPosition.y < gameGrid.GetLength(1)){
                    gridCells.Add(currentPosition);
                }
            }
        }

        return gridCells;
    }

    public bool IsPositionValidForObjectPlacement (Vector2Int gridPosition){
        return true;
    }

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

    public List<List<sbyte>> GetShapeOffset(ObjectShapeType shapeType)
    {
        List<List<sbyte>> shapeOffset = new List<List<sbyte>>();

        switch (shapeType)
        {
            case ObjectShapeType.City:
                for (sbyte i = -5; i < 6; i++)
                {
                    for (sbyte j = -5; j < 6; j++)
                    {
                        if ((Math.Abs(i) + Math.Abs(j)) <= 6 || (Math.Abs(i) == 3 && Math.Abs(j) == 4) || (Math.Abs(i) == 4 && Math.Abs(j) == 3))
                        {
                            shapeOffset.Add(new List<sbyte> { i, j });
                        }
                    }
                }
                break;

            // Add more cases for other shape types

            default:
                // Handle unknown shape types
                break;
        }

        return shapeOffset;
    }

    public void SetGridCellStatus (GameObject occupyingObject, ObjectShapeType shapeType, Vector2Int gridPosition){
        foreach (List<sbyte> position in GetShapeOffset(shapeType)){
            GetGridCellInformation(new Vector2Int(position[0] + gridPosition.x, position[1] + gridPosition.y)).AddOccupyingObject(occupyingObject);  
        }
    }
}