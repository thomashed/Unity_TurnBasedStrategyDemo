using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjcetPrefab = null;
    private GridSystem gridSystem;

    private void Awake()
    {
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

}
