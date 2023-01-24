using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as a facade into anything going on with the Grid
/// </summary>
public class LevelGrid : MonoBehaviour 
{
    public event EventHandler AnyUnitMovedGridPosition;
    
    [SerializeField] private Transform gridDebugObjcetPrefab = null;
    [SerializeField] private ScriptableObejctGridData gridDataSO = null;
    private GridSystem<GridObject> gridSystem;

    public int Width 
    {
        get 
        {
            return width;
        } 
    }

    public int Height 
    {
        get 
        {
            return height;
        }
    }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create more than one instance! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        this.width = gridDataSO.width;
        this.height = gridDataSO.height;
        this.cellSize = gridDataSO.cellSize;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> gridSystem, GridPosition gridPosition) => new GridObject(gridSystem, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjcetPrefab);
    }

    private void Start()
    {
        PathFinding.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        // only clear the gridObject's Unit, if it's the same unit as this one
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition gridPositionFrom, GridPosition gridPositionTo)
    {
        RemoveUnitAtGridPosition(gridPositionFrom, unit);
        AddUnitAtGridPosition(gridPositionTo, unit);
        OnAnyUnitMovedGridPosition();
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public Door GetDoorAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.Door; 
    }

    public void SetDoorAtGridPosition(GridPosition gridPosition, Door door)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.Door = door;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public void OnAnyUnitMovedGridPosition()
    {
        AnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

}
