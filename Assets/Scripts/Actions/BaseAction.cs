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

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionsList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionsList)
        {
            // generate AI action on this specific gridPosition to get the ActionValue
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);

            enemyAIActionList.Add(enemyAIAction);
        }

        // we have list with all possible actions, now we sort with the highest ActionValue first
        if (enemyAIActionList.Count > 0)
        {
            // we use Comparer, if returns negative value, the value is larger 
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue.CompareTo(a.actionValue));

            return enemyAIActionList[0]; // return action with highest ActionValue
        }
        else
        {
            // no possible enemy AI actions
            return null; 
        }
    }

    /// <summary>
    /// Returns this action's ActionValue for the action at a particular GridPosition.
    /// As a good example of how this is implemented, see MoveAction.cs
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    private void OnAnyActionStarted()
    {
        AnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnAnyActionComplete()
    {
        AnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

}
