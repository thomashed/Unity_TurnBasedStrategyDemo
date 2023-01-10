using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI actionPointsText = null;
    [SerializeField] private Unit unit = null;
    [SerializeField] private Image healthBarImage = null;
    [SerializeField] private HealthSystem healthSystem = null;

    private void Start()
    {
        Unit.AnyPointsChanged += Unit_OnAnyPointsChanged;
        healthSystem.Damaged += HealthSystem_Damaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    public void UpdateActionPointsText()
    {
        actionPointsText.text = unit.ActionPoints.ToString();
    }

    public void Unit_OnAnyPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void HealthSystem_Damaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

}
