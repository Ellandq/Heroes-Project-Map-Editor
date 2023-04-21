using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldObjectInteractionManager : MonoBehaviour
{
    [Header ("World Object Information")]
    [SerializeField] private GameObject currentObjectHoveredOver;
    [SerializeField] private ObjectType objectType;

    [Header ("Grid Cell Information")]
    [SerializeField] private Vector2Int gridPosition;

    [Header ("Events")]
    public UnityEvent<Vector2Int> onNewGridCellHoveredOver;
    public UnityEvent onNewWorldObjectHoveredOver;

    private void Update (){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GridCell"))){
            ChangeSelectedGridCell(hit.collider.GetComponent<GridCell>().GetPosition());
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("WorldObjects"))){
            
        }
    }

    private void ChangeSelectedObject (GameObject newObject){

    }

    private void ChangeSelectedGridCell (Vector2Int position){
        if (position != gridPosition){
            gridPosition = position;
            onNewGridCellHoveredOver?.Invoke(gridPosition);
        }
    }
}
