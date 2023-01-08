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
    [SerializeField] private GameObject enemyTurnVisualGameObject = null;

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
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
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
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn);
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);
    }

}
