using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private GameObject terrainPrefab;

    [Header ("Terrain Information")]
    [SerializeField] private GameObject terrainObject;
    [SerializeField] private TerrainModifier terrainModifier;

    private Vector2Int terrainSize;

    public void SetUpTerrainManager (Vector2Int size)
    {
        terrainSize = size;
        terrainObject = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrainObject.transform.parent = transform;
        terrainModifier = terrainObject.GetComponent<TerrainModifier>();
        terrainModifier.ChangeSize(terrainSize);
    }
}
