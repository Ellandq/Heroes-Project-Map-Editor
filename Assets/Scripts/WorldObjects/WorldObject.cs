using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] public GameObject flag;

    [Header ("Object Information")]
    [SerializeField] private ObjectType objectType;
    [SerializeField] private PlayerTag ownedByPlayer;
    [SerializeField] private Vector2Int gridPosition;
    [SerializeField] private Vector3 rotation;


    public WorldObject (Vector2Int gridPosition, Vector3 rotation, ObjectType objectType, PlayerTag ownedByPlayer = PlayerTag.None){
        this.objectType = objectType;
        UpdateObjectPosition(GameGrid.Instance.GetWorldPosFromGridPos(gridPosition));
        UpdateObjectRotation(rotation);
        ChangeOwningPlayer(ownedByPlayer);
    }

    // Variable Updates

    public virtual void ChangeOwningPlayer (PlayerTag ownedByPlayer = PlayerTag.None){
        this.ownedByPlayer = ownedByPlayer;
    }

    public virtual void UpdateGridPosition (Vector2Int gridPosition){
        this.gridPosition = gridPosition;
    }

    public virtual void UpdateObjectPosition (Vector3 position){
        transform.position = position;
        UpdateGridPosition(GameGrid.Instance.GetGridPosFromWorld(position));
    }

    public virtual void UpdateObjectRotation (Vector3 rotation){
        transform.localEulerAngles = rotation;
    }

    public virtual void UpdateObjectRotation (float rotation){
        transform.localEulerAngles = new Vector3 (0f, rotation, 0f);
    }

    // Getters
    public ObjectType GetObjectType (){
        return objectType;
    }

    public Vector2Int GetGridPosition (){
        return gridPosition;
    }

    public float GetRotation (){
        return rotation.y;
    }

    public PlayerTag GetPlayerTag (){
        return ownedByPlayer;
    }

    public virtual List<UnitSlot> GetUnitSlots (){
        Debug.LogError("Invalid request of: List<UnitSlot> from: " + this.gameObject.name);
        return null;
    }

    public virtual List<int> GetConvertedObjectInformation (){
        return new List<int>();
    }

    // Object Interactions

    public virtual void InteractWithObject (){
        Debug.Log("Interacting with object: " + this.gameObject.name);
    }

    public virtual void InteractWithObject (WorldObject other){
        Debug.Log("Interacting with object: " + this.gameObject.name + " , Interaction started by: " + other.gameObject.name);
    }

    // On Destroy

    protected virtual void OnDestroy (){
        Debug.Log("Object destroyed (" + this.gameObject.name + ")");
    }
}
