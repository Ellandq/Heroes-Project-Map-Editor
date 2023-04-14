using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance;
    public UnityEvent onSelectedObjectChange;

    [Header ("Camera referances: ")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera uiCamera;

    [Header("Raycast layers: ")]
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LayerMask uiLayers;
    
    [Header("Object selection information")]
    [SerializeField] public GameObject lastObjectSelected;
    [SerializeField] public bool objectSelected = false;
    [SerializeField] public GameObject selectedObject;
    [SerializeField] public string selectedObjectTag;
    [SerializeField] public Vector2Int selectedGridPosition;

    [Header ("Object references")]
    [SerializeField] private ObjectCreationUI objectCreationUI;

    private void Awake ()
    {
        Instance = this;
    }

    // Checks each frame if the mouse is over an interactible object
    private void Update ()
    {
        // Checks if the mouse is over UI
        if (InputManager.Instance.mouseInput.IsMouseOverUI()) return;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Checks if the mouse is over an object
        if (Physics.Raycast(ray, out hit, 200, layersToHit)){
            selectedObject = hit.transform.gameObject;
            selectedObjectTag = selectedObject.tag;
        }else{
            selectedObject = null;
            selectedObjectTag = null;
        }

        // Checks if the mouse button 0 is pressed
        if (InputManager.Instance.mouseInput.mouseButtonPressed_0)
        {
            // Uses different logic depending on the selected object 
            switch (selectedObjectTag)
            {
                case "Army":
                    ArmySelectionLogic();
                break;

                case "City":
                    CitySelectionLogic();
                    
                break;

                case "CityEnterance":
                    CityEnteranceSelectionLogic();
                break;

                case "Mine":
                    MineSelectionLogic();
                break;

                case "MineEnterance":
                    MineEnteranceSelectionLogic();
                break;

                case "Building":
                    BuildingSelectionLogic();
                break;

                case "BuildingEnterance":
                    BuildingEnteranceSelectionLogic();
                break;

                case "Dwelling":
                    DwellingSelectionLogic();
                break;

                case "DwellingEnterance":
                    DwellingEnteranceSelectionLogic();
                break;

                case "Resource":
                    ResourceSelectionLogic();
                break;

                case "Artifact":
                    ArtifactSelectionLogic();
                break;

                case "GridCell":
                    GridCellSelectionLogic();
                break;

                default:
                    selectedObject = null;
                    selectedObjectTag = null;
                break;
            }
        }  
    }

    public void ObjectInteraction ()
    {
        if (selectedObject != null){
            if (selectedObjectTag == "GridCell"){
                selectedGridPosition = selectedObject.GetComponent<GridCell>().GetPosition();
                objectCreationUI.UpdateElement();

            }
        }
    }
    
    // Adds the selected object
    public void AddSelectedObject (GameObject _selectedObject)
    {
        lastObjectSelected = _selectedObject;
        objectSelected = true;
    }

    // Removes the selected object
    public void RemoveSelectedObject ()
    {
        lastObjectSelected = null;
        objectSelected = false;
        onSelectedObjectChange.Invoke();
    }

    private void ArmySelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void CitySelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void CityEnteranceSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void MineSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void MineEnteranceSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void BuildingSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void BuildingEnteranceSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void DwellingSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void DwellingEnteranceSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void ResourceSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void ArtifactSelectionLogic()
    {
        ObjectInformationDisplay.Instance.ChangeSelectedObject(selectedObject.transform.parent.gameObject);
    }

    private void GridCellSelectionLogic()
    {
        
    }
}