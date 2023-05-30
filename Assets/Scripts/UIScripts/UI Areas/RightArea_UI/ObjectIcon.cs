using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ObjectIcon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header ("General Object Information")]
    [SerializeField] private Sprite objectSprite;
    [SerializeField] private string objectName;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private ObjectShapeType objectShapeType;
    [SerializeField] private List<string> objectFilters;

    [Header ("UI references")]
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private Image objectImage;

    private bool isDragging;

    private void Awake (){
        
        objectImage.sprite = objectSprite;
        nameDisplay.text = objectName;
        isDragging = false;
    }

    // This method is called when the pointer is initially pressed down on the UI object
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        ObjectPlacer.Instance.ActivateObjectPlacement(objectPrefab, objectShapeType);
    }

    // This method is called when the pointer is dragged across the UI object
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            ObjectPlacer.Instance.DeactivateObjectPlacement();
        }
    }

    // Object status
    public void ActivateObject (){
        this.gameObject.SetActive(true);
    }

    public void DeactivateObject (){
        this.gameObject.SetActive(false);
    }

    // Getters
    public GameObject GetObjectPrefab (){
        return objectPrefab;
    }

    public string GetObjectName (){
        return objectName;
    }

    public List<string> GetObjectFilters (){
        return objectFilters;
    }

}
