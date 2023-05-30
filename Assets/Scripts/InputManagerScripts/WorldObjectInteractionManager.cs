using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldObjectInteractionManager : MonoBehaviour
{
    [Header ("World Object Information")]
    [SerializeField] private WorldObject currentObjectHoveredOver;
    [SerializeField] private ObjectType objectType;

    [Header ("Grid Cell Information")]
    [SerializeField] private Vector2Int gridPosition;

    [Header ("Terrain Information")]
    [SerializeField] private Vector3 worldPosition;

    [Header ("Events")]
    public UnityEvent<Vector2Int> onNewGridCellHoveredOver;
    public UnityEvent<WorldObject> onNewWorldObjectHoveredOver;
    public UnityEvent<Vector3> onTerrainHit;

    private void Update (){
        if (InputManager.Instance.mouseInput.IsMouseOverUI()) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GridCell"))){
            ChangeSelectedGridCell(hit.collider.GetComponent<GridCell>().GetPosition());
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("WorldObjects"))){
            ChangeSelectedObject(hit.collider.GetComponent<WorldObject>());
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"))){
            ChangeWorldPosition(hit.point);
        }else{
            if (currentObjectHoveredOver != null) RemoveSelectedObject();
        }
    }

    private void ChangeSelectedObject (WorldObject newObject){
        if (newObject == currentObjectHoveredOver) return;
        currentObjectHoveredOver = newObject;
        onNewWorldObjectHoveredOver?.Invoke(currentObjectHoveredOver);
    }

    private void RemoveSelectedObject (){
        currentObjectHoveredOver = null;
    }

    private void ChangeSelectedGridCell (Vector2Int position){
        if (position == gridPosition) return;
        gridPosition = position;
        onNewGridCellHoveredOver?.Invoke(gridPosition);
    }

    private void ChangeWorldPosition (Vector3 position){
        if (position == worldPosition) return;
        worldPosition = position;
        onTerrainHit?.Invoke(worldPosition);
    }

    // Getters

    public WorldObject GetSelectedWorldObject (){
        return currentObjectHoveredOver;
    }

    public Vector2Int GetSelectedGridCellPosition (){
        return gridPosition;
    }
}
