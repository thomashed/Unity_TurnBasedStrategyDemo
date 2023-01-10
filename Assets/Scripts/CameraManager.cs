using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject = null;

    private void Start()
    {
        BaseAction.AnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.AnyActionCompleted += BaseAction_OnAnyActionComplete;
    
        HideActionCamera(); // hide this camera by default
    }

    private void Update()
    {
        
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);    
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) 
    {
        // check if its a shootaction, only then should we activate the actionCamera
        switch (sender)
        {
            case ShootAction shootAction:

                var shooterUnit = shootAction.Unit;
                var targetUnit = shootAction.TargetUnit;

                var cameraCharacterHeight = Vector3.up * 1.7f;

                var shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                var shoulderOffsetAmount = 0.5f;
                var shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                var actionCameraPosition = 
                    shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset + 
                    (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

}
