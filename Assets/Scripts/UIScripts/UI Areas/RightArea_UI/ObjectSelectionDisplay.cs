using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionDisplay : MonoBehaviour
{
    public static ObjectSelectionDisplay Instance;

    [Header ("UI References")]
    [SerializeField] private GameObject scrollRectContent;
    
    private List<ObjectIcon> displayedObjects;

    [Header ("Search filters")]
    [SerializeField] private List<string> objectTags;
    [SerializeField] private List<string> currentSelectedTags;

    // Awake function to run at the start
    private void Awake (){
        objectTags = new List<string>()
        {"Placeholder", "City", "Army", "Unit", "Hero", "Dwelling", "Mine", 
        "Building", "Resource", "Artifact", "Miscelanous", "Bazaar", 
        "Coalition", "DarkOnes", "Magic", "Hive", "Temple"};
        UpdateObjectSelection();
    }

    // Reset the object icon list
    private void UpdateObjectSelection (){
        displayedObjects = new List<ObjectIcon>();
        foreach (Transform child in scrollRectContent.transform){
            displayedObjects.Add(child.GetComponent<ObjectIcon>());
        }
    }

    // Activate search filter for user given type
    public void ChangeSearchFilter(string phrase)
    {
        if (string.IsNullOrEmpty(phrase))
        {
            currentSelectedTags = new List<string>(objectTags);
            UpdateDisplayedIcons();
            return;
        }

        string[] phrases = phrase.ToLower().Split(' ');
        currentSelectedTags = new List<string>();

        foreach (string str in phrases)
        {
            foreach (string tag in objectTags)
            {
                if (tag.ToLower().Contains(str) && !currentSelectedTags.Contains(tag))
                {
                    currentSelectedTags.Add(tag);
                }
            }
        }

        UpdateDisplayedIcons();
    }

    // Activate search filter for set type
    public void ChangeSearchFilterSingle (string phrase){
        currentSelectedTags = new List<string>(){ phrase };
        UpdateDisplayedIcons();
    }

    private void UpdateDisplayedIcons (){
        foreach (ObjectIcon icon in displayedObjects){
            List<string> tags = icon.GetObjectFilters();
            bool objectAdded = false;

            foreach (string tag in currentSelectedTags){
                if (tags.Contains(tag)){
                    objectAdded = true;
                    break;
                }
            }
            if (objectAdded) icon.ActivateObject();
            else icon.DeactivateObject();
        }
    }
}
