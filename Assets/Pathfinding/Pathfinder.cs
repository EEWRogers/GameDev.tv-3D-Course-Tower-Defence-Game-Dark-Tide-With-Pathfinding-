using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int endCoordinates;
    public Vector2Int EndCoordinates { get { return endCoordinates; } }
    Node startNode;
    Node endNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Queue<Node> nodesToExplore = new Queue<Node>(); 

    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake() 
    {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            endNode = grid[endCoordinates];
        }
    }

    void Start()
    {
        GenerateNewPath();
    }

    public List<Node> GenerateNewPath()
    {
        return GenerateNewPath(startCoordinates);
    }

    public List<Node> GenerateNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return GeneratePath();
    }

    void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();
        
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighbourCoordinates))
            {
                neighbours.Add(grid[neighbourCoordinates]);
            }
        }

        foreach (Node neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coordinates) && !neighbour.walkingRestricted)
            {
                neighbour.connectedTo = currentSearchNode;
                reached.Add(neighbour.coordinates, neighbour);
                nodesToExplore.Enqueue(neighbour);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.walkingRestricted = false;
        endNode.walkingRestricted = false;
        nodesToExplore.Clear();
        reached.Clear();

        bool isRunning = true;

        nodesToExplore.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while(nodesToExplore.Count > 0 && isRunning)
        {
            currentSearchNode = nodesToExplore.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if(currentSearchNode.coordinates == endCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> GeneratePath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].walkingRestricted;
            grid[coordinates].walkingRestricted = true;
            List<Node> newPath = GenerateNewPath();
            grid[coordinates].walkingRestricted = previousState;

            if (newPath.Count <= 1)
            {
                GenerateNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
