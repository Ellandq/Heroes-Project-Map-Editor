using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectUnitChoiceDisplay : MonoBehaviour
{
    [Header ("UI elements")]
    [SerializeField] private ObjectUnitsDisplay objectUnitsDisplay;
    [SerializeField] private GameObject unitFractionChoiceDisplay;
    [SerializeField] private List<Button> unitFractionChoiceButtons;
    [SerializeField] private GameObject unitTierChoiceDisplay;
    [SerializeField] private List<Button> unitTierChoiceButtons;
    [SerializeField] private List<Button> unitDisplayButtons;

    [Header ("Current Unit")]
    [SerializeField] private int currentButtonId;
    [SerializeField] private UnitChoiceStatus unitChoiceStatus;
    [SerializeField] private CityFraction unitFraction;

    private void ChangeUnitChoiceMenuState (UnitChoiceStatus _unitChoiceStatus)
    {
        unitChoiceStatus = _unitChoiceStatus;
        switch (unitChoiceStatus){
            case UnitChoiceStatus.MenuClosed:
                unitFractionChoiceDisplay.SetActive(false);
                unitTierChoiceDisplay.SetActive(false);
            break;

            case UnitChoiceStatus.FractionMenuOpen:
                unitFractionChoiceDisplay.SetActive(true);
                unitTierChoiceDisplay.SetActive(false);
            break;

            case UnitChoiceStatus.UnitTierFractionOpen:
                unitTierChoiceDisplay.SetActive(true);
            break;
        }

    }

    public void OpenUnitFractionMenu (int buttonId)
    {
        currentButtonId = buttonId;
        ChangeUnitChoiceMenuState(UnitChoiceStatus.FractionMenuOpen);
    }

    public void OpenUnitTierMenu (int _unitFraction)
    {
        unitFraction = (CityFraction)_unitFraction;
        ChangeUnitChoiceMenuState(UnitChoiceStatus.UnitTierFractionOpen);
    }

    public void FinalizeUnit (int unitTier)
    {
        if (unitTier == 0){
            objectUnitsDisplay.UpdateUnit(currentButtonId, UnitName.Empty);
        }else if (unitTier < 0){
            HeroTag heroTag = (HeroTag)(6 *((int)unitFraction - 1) - unitTier);
            objectUnitsDisplay.UpdateUnit(currentButtonId, heroTag);
        }else{
            UnitName unitType = (UnitName)(8 * ((int)unitFraction - 1) + unitTier); 
            objectUnitsDisplay.UpdateUnit(currentButtonId, unitType);
        }
        ChangeUnitChoiceMenuState(UnitChoiceStatus.MenuClosed);
    }

    private enum UnitChoiceStatus{
        MenuClosed, FractionMenuOpen, UnitTierFractionOpen
    }
}

