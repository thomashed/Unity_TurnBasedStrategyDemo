using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{


    [SerializeField] private Transform gridDebugObjcetPrefab = null;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        // gridSystem = new GridSystem<GridObject>(10, 10, 2f, (GridSystem<GridObject> gridSystem, GridPosition gridPosition) => new GridObject(gridSystem, gridPosition));
        // TODO: we should ideally only define one grid, as right now we also define it in LevelGrid
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjcetPrefab);

    }

}
