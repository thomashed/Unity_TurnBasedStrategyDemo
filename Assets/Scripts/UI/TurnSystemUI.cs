using CoolBeans.TurnSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnBtn = null;
    [SerializeField] private TextMeshProUGUI turnNumberText = null;

    private void Awake()
    {
        
    }

    private void Start()
    {
        TurnSystem.Instance.TurnChanged += TurnSystem_TurnChanged;

        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        UpdateTurnText();
    }

    private void Update()
    {
        
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = $"Turn {TurnSystem.Instance.TurnNumber}";
    }

    private void TurnSystem_TurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }



}
