using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObject : WorldObject
{
    private ResourceType resourceType;
    private int resourceCount;

    public void Initialize (Vector2Int gridPosition, Vector3 rotation, PlayerTag ownedByPlayer = PlayerTag.None, 
    ResourceType resourceType = ResourceType.Gold, ObjectType objectType = ObjectType.Resource)
    {
        base.Initialize(gridPosition, rotation, objectType, ownedByPlayer);
        ChangeResourceType(resourceType);
    }

    public void ChangeResourceType (ResourceType resourceType)
    {
        this.resourceType = resourceType;
    }

    public override List<int> GetConvertedObjectInformation ()
    {
        List<int> resourceInfo = new List<int>();

        resourceInfo.Add(GetGridPosition().x);
        resourceInfo.Add(GetGridPosition().y);
        resourceInfo.Add((int)resourceType);
        resourceInfo.Add(resourceCount);

        return resourceInfo;
    }

    protected override void OnDestroy()
    {
        GameGrid.Instance.GetGridCellInformation(GetGridPosition()).RemoveOccupyingObject();
    }
}
