using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool walkingRestricted = false;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool walkingRestricted)
    {
        this.coordinates = coordinates;
        this.walkingRestricted = walkingRestricted;
    }
}
