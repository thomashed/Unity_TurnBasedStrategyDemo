using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{

    [SerializeField] private Unit unit = null;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.SelectedUnitChanged += SelectedUnitChanged;
        //UpdateSelectionVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.SelectedUnitChanged -= SelectedUnitChanged;
    }

    private void SelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateSelectionVisual();
    }

    private void UpdateSelectionVisual()
    {
        meshRenderer.enabled = UnitActionSystem.Instance.SelectedUnit == unit;    
    }
}
