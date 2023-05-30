using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldObject
{
    public void Initialize (Vector2Int gridPosition, Vector3 rotation, PlayerTag ownedByPlayer = PlayerTag.None, ObjectType objectType = ObjectType.Building)
    {
        base.Initialize(gridPosition, rotation, objectType, ownedByPlayer);
        // Initialization
    }
    
}
