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

    private int terrainSize;

    private void Awake (){
        Instance = this;
        selectedGridCells = new List<GridCell>();
    }

    public void SetUpTerrainManager (int size)
    {
        terrainSize = size;
        terrainObject = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrainObject.transform.parent = transform;
        terrainModifier = terrainObject.GetComponent<TerrainModifier>();
        terrainModifier.ChangeTerrainSize(terrainSize);
    }
    
    public void RaiseGridTerrainLevel (Vector2Int gridPosition, float heightLevel, bool allowTallClifs){
        if (selectedGridCells.Contains(GameGrid.Instance.GetGridCellInformation(gridPosition))
            || (!allowTallClifs && GameGrid.Instance.AreNeighboursHigherLevel(gridPosition, heightLevel - 1))
            || GameGrid.Instance.GetGridCellInformation(gridPosition).IsSlopeEnterance()
            || GameGrid.Instance.GetGridCellInformation(gridPosition).IsSlope())
        {
            return;
        }

        // raise the terrain on given coordinates and update the corelated grid cell
        selectedGridCells.Add(GameGrid.Instance.GetGridCellInformation(gridPosition));
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeCellLevel(heightLevel);
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel);
    }

    public void LowerGridTerrainLevel (Vector2Int gridPosition, float heightLevel, bool allowTallClifs){
        if (selectedGridCells.Contains(GameGrid.Instance.GetGridCellInformation(gridPosition))
            || (!allowTallClifs && GameGrid.Instance.AreNeighboursLowerLevel(gridPosition, heightLevel + 1))
            || GameGrid.Instance.GetGridCellInformation(gridPosition).IsSlopeEnterance()
            || GameGrid.Instance.GetGridCellInformation(gridPosition).IsSlope())
        {
            return;
        }
        // lower the terrain on given coordinates and update the corelated grid cell
        selectedGridCells.Add(GameGrid.Instance.GetGridCellInformation(gridPosition));
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeCellLevel(heightLevel);
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel);
    }

    public void CreateSlope (List<Vector2Int> gridPositions){
        if ((gridPositions[0].x == gridPositions[1].x && gridPositions[0].x == gridPositions[2].x) 
            || (gridPositions[0].y == gridPositions[1].y && gridPositions[0].y == gridPositions[2].y)){
            //Check what grid cell is lower by one level (if any)
            if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() + 1 == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel()){
                // Check whether the higher gridCell is already a slope or the middle GridCell is a slope enterance
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel()) 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsSlopeEnterance() 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsSlope()) {
                    return;
                    }
                // Create the slope
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[2], gridPositions[0]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeSlopeStatus(new List<Vector2Int>{gridPositions[0], gridPositions[2]}, GetSlopeType(gridPositions[2], gridPositions[0]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f);
            }else if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() + 1){
                // Check whether the higher gridCell is already a slope or the middle GridCell is a slope enterance
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel()) 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsSlopeEnterance() 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsSlope()) {
                    return;
                    }
                // Create the slope
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[0], gridPositions[2]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeSlopeStatus(new List<Vector2Int>{gridPositions[2], gridPositions[0]}, GetSlopeType(gridPositions[0], gridPositions[2]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f);
            }
        }
    }

    public void CreateSlope (Vector2Int gridPosition_01, Vector2Int gridPosition_02){
        if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).IsSlope() ^ GameGrid.Instance.GetGridCellInformation(gridPosition_02).IsSlope()){
            if (((gridPosition_01.x == gridPosition_02.x) && (gridPosition_01.y == gridPosition_02.y - 1))
            ||  ((gridPosition_01.x == gridPosition_02.x) && (gridPosition_01.y == gridPosition_02.y + 1))
            ||  ((gridPosition_01.x == gridPosition_02.x - 1) && (gridPosition_01.y == gridPosition_02.y))
            ||  ((gridPosition_01.x == gridPosition_02.x + 1) && (gridPosition_01.y == gridPosition_02.y))){
                if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).IsSlope()){
                    //if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType() == SlopeType.None) return;
                    terrainModifier.CreateCornerSlope(gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetHeightLevel(), 
                    GetSlopeType(gridPosition_01, gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_02).ChangeSlopeStatus(gridPosition_01, GetSlopeType(gridPosition_01, gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_02).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetHeightLevel());
                }else{
                    //if (GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType() == SlopeType.None) return;
                    terrainModifier.CreateCornerSlope(gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetHeightLevel(), 
                    GetSlopeType(gridPosition_02, gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_01).ChangeSlopeStatus(gridPosition_02, GetSlopeType(gridPosition_02, gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_01).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetHeightLevel());
                }
            }
        }
    }

    public void RemoveSlope (Vector2Int gridPosition){
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeSlopeStatus();
        LowerGridTerrainLevel(gridPosition, GameGrid.Instance.GetGridCellInformation(gridPosition).GetHeightLevel() + 0.5f, false);
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
        if (higherPosition.x == lowerPosition.x){
            if (higherPosition.y < lowerPosition.y){
                slopeType = SlopeType.TopToBottom;    
            }else{
                slopeType = SlopeType.BottomToTop;
            }
        }else{
            if (higherPosition.x < lowerPosition.x){
                slopeType = SlopeType.RightToLeft;
            }else{
                slopeType = SlopeType.LeftToRight;
            }
        }
        
        return slopeType;
    }

    public SlopeType GetSlopeType(Vector2Int slopePosition, Vector2Int targetPosition, SlopeType connectedSlope)
    {
        SlopeType slopeType;
        
        // Determine the direction of the slope
        if (slopePosition.x == targetPosition.x){
            if (slopePosition.y < targetPosition.y){
                if (connectedSlope == SlopeType.RightToLeft){
                    slopeType = SlopeType.TopRightToBottomLeft;
                }else if (connectedSlope == SlopeType.LeftToRight){
                    slopeType = SlopeType.TopLeftToBottomRight;
                }else{
                    slopeType = SlopeType.None;
                }  
            }else{
                if (connectedSlope == SlopeType.RightToLeft){
                    slopeType = SlopeType.BottomRightToTopLeft;
                }else if (connectedSlope == SlopeType.LeftToRight){
                    slopeType = SlopeType.BottomLeftToTopRight;
                }else{
                    slopeType = SlopeType.None;
                } 
            }
        }else{
            if (slopePosition.x < targetPosition.x){
                if (connectedSlope == SlopeType.TopToBottom){
                    slopeType = SlopeType.TopRightToBottomLeft;
                }else if (connectedSlope == SlopeType.BottomToTop){
                    slopeType = SlopeType.BottomRightToTopLeft;
                }else{
                    slopeType = SlopeType.None;
                } 
            }else{
                if (connectedSlope == SlopeType.TopToBottom){
                    slopeType = SlopeType.TopLeftToBottomRight;
                }else if (connectedSlope == SlopeType.BottomToTop){
                    slopeType = SlopeType.BottomLeftToTopRight;
                }else{
                    slopeType = SlopeType.None;
                } 
            }
        }
        
        return slopeType;
    }

}
