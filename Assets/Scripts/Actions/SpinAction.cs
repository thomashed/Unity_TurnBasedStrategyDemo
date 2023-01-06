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
            isActive = false;
            onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        this.totalSpinAmount = 0f;
        this.isActive = true;  
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // trigger the action exactly where the unit is standing, as Spinning doesn't care about movement, thus neither position
        GridPosition unitGridPosition = unit.GridPosition;
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
        return 2;
    }
}
