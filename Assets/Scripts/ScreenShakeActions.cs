using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{

    private void Start()
    {
        ShootAction.AnyStartShooting += ShootAction_AnyStartShooting;
    }

    private void ShootAction_AnyStartShooting(object sender, ShootAction.StartShootingEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
