using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header ("Position information")]
    private Vector3 worldPosition;
    private Vector2Int gridPosition;
    private float heightLevel;

    [Header ("Slope information")]
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isSlopeEnterance;
    [SerializeField] private List<Vector2Int> slopeEntrances;
    [SerializeField] private SlopeType slopeType;

    [Header ("Material references")]
    [SerializeField] private Material material;

    //Saves a referance to the agame object that gets placed on the cell
    private GameObject objectInThisGridSpace = null;
    
    // Saves if the grid space is occpied or not and what type of object it is
    private bool isOccupied = false;
    private bool isObjectInteractable = false;

    // Set the position of this grid cell on the grid
    public void SetPosition(int x, int z, int y)
    {
        gridPosition = new Vector2Int(x, z);
        worldPosition = transform.position;
        ChangeCellLevel(y);
    }

    #region Getters

    public Vector2Int GetPosition (){
        return gridPosition;
    }

    public float GetHeightLevel (){
        return heightLevel;
    }

    public bool IsSlope (){
        return isSlope;
    }

    public bool IsSlopeEnterance (){
        return isSlopeEnterance;
    }

    public bool IsOccupied(){
        return isOccupied;
    }

    public bool IsValidForObjectPlacement (float height){
        if (heightLevel != height) return false; 
        return (!isSlope && !isSlopeEnterance && !isOccupied);
    }

    public bool IsValidForTerrainEdition (){
        return (isSlope || isSlopeEnterance || isOccupied);
    }

    // public bool IsValidForSlopePlacement (float height){

    // }

    public SlopeType GetSlopeType (){
        return slopeType;
    }

    #endregion

    #region Occupying object status

    public void AddOccupyingObject (GameObject other)
    {
        objectInThisGridSpace = other;
        isOccupied = true;
        if (objectInThisGridSpace.tag == "CityEnterance" | objectInThisGridSpace.tag == "Army" 
            | objectInThisGridSpace.tag == "MineEnterance" | objectInThisGridSpace.tag == "Resource"){
            isObjectInteractable = true;
        }
    }

    // Changes the status of this GridCell to an empty one
    public void RemoveOccupyingObject()
    {
        objectInThisGridSpace = null;
        isOccupied = false;
        isObjectInteractable = false;
    }

    public void DestroyOccupyingObject(){
        if (objectInThisGridSpace != null){
            Destroy(objectInThisGridSpace);
        }
        RemoveOccupyingObject();
    }

    #endregion

    public void ChangeCellLevel (float level){
        heightLevel = level;
        worldPosition.y = level * 2.5f + 0.1f;
        transform.position = worldPosition;
    }

    public void ChangeSlopeStatus (){
        isSlope = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        slopeType = SlopeType.None;
        for (int i = 0; i < slopeEntrances.Count; i++){
            GameGrid.Instance.GetGridCellInformation(slopeEntrances[i]).ChangeSlopeEntranceStatus(false);
        }
    }

    
    public void ChangeSlopeStatus(List<Vector2Int> _slopeEntrances, SlopeType _slopeType) {
        slopeEntrances = new List<Vector2Int>(_slopeEntrances);
        isSlope = true;
        slopeType = _slopeType;
        for (int i = 0; i < slopeEntrances.Count; i++) {
            GameGrid.Instance.GetGridCellInformation(slopeEntrances[i]).ChangeSlopeEntranceStatus(true);
        }

        switch (slopeType) {
            case SlopeType.LeftToRight:
                transform.rotation = Quaternion.Euler(0f, 0f, 25f);
                break;
            case SlopeType.RightToLeft:
                transform.rotation = Quaternion.Euler(0f, 0f, -25f);
                break;
            case SlopeType.BottomToTop:
                transform.rotation = Quaternion.Euler(-25f, 0f, 0f);
                break;
            case SlopeType.TopToBottom:
                transform.rotation = Quaternion.Euler(25f, 0f, 0f);
                break;
            default:
                break;
        }
    }

    public void ChangeSlopeStatus(Vector2Int _slopeEntrance, SlopeType _slopeType) {
        slopeEntrances = new List<Vector2Int>(){_slopeEntrance};
        isSlope = true;
        slopeType = _slopeType;
        GameGrid.Instance.GetGridCellInformation(slopeEntrances[0]).ChangeSlopeEntranceStatus(true);
        

        switch (slopeType) {
            case SlopeType.BottomLeftToTopRight:
                transform.rotation = Quaternion.Euler(-15f, 0f, 15f);
                break;
            case SlopeType.BottomRightToTopLeft:
                transform.rotation = Quaternion.Euler(-15f, 0f, -15f);
                break;
            case SlopeType.TopLeftToBottomRight:
                transform.rotation = Quaternion.Euler(15f, 0f, 15f);
                break;
            case SlopeType.TopRightToBottomLeft:
                transform.rotation = Quaternion.Euler(15f, 0f, -15f);
                break;
            default:
                break;
        }
    }

    public void ChangeSlopeEntranceStatus (bool status){
        isSlopeEnterance = status;
    }

    public void ChangeCellHighlight(Color color){
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer != null) {
            Material material = renderer.material;
            if (material != null) {
                material.color = color;
            }
        }
    }
}
