using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private const string RECALCULATE_PATH = "RecalculatePath";

    [SerializeField] private Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] private Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    private Node startNode;
    private Node destinationNode;
    private Node currentSearchNode;

    private Queue<Node> unexploredNodes = new Queue<Node>();
    private Dictionary<Vector2Int, Node> knownCoordNodes = new Dictionary<Vector2Int, Node>();

    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager gridManager;
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager.Grid != null) {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    public List<Node> GetNewPath() {
        gridManager.ResetNodes();
        BreadthFirstSearch();
        return BuildPath();
    }

    private void ExploreNeighbors() {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions) {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;
            if (grid.TryGetValue(neighborCoordinates, out Node neighborNode)) {
                neighbors.Add(neighborNode);
            }
        }

        foreach (Node neighbor in neighbors) {
            if (!(knownCoordNodes.ContainsKey(neighbor.coordinates)) && neighbor.isWalkable) {
                neighbor.connectedTo = currentSearchNode;
                knownCoordNodes.Add(neighbor.coordinates, neighbor);
                unexploredNodes.Enqueue(neighbor);
            }
        }
    }

    private void BreadthFirstSearch() {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        unexploredNodes.Clear();
        knownCoordNodes.Clear();

        bool isRunning = true;

        unexploredNodes.Enqueue(startNode);
        knownCoordNodes.Add(startCoordinates, startNode);

        while (unexploredNodes.Count > 0 && isRunning) {
            currentSearchNode = unexploredNodes.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates) {
                isRunning = false;
            }
        }
    }

    private List<Node> BuildPath() {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null) {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates) {
        if (grid.ContainsKey(coordinates)) {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1) {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers() {
        BroadcastMessage(RECALCULATE_PATH, SendMessageOptions.DontRequireReceiver);
    }
}
