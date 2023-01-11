using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive) return;

        var spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0,spinAddAmount,0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.totalSpinAmount = 0f;
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // trigger the action exactly where the unit is standing, as Spinning doesn't care about movement, thus neither position
        GridPosition unitGridPosition = Unit.GridPosition;
        return new List<GridPosition>() 
        { 
            unitGridPosition 
        };
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override int GetActionPointCost()
    {
        return 1;
    }
}
