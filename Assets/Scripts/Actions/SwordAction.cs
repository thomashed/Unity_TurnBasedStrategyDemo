using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler AnySwordHit;

    public event EventHandler SwordActionStarted;
    public event EventHandler SwordActionCompleted;

    [SerializeField] private int maxSwordDistance = 1;

    public int MaxSwordDistance { 
        get 
        {
            return maxSwordDistance;
        }
    }

    private Unit targetUnit;

    private float stateTimer = 0f;
    private State state;

    private enum State 
    {
        SwingingSwordBeforeHit,    
        SwingingSwordAfterHit
    }

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        // check our state when stateTimer is ticking
        if (stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                OnAnySwordHit();
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCommpleted();
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GridPosition;

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue; // are we inside the grid?
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue; // if no unit on the position, we don't care, as we search for EnemyAI, which is a Unit
                var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (Unit.IsEnemy == targetUnit.IsEnemy) continue; // both on the same team, ignore

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted();

        ActionStart(onActionComplete);
    }

    private void OnSwordActionStarted()
    {
        SwordActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnSwordActionCommpleted()
    {
        SwordActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    private void OnAnySwordHit()
    {
        AnySwordHit?.Invoke(this, EventArgs.Empty);
    }


}
