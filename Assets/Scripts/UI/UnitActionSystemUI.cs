using CoolBeans.TurnSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab = null;
    [SerializeField] private Transform actionButtonContainerTransform = null;
    [SerializeField] private TextMeshProUGUI actionPointsText = null;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        this.actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.SelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.SelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.ActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.TurnChanged += TurnSystem_OnTurnChanged;
        Unit.AnyPointsChanged += Unit_OnPointsChanged;

        UpdateActionPoints();
        CreateUnitActionButtons(); // call from the beginning, in case we have a unit selected from the start    
        //UpdateSelectedVisual(); // TODO: is this needed? As we're already triggering above events from the beginning
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

        actionButtonUIList.Clear();

        var selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        if (selectedUnit == null) return; // we also call this method on start, but we don't necesarrily have a default selected unit

        foreach (var baseAction in selectedUnit.BaseActionArray)
        {
            var actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            var actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UpdateSelectedVisual()
    {
        foreach (var actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        var selectedUnit = UnitActionSystem.Instance.SelectedUnit;
        actionPointsText.text = $"Action Points: {selectedUnit.ActionPoints}";
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) 
    {
        UpdateActionPoints();
    }

    private void Unit_OnPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

}
