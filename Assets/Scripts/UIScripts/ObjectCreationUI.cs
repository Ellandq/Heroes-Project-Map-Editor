using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectCreationUI : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    [SerializeField] private TMP_Dropdown dropDownMenu;

    public void ChangeObjectType ()
    {
        objectType = (ObjectType)Convert.ToInt32(dropDownMenu.value);
    }

    private void EnableElement ()
    {
        dropDownMenu.value = 0;
        objectType = (ObjectType)Convert.ToInt32(dropDownMenu.value);
        this.gameObject.SetActive(true);
    }

    private void DisableElement ()
    {
        this.gameObject.SetActive(false);
    }

    public void UpdateElement ()
    {
        EnableElement();
    }

    public void ConfirmObjectCreation ()
    {
        WorldObjectManager.Instance.CreateObject(objectType, ObjectSelector.Instance.selectedGridPosition);
        DisableElement();
    }
}


