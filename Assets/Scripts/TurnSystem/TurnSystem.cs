using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.TurnSystem
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem Instance { get; private set; }
        
        public int TurnNumber { get; private set; } = 1;

        public event EventHandler TurnChanged;

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


        public void NextTurn()
        {
            TurnNumber++;
            OnTurnChanged();
        }

        private void OnTurnChanged() 
        {
            TurnChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}


