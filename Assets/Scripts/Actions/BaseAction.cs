using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler AnyActionStarted;
    public static event EventHandler AnyActionCompleted;

    public Unit Unit { get; protected set; }
    protected bool isActive = false;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        this.Unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        var validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost() 
    { 
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted();
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionComplete();
    }

    private void OnAnyActionStarted()
    {
        AnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnAnyActionComplete()
    {
        AnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

}
