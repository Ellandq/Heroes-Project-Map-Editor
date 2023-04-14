using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectTool : MonoBehaviour
{
    public void ActivateSelectTool(){
        BrushHandler.Instance.ChangeBrushSize(1);
        BrushHandler.Instance.onLeftMouseButtonPressed.AddListener(ObjectSelection);
    }

    private void ObjectSelection (){
        
    }
}
