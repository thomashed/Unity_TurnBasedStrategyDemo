using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab = null;
    [SerializeField] private Transform actionButtonContainerTransform = null;
    
    private void Start()
    {
        UnitActionSystem.Instance.SelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons(); // call from the beginning, in case we have a unit selected from the start    
    }

    private void Update()
    {

    }

    private void CreateUnitActionButtons()
    {
        // before populating the UI container, we should empty it first, to remove old buttons
        foreach (Transform actionButtonTransform in actionButtonContainerTransform)
        {
            Destroy(actionButtonTransform.gameObject);    
        }

        var selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        if (selectedUnit == null) return; // we also call this method on start, but we don't necesarrily have a default selected unit

        foreach (var baseAction in selectedUnit.BaseActionArray)
        {
            var actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            var actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

}
