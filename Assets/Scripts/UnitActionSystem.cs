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
        MouseWorld.InputPrimaryClicked += InputPrimaryClicked;
    }

    private void Start()
    {
        SetSelectedUnit(this.SelectedUnit);
    }

    private void Update()
    {
        if (isBusy) return;

        if (Input.GetKeyDown(KeyCode.Y) && SelectedUnit is not null)
        {
            SetBusy();
            SelectedUnit.SpinAction.Spin(ClearBusy);
        }
    }


    private void InputPrimaryClicked(object sender, EventArgsPrimaryInput e)
    {
        if (isBusy) return;
        // see if we clicked a unit, if so, try and select said unit
        if (TrySelectNewUnit()) return;
        if (SelectedUnit == null) return;

        var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (SelectedUnit.MoveAction.IsValidActionGridPosition(mouseGridPosition))
        {
            SetBusy();
            SelectedUnit.MoveAction.Move(mouseGridPosition, ClearBusy);
        }


    }

    private bool TrySelectNewUnit()
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
