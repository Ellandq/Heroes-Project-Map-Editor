using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance;
    [SerializeField] private GameObject terrainPrefab;

    [Header ("Terrain Information")]
    private GameObject terrainObject;
    private TerrainModifier terrainModifier;

    private Vector3 lastModifiedPosition;
    private int terrainSize;

    private void Awake (){
        Instance = this;
    }

    public void SetUpTerrainManager (int size)
    {
        terrainSize = size;
        terrainObject = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrainObject.transform.parent = transform;
        terrainModifier = terrainObject.GetComponent<TerrainModifier>();
        terrainModifier.ChangeTerrainSize(terrainSize);
        lastModifiedPosition = new Vector3(0, 0, 0);
    }
    
    #region Terrain Height Modifcation

    public void ModifyTerrain (Vector3 worldPosition, float radius, int multiplier){
        terrainModifier.ModifyTerrain(worldPosition, radius, multiplier);
    } 

    public void RaiseGridTerrainLevel (List<Vector2Int> gridPosition, float heightLevel, bool allowTallClifs){
        for (int i = gridPosition.Count - 1; i >= 0; i--){
            if ((!allowTallClifs && GameGrid.Instance.AreNeighboursHigherLevel(gridPosition[i], heightLevel - 1))
            || GameGrid.Instance.GetGridCellInformation(gridPosition[i]).IsValidForTerrainEdition()
            || GameGrid.Instance.GetGridCellInformation(gridPosition[i]).GetHeightLevel() == heightLevel)
            {
                gridPosition.RemoveAt(i);
            }           
        }
        if (gridPosition.Count == 0) return; 
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel, 1f);
    }

    public void LowerGridTerrainLevel (List<Vector2Int> gridPosition, float heightLevel, bool allowTallClifs){
        for (int i = gridPosition.Count - 1; i >= 0; i--){
            if ((GameGrid.Instance.GetGridCellInformation(gridPosition[i]).GetHeightLevel() != heightLevel + 1f
            || !allowTallClifs && GameGrid.Instance.AreNeighboursLowerLevel(gridPosition[i], heightLevel + 1))
            || GameGrid.Instance.GetGridCellInformation(gridPosition[i]).IsValidForTerrainEdition())
            {
                gridPosition.RemoveAt(i);
            }     
        }
        if (gridPosition.Count == 0) return; 
        terrainModifier.SetGridCellTerrain(gridPosition, heightLevel, -1f);
    }

    public void CreateSlope (List<Vector2Int> gridPositions){
        if ((gridPositions[0].x == gridPositions[1].x && gridPositions[0].x == gridPositions[2].x) 
            || (gridPositions[0].y == gridPositions[1].y && gridPositions[0].y == gridPositions[2].y)){
            //Check what grid cell is lower by one level (if any)
            if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() + 1 == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel()){
                // Check whether the higher gridCell is already a slope or the middle GridCell is a slope enterance
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel()) 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsValidForTerrainEdition()) {
                    return;
                    }
                // Create the slope
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[2], gridPositions[0]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeSlopeStatus(new List<Vector2Int>{gridPositions[0], gridPositions[2]}, GetSlopeType(gridPositions[2], gridPositions[0]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() - 0.5f);
            }else if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() == GameGrid.Instance.GetGridCellInformation(gridPositions[2]).GetHeightLevel() + 1){
                // Check whether the higher gridCell is already a slope or the middle GridCell is a slope enterance
                if (GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() != Mathf.Floor(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel()) 
                    || GameGrid.Instance.GetGridCellInformation(gridPositions[1]).IsValidForTerrainEdition()) {
                    return;
                    }
                // Create the slope
                terrainModifier.CreateSlope(gridPositions[1], GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f, GetSlopeType(gridPositions[0], gridPositions[2]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeSlopeStatus(new List<Vector2Int>{gridPositions[2], gridPositions[0]}, GetSlopeType(gridPositions[0], gridPositions[2]));
                GameGrid.Instance.GetGridCellInformation(gridPositions[1]).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPositions[0]).GetHeightLevel() - 0.5f);
            }
        }
    }

    // Slope creation for diagonal slopes
    public void CreateSlope (Vector2Int gridPosition_01, Vector2Int gridPosition_02){
        if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetHeightLevel() != GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetHeightLevel() - 0.5f) return;
        if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).IsSlope() ^ GameGrid.Instance.GetGridCellInformation(gridPosition_02).IsSlope()){
            if (((gridPosition_01.x == gridPosition_02.x) && (gridPosition_01.y == gridPosition_02.y - 1))
            ||  ((gridPosition_01.x == gridPosition_02.x) && (gridPosition_01.y == gridPosition_02.y + 1))
            ||  ((gridPosition_01.x == gridPosition_02.x - 1) && (gridPosition_01.y == gridPosition_02.y))
            ||  ((gridPosition_01.x == gridPosition_02.x + 1) && (gridPosition_01.y == gridPosition_02.y))){
                if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).IsSlope()){
                    if (GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType() == SlopeType.None) return;
                    terrainModifier.CreateCornerSlope(gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetHeightLevel(), 
                    GetSlopeType(gridPosition_01, gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_02).ChangeSlopeStatus(gridPosition_01, GetSlopeType(gridPosition_01, gridPosition_02, GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_02).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPosition_01).GetHeightLevel());
                }else{
                    if (GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType() == SlopeType.None) return;
                    terrainModifier.CreateCornerSlope(gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetHeightLevel(), 
                    GetSlopeType(gridPosition_02, gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_01).ChangeSlopeStatus(gridPosition_02, GetSlopeType(gridPosition_02, gridPosition_01, GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetSlopeType()));
                    GameGrid.Instance.GetGridCellInformation(gridPosition_01).ChangeCellLevel(GameGrid.Instance.GetGridCellInformation(gridPosition_02).GetHeightLevel());
                }
            }
        }
    }

    public void RemoveSlope (Vector2Int gridPosition){
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).IsSlopeEnterance()) return;
        GameGrid.Instance.GetGridCellInformation(gridPosition).ChangeSlopeStatus();
        terrainModifier.RemoveSlope(gridPosition, GameGrid.Instance.GetGridCellInformation(gridPosition).GetHeightLevel() - 0.5f);
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

    #endregion

    #region Terrain Type Modification

    public void PaintTerrain (Vector3 worldPosition, float radius, TerrainType terrainType){
        if (worldPosition == lastModifiedPosition) return;

        terrainModifier.PaintTerrain(worldPosition, radius, terrainType);

        lastModifiedPosition = worldPosition;
    }

    #endregion
}
