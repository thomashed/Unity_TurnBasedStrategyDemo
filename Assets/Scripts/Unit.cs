using CoolBeans.Grid;
using CoolBeans.TurnSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler AnyPointsChanged;
    public static event EventHandler AnyUnitSpawned;
    public static event EventHandler AnyUnitDead;

    [SerializeField] private bool isEnemy;

    public bool IsEnemy { get { return isEnemy; } }

    public GridPosition GridPosition { get; private set; }
    public HealthSystem HealthSystem { get; private set; }
    public BaseAction[] BaseActionArray { get; private set; } // contains all available actions for this unit
    public int ActionPoints { get; private set; } = ACTION_POINTS_MAX;

    private void Awake()
    {
        this.HealthSystem = GetComponent<HealthSystem>();
        this.BaseActionArray = GetComponents<BaseAction>(); 
    }

    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this); // place the Unit on the levelGrid

        TurnSystem.Instance.TurnChanged += TurnSystem_OnTurnChanged;

        HealthSystem.Dead += HealthSystem_OnDead;
        OnAnyUnitSpawned();
    }

    internal Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != GridPosition)
        {
            var oldGridPosition = GridPosition;
            GridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition); // unit changed to a new gridPosition, we update grid 
        }
    }

    // return action of the requested type
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in BaseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction; 
            }
        }

        return null;
    }
    

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) 
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return ActionPoints >= baseAction.GetActionPointCost();
    }

    public void SpendActionPoints(int amount)
    {
        ActionPoints -= amount;
        OnPointsChanged();
    }

    public void Damage(int damageAmount)
    {
        HealthSystem.Damage(damageAmount);
    }

    public float GetHealthNormalized()
    {
        return HealthSystem.GetHealthNormalized();
    }

    private void OnPointsChanged()
    {
        AnyPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnAnyUnitSpawned()
    {
        AnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void OnAnyUnitDead()
    {
        AnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(GridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead(); // Is this a problem, being called AFTER DEstroy?
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy && !TurnSystem.Instance.IsPlayerTurn) || (!IsEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            ActionPoints = ACTION_POINTS_MAX;
            OnPointsChanged();
        }
    }

}
