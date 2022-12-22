using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    public MoveAction MoveAction { get; private set; }

    private void Awake()
    {
        this.MoveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // place the Unit on the levelGrid
    }

    void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition); // unit changed to a new gridPosition, we update grid 
            gridPosition = newGridPosition;
        }
    }

}
