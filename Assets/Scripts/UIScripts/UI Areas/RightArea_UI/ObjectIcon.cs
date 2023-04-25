using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectIcon : MonoBehaviour
{
    [Header ("General Object Information")]
    [SerializeField] private ObjectType objectType;
    [SerializeField] private Sprite objectSprite;
    [SerializeField] private string objectName;
    [SerializeField] private GameObject objectPrefab;

    [Header ("UI references")]
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private Image objectImage;

    private void Awake (){
        objectImage.sprite = objectSprite;
        nameDisplay.text = objectName;
    }

    public GameObject GetObjectPrefab (){
        return objectPrefab;
    }

}
