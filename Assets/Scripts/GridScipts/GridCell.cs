using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header ("Position information")]
    private Vector3 worldPosition;
    private Vector2Int gridPosition;
    private int posX;
    private int posZ;
    private float heightLevel;

    [Header ("Slope information")]
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isSlopeEnterance;
    [SerializeField] private List<Vector2Int> slopeEnterances;

    [Header ("Material references")]
    [SerializeField] private Material material;

    //Saves a referance to the agame object that gets placed on the cell
    public GameObject objectInThisGridSpace = null;
    
    // Saves if the grid space is occpied or not and what type of object it is
    public bool isOccupied = false;
    public bool isObjectInteractable = false;

    // Set the position of this grid cell on the grid
    public void SetPosition(int x, int z, int y)
    {
        posX = x;
        posZ = z;
        gridPosition = new Vector2Int(x, z);
        worldPosition = transform.position;
        ChangeCellLevel(y);
    }

    #region Getters

    // Get the position of this grid space on the grid
    public Vector2Int GetPosition (){
        return new Vector2Int(posX, posZ);
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
        worldPosition.y = level * 5 + 0.1f;
        transform.position = worldPosition;
        if (isSlope){

        }
    }

    public void ResetSlope (){
        
    }

    public void ChangeSlopeStatus (){
        for (int i = 0; i < slopeEnterances.Count; i++){
            
        }
    }

    public void ChangeSlopeStatus (List<Vector2Int> _slopeEnterances){
        slopeEnterances = new List<Vector2Int>(_slopeEnterances);
    }

    public void ChangeSlopeEnteranceStatus (){

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
