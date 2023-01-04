using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GridPosition GridPosition { get; private set; }
    public MoveAction MoveAction { get; private set; } // TODO: make a generic method to request an action instead of individual fields. We already have the array of actions below

    public SpinAction SpinAction { get; private set; }

    public BaseAction[] BaseActionArray { get; private set; } // contains all available actions for this unit

    private void Awake()
    {
        this.MoveAction = GetComponent<MoveAction>();
        this.SpinAction = GetComponent<SpinAction>();
        this.BaseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this); // place the Unit on the levelGrid
    }

    void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != GridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, GridPosition, newGridPosition); // unit changed to a new gridPosition, we update grid 
            GridPosition = newGridPosition;
        }
    }

}
