using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public static ObjectPlacer Instance;
    private GameObject createdObject;
    private GameObject selectedObjectPrefab;
    private ObjectShapeType objectShapeType;
    private Vector2Int currentGridPosition;

    private bool objectPositionValid;

    private void Awake (){
        Instance = this;
        objectPositionValid = false;
        createdObject = null;
        selectedObjectPrefab = null;
        currentGridPosition = new Vector2Int(0, 0);

        InputManager.Instance.worldObjectInteractionManager.onNewGridCellHoveredOver.AddListener(UpdateSelectedObjectPosition);
    }

    public void ActivateObjectPlacement (GameObject selectedObjectPrefab, ObjectShapeType objectShapeType){
        this.objectShapeType = objectShapeType;
        this.selectedObjectPrefab = selectedObjectPrefab;
        UIManager.Instance.ChangeTool((int)BrushMode.SelectTool);
        PlaceObject();
    }

    public void DeactivateObjectPlacement (){
        if (objectPositionValid && selectedObjectPrefab != null && !InputManager.Instance.mouseInput.IsMouseOverUI()){
            WorldObjectManager.Instance.CreateObject(selectedObjectPrefab, currentGridPosition);
        }
        selectedObjectPrefab = null;
        RemoveObject();
    }

    private void PlaceObject (){
        if (selectedObjectPrefab != null){
            createdObject = Instantiate(selectedObjectPrefab, transform.position + new Vector3(50f, -25f, 50f), Quaternion.identity);
            createdObject.transform.parent = transform;
        }else{
            DeactivateObjectPlacement();
            Debug.LogError("No prefab selected");
        }
    }

    private void UpdateSelectedObjectPosition (Vector2Int gridPosition){
        if (createdObject == null || gridPosition == currentGridPosition) return;
        currentGridPosition = gridPosition;
        CheckSelectedPosition(currentGridPosition);
        createdObject.transform.position = GameGrid.Instance.GetWorldPosFromGridPos(currentGridPosition);
        // logic for changing color
        if (objectPositionValid)
        {
            createdObject.GetComponentInChildren<Renderer>().material.color = Color.blue; // Set the valid color
        }
        else
        {
            createdObject.GetComponentInChildren<Renderer>().material.color = Color.red; // Set the invalid color
        }
    }

    private void CheckSelectedPosition (Vector2Int gridPosition){
        GridCell origin = GameGrid.Instance.GetGridCellInformation(gridPosition);
        float originHeight = origin.GetHeightLevel();
        if (!origin.IsValidForObjectPlacement(originHeight)){
            objectPositionValid = false;
            return;
        }
        float height = origin.GetHeightLevel();
        foreach (List<sbyte> position in GameGrid.Instance.GetShapeOffset(objectShapeType)){
            try{
                if (!GameGrid.Instance.GetGridCellInformation(new Vector2Int(position[0] + gridPosition.x, position[1] + gridPosition.y)).IsValidForObjectPlacement(originHeight)){
                objectPositionValid = false;
                return;
                }
            }catch (IndexOutOfRangeException){
                objectPositionValid = false;
                return;
            }
            
        }
        objectPositionValid = true;
    }

    private void RemoveObject (){
        if (createdObject == null) return;
        Destroy(createdObject);
    }
}
