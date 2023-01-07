using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBusyUI : MonoBehaviour
{
    
    private void Start()
    {
        UnitActionSystem.Instance.BusyChanged += UnitActionSystem_OnBusyChanged;
        gameObject.SetActive(false); // hide busy button by default
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }

}
