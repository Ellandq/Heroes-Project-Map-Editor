using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;
    internal int posX;
    internal int posZ;
    private string currentColidingObject;
    private Vector3 currentColidingObjectCoordinates;

    //Saves a referance to the agame object that gets placed on the cell
    public GameObject objectInThisGridSpace = null;
    
    // Saves if the grid space is occpied or not and what type of object it is
    public bool isOccupied = false;
    public bool isObjectInteractable = false;

    // Set the position of this grid cell on the grid
    public void SetPosition(int x, int z)
    {
        posX = x;
        posZ = z;
        gridPosition = new Vector2Int(x, z);
    }
    // Get the position of this grid space on the grid
    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posZ);
    }
    public void AddOccupyingObject ()
    {
        
    }
    
    // Changes the status of this GridCell to an empty one
    public void RemoveOccupyingObject()
    {
        objectInThisGridSpace = null;
        isOccupied = false;
        isObjectInteractable = false;
    }
}
