using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectUnitsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject unitsDisplayID;    
    [SerializeField] private GameObject unitsDisplayCount;  

    [SerializeField] public List<TMP_Text> unitIdDisplay;
    [SerializeField] public List<TMP_InputField> unitCountDisplay;

    private void Awake ()
    {
        GetUnitIDs();
        GetUnitCounts();
    }

    private void GetUnitIDs()
    {
        for (int i = 0; i < 7; i ++){
            unitIdDisplay.Add(unitsDisplayID.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>());
        }
    }
    
    private void GetUnitCounts()
    {
        for (int i = 0; i < 7; i ++){
            unitCountDisplay.Add(unitsDisplayCount.transform.GetChild(i).GetComponent<TMP_InputField>());
        }
    }

    public void UpdateUnitDisplay (List<int> id, List<int> count)
    {
        for (int i = 0; i < 7; i++){
            if (id[i] <= Enum.GetValues(typeof(UnitName)).Cast<int>().Max()){
                unitIdDisplay[i].text = Enum.GetName(typeof(UnitName),  id[i]);
                if (id[i] != 0){
                    unitCountDisplay[i].text = Convert.ToString(count[i]);
                    unitCountDisplay[i].interactable = true;
                }else{
                    unitCountDisplay[i].text = "0";
                    unitCountDisplay[i].interactable = false;
                }
            }else{
                unitIdDisplay[i].text = Enum.GetName(typeof(HeroTag),  id[i] - Enum.GetValues(typeof(UnitName)).Cast<int>().Max());
                unitCountDisplay[i].text = "1";
                unitCountDisplay[i].interactable = false;
            }
            
            
        }
    }

    public void UpdateUnit (int unitId, UnitName unitName)
    {
        unitIdDisplay[unitId].text = Enum.GetName(typeof(UnitName), unitName);
        if (unitName != UnitName.Empty){
            unitCountDisplay[unitId].interactable = true;
        }else{
            unitCountDisplay[unitId].text = "0";
            unitCountDisplay[unitId].interactable = false;
        }
    }

    public void UpdateUnit (int unitId, HeroTag heroTag)
    {
        unitIdDisplay[unitId].text = Enum.GetName(typeof(HeroTag), heroTag);
        if (heroTag != HeroTag.Empty){
            unitCountDisplay[unitId].text = "1";
            unitCountDisplay[unitId].interactable = false;
        }else{
            unitCountDisplay[unitId].text = "0";
            unitCountDisplay[unitId].interactable = false;
        }
    }
}
