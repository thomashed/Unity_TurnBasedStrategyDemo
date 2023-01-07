using CoolBeans.Grid;
using CoolBeans.TurnSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler PointsChanged;

    [SerializeField] private bool isEnemy;

    public bool IsEnemy { get { return isEnemy; } }

    public GridPosition GridPosition { get; private set; }
    public MoveAction MoveAction { get; private set; } // TODO: make a generic method to request an action instead of individual fields. We already have the array of actions below
    public SpinAction SpinAction { get; private set; }
    public BaseAction[] BaseActionArray { get; private set; } // contains all available actions for this unit
    public int ActionPoints { get; private set; } = ACTION_POINTS_MAX;

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

        TurnSystem.Instance.TurnChanged += TurnSystem_OnTurnChanged;
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

    

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) 
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return ActionPoints >= baseAction.GetActionPointCost();
    }

    public void SpendActionPoints(int amount)
    {
        ActionPoints -= amount;
        OnPointsChanged();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        ActionPoints = ACTION_POINTS_MAX;
        OnPointsChanged();
    }

    private void OnPointsChanged()
    {
        PointsChanged?.Invoke(this, EventArgs.Empty);
    }

}
