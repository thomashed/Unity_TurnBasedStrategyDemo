using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<StartShootingEventArgs> StartShooting;

    public class StartShootingEventArgs : EventArgs
    {
        public Unit targetUnit; // should this be props? constructor?
        public Unit shootingUnit;
    }
    

    [SerializeField] private int maxShootRange = 7;
    public int MaxShootRange { get => maxShootRange; private set { } }

    public Unit TargetUnit { get; private set; }
    private bool canShootBullet;

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

        switch (state)
        {
            case State.Aiming:
                RotateTowardsTarget();
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:

                break;
        }

        // check our state when stateTimer is ticking
        if (stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void RotateTowardsTarget()
    {
        var rotationSpeed = 15f;
        var aimDirection = (TargetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);
    }

    private void Shoot()
    {
        OnStartShooting();
        TargetUnit.Damage(40);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.2f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        var unitGridPosition = Unit.GridPosition;

        for (int x = -maxShootRange; x <= maxShootRange; x++)
        {
            for (int z = -maxShootRange; z <= maxShootRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue; // are we inside the grid?

                int testDistance = Math.Abs(x) + Math.Abs(z); // make the range "circular" for shooting 
                if (testDistance > maxShootRange) continue; 

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
        TargetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        var aimingStateTime = 1.5f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    private void OnStartShooting()
    {
        var eventArgs = new StartShootingEventArgs();
        
        StartShooting?.Invoke(this, new StartShootingEventArgs
        {
            targetUnit = TargetUnit,
            shootingUnit = Unit
        });
    }

   

}
