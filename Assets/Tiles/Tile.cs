using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] bool placementRestricted = false;
    public bool PlacementRestricted {get {return placementRestricted;} }

    GridManager gridManager;
    Pathfinder pathfinder;
    MeshRenderer tileMeshRenderer;
    Vector2Int coordinates = new Vector2Int();
    
    void Awake() 
    {
        tileMeshRenderer = GetComponentInChildren<MeshRenderer>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(this.transform.position);

            if (placementRestricted)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseOver()
    {
        if (!placementRestricted)
        {
            setTileColour(1f, 3f, 1f); //sets tile colour to green
        }
        else
        {
            setTileColour(3f, 1f, 1f); //sets tile colour to red
        }
    }

    void OnMouseExit() 
    {
        setTileColour(1f,1f,1f); //resets tile colour to original colour
    }

    void OnMouseDown() 
    {
        if (!gridManager.Grid.ContainsKey(coordinates)) { return; }
        if (!gridManager.GetNodeFromDictionary(coordinates).walkingRestricted && !pathfinder.WillBlockPath(coordinates))
        {
            bool successfullyPlaced = tower.CreateTower(tower, transform.position); // create the tower prefab
            
            if (successfullyPlaced)
            {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
            }
        }
        else
        {
            Debug.Log("You cannot build here.");
        }
    }

    void setTileColour(float red, float green, float blue)
    {
        Color tileColour = new Color(red,green,blue);
        tileMeshRenderer.material.color = tileColour;
    }
}
