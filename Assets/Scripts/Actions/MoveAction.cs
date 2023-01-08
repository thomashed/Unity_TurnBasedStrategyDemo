using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolBeans.Grid;
using System;

public class MoveAction : BaseAction
{
    private Vector3 targetDest = Vector3.zero;
    [SerializeField] private Animator unitAnimator = null;
    [SerializeField] private float stoppingDistance = 0.01f;
    private float rotationSpeed = 15f;
    private float movingSpeed = 5f;

    [SerializeField] private int maxMoveDistance = 4;

    protected override void Awake()
    {
        base.Awake();   
        targetDest = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;
        if (Vector3.Distance(transform.position, targetDest) < stoppingDistance)
        {
            unitAnimator.SetBool("IsWalking", false);
            ActionComplete();
            return;
        }
        else 
        {
            CheckUnitMovement();
        }
    }

    private void CheckUnitMovement()
    {
        unitAnimator.SetBool("IsWalking", true);
        var newPos = (targetDest - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, newPos, Time.deltaTime * rotationSpeed);
        transform.position += newPos * movingSpeed * Time.deltaTime;
    }

    // return a list of valid gridPositions
    // we want the unit's transform to be at the center of a grid 
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        var unitGridPosition = unit.GridPosition;

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testGridPosition == unitGridPosition) continue; // the position where the unit currently/already is, it's not valid 
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue; // if there's already a unit on the position, it's not valid

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetDest = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
