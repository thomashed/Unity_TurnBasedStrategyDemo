using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab = null;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] private GameObject gameObject = null;
    private GridSystem<PathNode> gridSystem;

    private int width;
    private int height;
    private float cellSize;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create more than one instance! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        // cycle through all grids and check if we hit a PathNode that is an obstacle
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                // do raycast to check if we hit anything in layer Obstacles
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    // we have obstacle
                    GetNode(x,z).IsWalkable = false;
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>(); // PathNodes still to be searched
        List<PathNode> closedList = new List<PathNode>(); // PathNodes we have already searched 

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        // first reset all path nodes in our grid
        for (int x = 0; x < gridSystem.Width; x++)
        {
            for (int z = 0; z < gridSystem.Height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.Gcost = int.MaxValue;
                pathNode.Hcost = 0;
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();

            }
        }

        startNode.Gcost = 0; // since we are on the start node
        startNode.Hcost = CalculateDistance(startGridPosition, endGridPosition); // caluclate cost from start to finish    
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            // check if current node is also our final node
            if (currentNode == endNode)
            {
                pathLength = endNode.Fcost;
                // reached final node
                return CalculatePath(endNode);
            }

            // remove the node we're analysing now from openlist, and log it into closedList
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // now search neighbours of the current node
            // get the neighbour with lowest F value
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                // check if this neighbour is in closed list, means we already searched it
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.IsWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                // gCost based on movement cost to this neighbour node + currentnode's gCost
                int tentativeGCost = 
                    currentNode.Gcost + CalculateDistance(currentNode.GridPosition, neighbourNode.GridPosition);

                if (tentativeGCost < neighbourNode.Gcost)
                {
                    neighbourNode.CameFromPathNode = currentNode;
                    neighbourNode.Gcost = tentativeGCost;
                    neighbourNode.Hcost = CalculateDistance(neighbourNode.GridPosition, endGridPosition);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // no path found
        pathLength = 0;
        return null;
    }

    // calcualte a Heuristic for the distance 
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.X);
        int zDistance = Mathf.Abs(gridPositionDistance.Z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].Fcost < lowestFCostPathNode.Fcost)
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode; 
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GridPosition;

        if (gridPosition.X - 1 >= 0)
        {
            // left
            neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z + 0));

            // left down
            if (gridPosition.Z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z - 1));
            }

            // left up
            if (gridPosition.Z + 1 < gridSystem.Height)
            {
                neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z + 1));
            }
        }

        if (gridPosition.X + 1 < gridSystem.Width)
        {
            // right
            neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z + 0));
            
            // right down
            if (gridPosition.Z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z - 1));
            }

            // right up
            if (gridPosition.Z + 1 < gridSystem.Height)
            {
                neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z + 1));
            }   
        }

        // down
        if (gridPosition.Z - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.X + 0, gridPosition.Z - 1));
        }

        // up
        if (gridPosition.Z + 1 < gridSystem.Height)
        {
            neighbourList.Add(GetNode(gridPosition.X + 0, gridPosition.Z + 1));
        }   

        return neighbourList;
    }

    // returns the path, by walking back from endNode, and checking all nodes "CameFrom" 
    // prop, until said prop is null --> means we've reached endOfPath, i.e. the start
    // in this case
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        // walk back to find complete path
        PathNode currentNode = endNode;
        while (currentNode.CameFromPathNode != null)
        {
            pathNodeList.Add(currentNode.CameFromPathNode);
            currentNode = currentNode.CameFromPathNode;
        }

        pathNodeList.Reverse();
        
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GridPosition);
        }

        return gridPositionList;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x,z));
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        GetNode(gridPosition.X, gridPosition.Z).IsWalkable = isWalkable;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable;
    }

    // check if there's a path to the given gridposition, if not, we shouldn't show it as a position we can walk to
    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition,endGridPosition, out int pathLength);
        return pathLength;
    }

}
