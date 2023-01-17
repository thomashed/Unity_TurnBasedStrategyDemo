using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolBeans.Grid;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler StartMoving;
    public event EventHandler StopMoving;

    [SerializeField] private float stoppingDistance = 0.01f;
    private float rotateSpeed = 15f;
    private float moveSpeed = 5f;

    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList; // worldPos of all GridPositions we should follow to reach targetPos
    private int currentPositionIndex; // to know which position we're on in our positionLIst

    private void Update()
    {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // keep moving
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                // we reached target 
                OnStopMoving();
                ActionComplete();
            }
        }

        //if (Vector3.Distance(transform.position, targetDest) < stoppingDistance)
        //{
        //    OnStopMoving();
        //    ActionComplete();
        //    return;
        //}
        //else 
        //{
        //    CheckUnitMovement();
        //}
    }

    // return a list of valid gridPositions
    // we want the unit's transform to be at the center of a grid 
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        var unitGridPosition = Unit.GridPosition;

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testGridPosition == unitGridPosition) continue; // the position where the unit currently/already is, it's not valid 
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue; // if there's already a unit on the position, it's not valid
                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition)) continue;
                int pathFindingDistanceMultiplier = 10; // because we multiplied with 10 in PathFinding so we could work with ints instead of floats
                if (PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier) continue;   
                
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(Unit.GridPosition, gridPosition, out int pathLength);
        currentPositionIndex = 0;

        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving();
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        // get ShootAction to find out if our potential moveTo positions contain shootable targets within range(at that position)
        int targetCountAtGridPosition = Unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition, 
            actionValue = targetCountAtGridPosition * 10,
        };
    }

    private void OnStartMoving()
    {
        StartMoving?.Invoke(this, EventArgs.Empty);
    }
    private void OnStopMoving()
    {
        StopMoving?.Invoke(this, EventArgs.Empty);
    }

}
