using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as a facade into anything going on with the Grid
/// </summary>
public class LevelGrid : MonoBehaviour 
{
    [SerializeField] private Transform gridDebugObjcetPrefab = null;
    private GridSystem gridSystem;

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

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).Unit = unit;
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) 
    {
        return gridSystem.GetGridObject(gridPosition).Unit;
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        gridSystem.GetGridObject(gridPosition).Unit = null;
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition gridPositionFrom, GridPosition gridPositionTo)
    {
        ClearUnitAtGridPosition(gridPositionFrom);
        SetUnitAtGridPosition(gridPositionTo, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

}
