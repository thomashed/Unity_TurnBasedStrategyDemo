using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GridPosition GridPosition { get; private set; }
    public MoveAction MoveAction { get; private set; }

    public SpinAction SpinAction { get; private set; }  

    private void Awake()
    {
        this.MoveAction = GetComponent<MoveAction>();
        this.SpinAction = GetComponent<SpinAction>();
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
