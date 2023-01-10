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
    private GridSystem gridSystem;

    public int Width 
    {
        get 
        {
            return gridSystem.Width;
        } 
    }

    public int Height 
    {
        get 
        {
            return gridSystem.Height;
        }
    }

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

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjcetPrefab);
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

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public void OnAnyUnitMovedGridPosition()
    {
        AnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

}
