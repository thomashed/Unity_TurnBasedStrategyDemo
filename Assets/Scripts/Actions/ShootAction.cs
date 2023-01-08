using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int maxShootDistance = 7;

    private enum State 
    {
        Aiming,
        Shooting, 
        Cooloff,
    }

    private State state;
    private float stateTimer;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        // check our state when stateTimer is down
        if (stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 3.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                isActive = false;
                onActionComplete();
                break;
        }
        print($"ShootAction/NextState: {state}");
    }

    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        var unitGridPosition = unit.GridPosition;

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue; // are we inside the grid?

                int testDistance = Math.Abs(x) + Math.Abs(z); // make the range "circular" for shooting 
                if (testDistance > maxShootDistance) continue; 

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue; // if no unit on the position, we don't care, as we search for EnemyAI, which is a Unit

                var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (unit.IsEnemy == targetUnit.IsEnemy) continue; // both on the same team, ignore
                
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.isActive = true;

        state = State.Aiming;
        var aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        print("ShootAction/TakeAction");
    }

}
