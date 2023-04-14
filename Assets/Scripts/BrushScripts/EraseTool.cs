using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EraseTool : MonoBehaviour
{   
    [Header ("Brush Information")]
    [SerializeField] private int brushSize;

    [Header ("UI References")]
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private TMP_Text brushSizeDisplay;

    public void ActivateSelectTool(){
        BrushHandler.Instance.onLeftMouseButtonPressed.AddListener(EraseObjects);
    }

    public void UpdateBrushSize (float size){
        brushSize = Convert.ToInt32(size) * 2 - 1;
        BrushHandler.Instance.ChangeBrushSize(brushSize);
        brushSizeDisplay.text = Convert.ToString(brushSize);
    }

    public void UpdateBrushSize (){
        BrushHandler.Instance.ChangeBrushSize(brushSize);
    }

    private void EraseObjects(){
        foreach (GridCell cell in BrushHandler.Instance.GetCurrentSelectedGridCells()){
            cell.DestroyOccupyingObject();
        }
    }
}
