using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObj : MonoBehaviour
{
    public ResourceType resourceType;
    public ObjectType objectType;
    public int count;
    public Vector2Int gridPosition;

    public void ResourceInitialization (Vector2Int _gridPosition)
    {
        count = 500;
        resourceType = ResourceType.Gold;
        objectType = ObjectType.Resource;
        gridPosition = _gridPosition;
    }

    void OnDestroy()
    {
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
    }

    public List<int> GetConvertedResourceInformation ()
    {
        List<int> resourceInfo = new List<int>();

        resourceInfo.Add(gridPosition.x);
        resourceInfo.Add(gridPosition.y);
        resourceInfo.Add((int)resourceType);
        resourceInfo.Add(count);

        return resourceInfo;
    }

    public void ChangeResourceType (ResourceType _resourceType)
    {
        resourceType = _resourceType;
    }
}
