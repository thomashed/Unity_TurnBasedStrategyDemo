using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int maxShootDistance = 7;
    private float totalSpinAmount;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive) return;

        var spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
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
        this.totalSpinAmount = 0f;
        this.isActive = true;
    }

}
