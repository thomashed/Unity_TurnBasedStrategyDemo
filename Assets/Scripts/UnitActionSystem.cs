using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{

    [SerializeField] private LayerMask layerMaskUnits; 
    [SerializeField] private Unit selectedUnit = null;

    public event EventHandler SelectedUnitChanged;
    public static UnitActionSystem Instance;
    private bool isBusy = false;

    public Unit SelectedUnit { get => selectedUnit; } // we're using a backing field, as Unity doesn't allow us to serialize props
    public BaseAction SelectedAction { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create more than one instance! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(this.SelectedUnit);
    }

    private void Update()
    {
        if (isBusy) return;

        if (TryeHandleUnitSelection()) return; // see if we clicked a unit, if so, select said unit
        if (SelectedUnit == null) return;        

        HandleSelectedAction();
    }

    /// <summary>
    /// Will check which action is currently selected and invoke said action
    /// </summary>
    private void HandleSelectedAction() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            // v1: check action type, and call that action's unique action method, as they require different params
            switch (SelectedAction) 
            {
                case MoveAction moveAction:
                    var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                    if (!moveAction.IsValidActionGridPosition(mouseGridPosition)) return; // if we clicked on invalid position, stop execution
                    SetBusy();
                    moveAction.Move(mouseGridPosition, ClearBusy);
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.Spin(ClearBusy);
                    break;
            }

        }
    }

    private bool TryeHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMaskUnits))
            {
                return false;
            }

            if (hit.collider.transform.TryGetComponent<Unit>(out Unit newUnit))
            {
                SetSelectedUnit(newUnit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit newUnit) 
    { 
        selectedUnit = newUnit;
        SetSelectedAction(newUnit.MoveAction); // we know that all units always have a MoveAction
        OnSelectedUnitChanged();
    }

    public void SetSelectedAction(BaseAction baseAction) 
    {
        SelectedAction = baseAction;
    }

    private void OnSelectedUnitChanged()
    {
        SelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetBusy()
    {
        this.isBusy = true;
    }
    private void ClearBusy()
    {
        this.isBusy = false;
    }

}
