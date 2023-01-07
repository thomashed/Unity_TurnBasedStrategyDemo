using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro = null;
    [SerializeField] private Button button = null;
    [SerializeField] private GameObject selectedGameObject = null;

    public BaseAction BaseAction { get; private set; }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.BaseAction = baseAction;   
        this.textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(() => {
            // set selectedAction to this^ baseAction
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual() 
    {
        var selectedBaseAction = UnitActionSystem.Instance.SelectedAction;
        selectedGameObject.SetActive(selectedBaseAction == BaseAction); 
    }



}
