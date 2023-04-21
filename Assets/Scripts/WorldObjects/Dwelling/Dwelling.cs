using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : WorldObject
{
    public Dwelling (Vector2Int gridPosition, Vector3 rotation, PlayerTag ownedByPlayer = PlayerTag.None, ObjectType objectType = ObjectType.Dwelling)
        : base(gridPosition, rotation, objectType, ownedByPlayer)
    {
        // Initialization
    }
}
