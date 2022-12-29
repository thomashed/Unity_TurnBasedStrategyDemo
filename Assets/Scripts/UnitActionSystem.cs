using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{

    [SerializeField] private LayerMask layerMaskUnits;

    public event EventHandler SelectedUnitChanged;
    public static UnitActionSystem Instance;
    public Unit SelectedUnit { get; private set; }

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


    private void InputPrimaryClicked(object sender, EventArgsPrimaryInput e)
    {
        // see if we clicked a unit, if so, try and select said unit
        if (TrySelectNewUnit()) return;
        if (SelectedUnit == null) return;

        var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (SelectedUnit.MoveAction.IsValidActionGridPosition(mouseGridPosition))
        {
            SelectedUnit.MoveAction.Move(mouseGridPosition);
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
        SelectedUnit = newUnit;
        OnSelectedUnitChanged();
    }

    private void OnSelectedUnitChanged()
    {
        SelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
}
