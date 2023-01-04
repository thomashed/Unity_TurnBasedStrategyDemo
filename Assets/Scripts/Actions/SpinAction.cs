using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive) return;

        var spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0,spinAddAmount,0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }

    public void Spin(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        this.totalSpinAmount = 0f;
        this.isActive = true;  
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
