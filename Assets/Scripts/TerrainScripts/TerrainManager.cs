using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance;
    [SerializeField] private GameObject terrainPrefab;
    
    private List<GridCell> selectedGridCells;

    [Header ("Terrain Information")]
    [SerializeField] private GameObject terrainObject;
    [SerializeField] private TerrainModifier terrainModifier;

    private Vector2Int terrainSize;

    private void Awake (){
        Instance = this;
    }

    public void SetUpTerrainManager (Vector2Int size)
    {
        terrainSize = size;
        terrainObject = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrainObject.transform.parent = transform;
        terrainModifier = terrainObject.GetComponent<TerrainModifier>();
        terrainModifier.ChangeTerrainSize(terrainSize);
    }
    
    public void RaiseGridTerrainLevel (Vector2Int gridPosition, float heightLevel){
        if (selectedGridCells.Contains(GameGrid.Instance.GetGridCellInformation(gridPosition)) || GameGrid.Instance.AreNeighboursHigherLevel(gridPosition, heightLevel - 1)) return;
        selectedGridCells.Add(GameGrid.Instance.GetGridCellInformation(gridPosition));
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeCellLevel(heightLevel);
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel);
    }

    public void LowerGridTerrainLevel (Vector2Int gridPosition, float heightLevel){
        if (selectedGridCells.Contains(GameGrid.Instance.GetGridCellInformation(gridPosition)) || GameGrid.Instance.AreNeighboursLowerLevel(gridPosition, heightLevel + 1)) return;
        selectedGridCells.Add(GameGrid.Instance.GetGridCellInformation(gridPosition));
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeCellLevel(heightLevel);
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel);
    }

    public void CreateSlope (List<Vector2Int> gridPositions){
        if ((gridPositions[0].x == gridPositions[1].x && gridPositions[0].x == gridPositions[2].x) || (gridPositions[0].y == gridPositions[1].y && gridPositions[0].y == gridPositions[2].y)){
            //Check what grid cell is lower by one level (if any)
            if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() + 1 == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel()){
                // Check whether the higher gridCell is already a slope
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel())) return;
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[2], gridPositions[0]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f);
            }else if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() + 1){
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel())) return;
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[0], gridPositions[2]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f);
            }
        }
    }

    public void ResetSelectedGridCellList (){
        if (selectedGridCells == null || selectedGridCells.Count != 0){
            selectedGridCells = new List<GridCell>();
        }
    }

    public SlopeType GetSlopeType(Vector2Int higherPosition, Vector2Int lowerPosition)
    {
        SlopeType slopeType;
        
        // Determine the direction of the slope
        if (higherPosition.x == lowerPosition.x)
        {
            if (higherPosition.y < lowerPosition.y)
            {
                slopeType = SlopeType.BottomToTop;
            }
            else
            {
                slopeType = SlopeType.TopToBottom;
            }
        }
        else
        {
            if (higherPosition.x < lowerPosition.x)
            {
                slopeType = SlopeType.LeftToRight;
            }
            else
            {
                slopeType = SlopeType.RightToLeft;
            }
        }
        
        return slopeType;
    }

}
